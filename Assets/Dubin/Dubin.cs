using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Circle {
	public Vector3 pos;
	public string type;

	public Circle(Vector3 pos, string type) {
		this.pos = pos;
		this.type = type;
	}
}

public class Dubin : MonoBehaviour {
	// Debug
	public Circle[] proxCircles = null;
	public List<Line> tangents = null;


	// Debug
	/*
	Vector3 start;
	Vector3 startDir;
	Vector3 goal;
	Vector3 goalDir;
	List<KeyValuePair<Vector3, float>> circles = null; // Debugging
	Line winningTangent = null; // debug
*/
	/*
	void Start(){
		start = transform.position;
		startDir = transform.position + transform.forward * 2;
		goal = new Vector3 (10, 1, 10);
		Transform tmp = new GameObject ().transform;
		tmp.position = goal;
		Quaternion theta = Quaternion.LookRotation (start - goal);
		tmp.rotation = Quaternion.RotateTowards (tmp.rotation, theta, 9000);
		//tmp.rotation = transform.rotation;
		goalDir = tmp.position + tmp.forward * 2;

		CSCTrajectories (start, goal, transform.rotation, tmp.rotation, minRadius, minRadius);
		winningTangent = GetShortestTangent ();
		/*
		tangents = new List<Line> ();
		List<Line> innerTangents = CalculateTangents (p1, p2, minRadius, minRadius, "inner");
		List<Line> outerTangents = CalculateTangents (p1, p2, minRadius, minRadius, "outer");
		tangents.AddRange (innerTangents);
		tangents.AddRange (outerTangents);

	}
*/

	public Line MinTrajectory(Vector3 start, Vector3 goal, Quaternion startAngle, Quaternion goalAngle,
	                          float r1, float r2) {
		Line SI = CSCMinTrajectoryInner(start, goal, startAngle, goalAngle, r1, r2);
		Line SO = CCCMinTrajectoryOuter(start, goal, startAngle, goalAngle, r1, r2);
		if (SI == null && SO != null)
			return SO;
		else if (SO == null && SI != null)
			return SI;
		else if (Vector3.Distance (SI.point2, SI.point1) < Vector3.Distance (SO.point2, SO.point1))
			return SI;
		else
			return SO;
	}

	public Line CCCMinTrajectoryOuter(Vector3 start, Vector3 goal, Quaternion startAngle, Quaternion goalAngle,
	                             float r1, float r2) {
		CSCTrajectories(start, goal, startAngle,goalAngle, r1, r2, "outer");
		return GetShortestTangent ();
	}

	public Line CSCMinTrajectoryInner(Vector3 start, Vector3 goal, Quaternion startAngle, Quaternion goalAngle,
	                   float r1, float r2) {
		CSCTrajectories(start, goal, startAngle,goalAngle, r1, r2, "inner");
		return GetShortestTangent ();
	}

	Line GetShortestTangent() {
		Line ret = null;
		float shortest = -1;
		foreach (Line tangent in tangents) {
			float dist = Vector3.Distance (tangent.point1, tangent.point2);
			if(shortest == -1 || dist < shortest) {
				shortest = dist;
				ret = tangent;
			}
		}	

		return ret;
	}

	void CSCTrajectories(Vector3 start, Vector3 goal, Quaternion startAngle, Quaternion goalAngle,
	                    float r1, float r2, string type) {
		tangents = new List<Line> ();
		proxCircles = GetProximityCircles (start, goal, startAngle, goalAngle, r1, r2);
		List<Line> possibleTangents;

		// RSR
		if(type == "inner")
			possibleTangents = CalculateTangents (proxCircles [0].pos, proxCircles [2].pos, r1, r2, "inner");
		else
			possibleTangents = CalculateTangents (proxCircles [0].pos, proxCircles [2].pos, r1, r2, "outer");
		foreach (Line tangent in possibleTangents) {
			float angle = Vector3.Angle (Vector3.Normalize(transform.forward), Vector3.Normalize (tangent.point1 - start));
			if(angle < 90){
				tangents.Add (tangent);
			}
		}

		//RSL
		if(type == "inner")
			possibleTangents = CalculateTangents (proxCircles [0].pos, proxCircles [3].pos, r1, r2, "inner");
		else
			possibleTangents = CalculateTangents (proxCircles [0].pos, proxCircles [3].pos, r1, r2, "outer");
		foreach (Line tangent in possibleTangents) {
			float angle = Vector3.Angle (Vector3.Normalize(transform.forward), Vector3.Normalize (tangent.point1 - start));
			if(angle < 90){
				tangents.Add (tangent);
			}
		}
		
		// LSR
		if(type == "inner")
			possibleTangents = CalculateTangents (proxCircles [1].pos, proxCircles [2].pos, r1, r2, "inner");
		else
			possibleTangents = CalculateTangents (proxCircles [1].pos, proxCircles [2].pos, r1, r2, "outer");
		foreach (Line tangent in possibleTangents) {
			float angle = Vector3.Angle (Vector3.Normalize(-transform.forward), Vector3.Normalize (tangent.point1 - start));
			if(angle < 90){
				tangents.Add (tangent);
			}
		}

		// LSL
		if(type == "inner")
			possibleTangents = CalculateTangents (proxCircles [1].pos, proxCircles [3].pos, r1, r2, "inner");
		else
			possibleTangents = CalculateTangents (proxCircles [1].pos, proxCircles [3].pos, r1, r2, "outer");
		foreach (Line tangent in possibleTangents) {
			float angle = Vector3.Angle (Vector3.Normalize(-transform.forward), Vector3.Normalize (tangent.point1 - start));
			if(angle < 90){
				tangents.Add (tangent);
			}
		}
	}

