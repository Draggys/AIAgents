using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DubinWaypoint : MonoBehaviour {

	public Dubin dubin;
	Line dS = null;

	public float carLength = 5f;
	public float maxWheelAngle = 87f;
	public bool carMadeIt = false;
	public float vel = 1;
	float minRadius = 0f;

	List<Vector3> path = null;
	

	void Start () {
		minRadius = Mathf.Abs (carLength / (Mathf.Tan (maxWheelAngle) ));
		dubin = gameObject.AddComponent<Dubin> ();

		List<Vector3> prePath = new List<Vector3> ();
		prePath.Add (Vector3.zero);
		prePath.Add (new Vector3 (10, 1, 10));

		path = new List<Vector3> ();
		Vector3 start = prePath [0];
		Vector3 goal = prePath [1];

		Transform tmp = new GameObject ().transform;
		Quaternion theta = Quaternion.LookRotation (start - goal);
		tmp.rotation = Quaternion.RotateTowards (tmp.rotation, theta, 9000);

		// Two different goal angles
		//Line S = dubin.CSCMinTrajectory (start, goal, transform.rotation, tmp.rotation, minRadius, minRadius);
		Line S = dubin.CSCMinTrajectory (start, goal, transform.rotation, transform.rotation, minRadius, minRadius);
		dS = S;

		path.Add (prePath [0]);
		path.Add (S.point1);
		path.Add (S.point2);
		path.Add (prePath [1]);

		StartCoroutine ("Move");

		
	//	Destroy (tmp.gameObject);
	}

	int index = 1;
	Vector3 current = Vector3.zero;
	IEnumerator Move() {
		current = path [index];
		while (true) {
			if(carMadeIt) {
				index++;
				if(index >= path.Count)
					yield break;
				current = path[index];
				carMadeIt = false;
			}

			float wheelAngleRad=maxWheelAngle*(Mathf.PI/180);
			float dTheta=(vel/carLength)*Mathf.Tan(wheelAngleRad);
			Quaternion theta = Quaternion.LookRotation (current - transform.position);

			if(transform.rotation!=theta){
				transform.rotation = Quaternion.RotateTowards (transform.rotation, theta, dTheta * Time.deltaTime);
			}

			Vector3 curDir=transform.eulerAngles;
			Vector3 newPos=transform.position;
			float angleRad=curDir.y*(Mathf.PI/180);
			newPos.x=newPos.x+(vel/carLength*Mathf.Sin(angleRad)*Time.deltaTime);
			newPos.z=newPos.z+(vel/carLength*Mathf.Cos(angleRad)*Time.deltaTime);
			transform.position=newPos;

			if(Vector3.Distance (current, transform.position) < 1)
				carMadeIt = true;

			yield return null;
		}
	}

	void OnDrawGizmos() {
		if (dubin != null) {
			Gizmos.color = Color.black;
			if (dubin.proxCircles != null) {
				foreach (Circle circle in dubin.proxCircles) {
					Gizmos.DrawWireSphere (circle.pos, minRadius);
				}
			}

			Gizmos.color = Color.blue;
			if (dubin.tangents != null) {
				foreach (Line line in dubin.tangents) {
					Gizmos.DrawLine (line.point1, line.point2);
				}
			}

			Gizmos.color = Color.green;
			if(dS != null) {
				Gizmos.DrawLine (dS.point1, dS.point2);
			}

			Gizmos.color = Color.red;
			for(int i = index; i < path.Count; i++) {
				Gizmos.DrawCube (path[i], Vector3.one * 0.2f);
			}
		}
	}	
}