	Circle[] GetProximityCircles(Vector3 startPos, Vector3 goalPos, Quaternion startAngle, Quaternion goalAngle, 
	                              float r1, float r2) {
		Transform current = new GameObject().transform;
		current.position = startPos;
		current.rotation = startAngle;
		Transform goal = new GameObject().transform;
		goal.position = goalPos;
		goal.rotation = goalAngle;
		Circle[] ret = {new Circle(current.position + current.right * r1, "R"),
			new Circle(current.position - current.right * r1, "L"),
			new Circle(goal.position + goal.right * r2, "R"), 
			new Circle(goal.position - goal.right * r2, "L")};

		Destroy (current.gameObject);
		Destroy (goal.gameObject);
		return ret;
	}

	List<Line> CalculateTangents(Vector3 p1, Vector3 p2, float r1, float r2, string type){
		Vector3 V1 = p2 - p1;
		float D = Vector3.Magnitude (V1);

		Vector3 p3 = new Vector3((p1.x + p2.x)/2, 1, (p1.z + p2.z) / 2);
		float r3 = D / 2;

		float r4 = r1 + r2;
		if (type == "outer") {
			r4 = r1 - r2; // Assumption: r1 >= r2
		}

		/*
		float theta = vel / L + Mathf.Atan2 (V1.z, V1.x);
		Vector3 pt = new Vector3 (p1.x + r4 * Mathf.Cos (theta), 1,
		                          p1.z + r4 * Mathf.Sin (theta));
		                          */

		List<Vector3> pts = IntersectionPoints (p1, p3, r4, r3);
		if (pts == null) {
			print ("No intersection was found");
			return new List<Line> ();
		}

		List<Line> ret = new List<Line> ();
		Vector3[] tangent = CalculateTangentHelper (pts [0], p1, p2, r1);
		ret.Add (new Line (tangent [0], tangent [1]));
		if (pts.Count == 2) {
			tangent = CalculateTangentHelper (pts [1], p1, p2, r1);
			ret.Add (new Line (tangent [0], tangent [1]));
		}

		// DEBUGGING
		/*
		circles = new List<KeyValuePair<Vector3, float> > ();
		circles.Add (new KeyValuePair<Vector3, float> (p1, r4));
		circles.Add (new KeyValuePair<Vector3, float> (p3, r3));
		circles.Add (new KeyValuePair<Vector3, float> (p2, r2));
		circles.Add (new KeyValuePair<Vector3, float> (p1, r1));
		*/
		return ret;
	}
	
	Vector3[] CalculateTangentHelper(Vector3 pt, Vector3 p1, Vector3 p2, float r1) {
		Vector3 V2 = pt - p1;
		Vector3 V3 = Vector3.Normalize (V2) * r1;
		Vector3 pit1 = p1 + V3;
		
		Vector3 V4 = (p2 - pt);
		Vector3 pit2 = new Vector3 (pit1.x + V4.x, 1, pit1.z + V4.z);
		
		Vector3[] ret = {pit1, pit2};
		return ret;
	}

	List<Vector3> IntersectionPoints(Vector3 p1, Vector3 p2, float r1, float r2) {
		float dx = p1.x - p2.x;
		float dy = p1.z - p2.z;
		float dist = Mathf.Sqrt (dx * dx + dy * dy);

		if (dist > r1 + r2) {
			return null;
		}
		else if (dist < Mathf.Abs (r1 - r2)) {
			return null;
		}
		else if ((dist == 0) && (r1 == r2)) {
			return null;
		}
		else {
			float a = (r1 * r1 - r2 * r2 + dist * dist) / (2 * dist);
			float h = Mathf.Sqrt (r1 * r1 - a * a);

			Vector3 center = new Vector3(p1.x + a * (p2.x - p1.x) / dist, 1, 
			                             p1.z + a * (p2.z - p1.z) / dist);

			Vector3 i1 = new Vector3(center.x - h * (p2.z - p1.z) / dist, 1, 
			                         center.z + h * (p2.x - p1.x) / dist);

			Vector3 i2 = new Vector3(center.x + h * (p2.z - p1.z) / dist, 1, 
			                         center.z - h * (p2.x - p1.x) / dist);

			List<Vector3> ret = new List<Vector3> ();
			if(dist == r1 + r2){
				ret.Add (i1);
				return ret;
			}
			else {
				ret.Add (i1);
				ret.Add (i2);
				return ret;
			}
		}
	}

	void OnDrawGizmos() {
		/*
		Gizmos.color = Color.black;
		if (circles != null) {
			foreach (KeyValuePair<Vector3, float> p in circles) {
				Gizmos.DrawWireSphere(p.Key, p.Value);
			}
		}

		Gizmos.color = Color.blue;
		if (tangents != null) {
			foreach(Line line in tangents) {
				Gizmos.DrawLine (line.point1, line.point2);
			}
		}

		Gizmos.color = Color.white;
		if (proxCircles != null) {
			foreach(Circle circle in proxCircles) {
				Gizmos.DrawWireSphere(circle.pos, minRadius);
			}
		}

		Gizmos.color = Color.red;
		Gizmos.DrawLine (start, startDir);
		Gizmos.DrawLine (goal, goalDir);

		Gizmos.color = Color.green;
		if(winningTangent != null)
			Gizmos.DrawLine (winningTangent.point1, winningTangent.point2);
			*/
	}

}
