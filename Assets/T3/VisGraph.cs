using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class VisGraph : MonoBehaviour {

	List<Obstacle> obstacles = new List<Obstacle> ();
	PolyData polyData = null;

	List<Line> walkableLines;

	void Start() {
		PolyMapLoader loader = new PolyMapLoader ("x", "y", "goalPos", "startPos", "button");
		polyData = loader.polyData;

		CreateObstacles ();
		ConstructWalkableLines ();
		print ("Walkable lines: " + walkableLines.Count);
	}

	// *Specific case* Nodes
	// 0 -> 3 : top left
	// 4 -> 9 : middle
	// 10 -> 13 : middle right
	// 14 -> 17 : down right
	// 18 -> 22 : down left
	public void CreateObstacles() {
		Obstacle obstacle = new Obstacle();
		for(int i = 0; i <= 3; i++) {
			obstacle.vertices.Add (polyData.nodes[i]);
			obstacle.id = i;
			if(i != 3) {
				obstacle.edges.Add (new Line(polyData.nodes[i], polyData.nodes[i+1]));
			}
			else { 
				obstacle.edges.Add (new Line(polyData.nodes[i], polyData.nodes[0]));
			}
		}

		obstacles.Add (new Obstacle(obstacle));
		obstacle.edges.Clear ();
		obstacle.vertices.Clear ();

		for(int i = 4; i <= 9; i++) {
			obstacle.id = i;
			obstacle.vertices.Add (polyData.nodes[i]);
			if(i != 9)
				obstacle.edges.Add (new Line(polyData.nodes[i], polyData.nodes[i+1]));
			else 
				obstacle.edges.Add (new Line(polyData.nodes[i], polyData.nodes[4]));
		}
		obstacles.Add (new Obstacle(obstacle));
		obstacle.edges.Clear ();
		obstacle.vertices.Clear ();

		for(int i = 10; i <= 13; i++) {
			obstacle.id = i;
			obstacle.vertices.Add (polyData.nodes[i]);
			if(i != 13)
				obstacle.edges.Add (new Line(polyData.nodes[i], polyData.nodes[i+1]));
			else 
				obstacle.edges.Add (new Line(polyData.nodes[i], polyData.nodes[10]));
		}

		obstacles.Add (new Obstacle(obstacle));
		obstacle.edges.Clear ();
		obstacle.vertices.Clear ();

		for(int i = 14; i <= 17; i++) {
			obstacle.id = i;
			obstacle.vertices.Add (polyData.nodes[i]);
			if(i != 17)
				obstacle.edges.Add (new Line(polyData.nodes[i], polyData.nodes[i+1]));
			else 
				obstacle.edges.Add (new Line(polyData.nodes[i], polyData.nodes[14]));
		}
		
		obstacles.Add (new Obstacle(obstacle));
		obstacle.edges.Clear ();
		obstacle.vertices.Clear ();

		for(int i = 18; i <= 22; i++) {
			obstacle.id = i;
			obstacle.vertices.Add (polyData.nodes[i]);
			if(i != 22)
				obstacle.edges.Add (new Line(polyData.nodes[i], polyData.nodes[i+1]));
			else 
				obstacle.edges.Add (new Line(polyData.nodes[i], polyData.nodes[18]));
		}
		obstacles.Add (obstacle);
	}

	void ConstructWalkableLines() {
		walkableLines = new List<Line> ();

		// For every obstacle and it's neighbours
		// See if a line can be drawn from each vertex from the current obstacle to 
		// it's neighbours' vertices.
		// If the line does not intersect with any other line, add it to walkableLines
		foreach (Obstacle obs in obstacles) {
			foreach (Obstacle neigh in obstacles) {
				if(obs != neigh) {
					foreach(Vector3 vertex in obs.vertices) {
						foreach(Vector3 neighVertex in neigh.vertices) {
							Line potentialLine = new Line(vertex, neighVertex);
							if(!IntersectsWithAnyLine(potentialLine)){
								walkableLines.Add (potentialLine); //debugging
							}
						}
					}
				}
			}
		}
	}

	bool IntersectsWithAnyLine(Line myLine) {
		foreach (Obstacle obs in obstacles) {
			foreach (Line line in obs.edges) {
				if (myLine.point1 == line.point1 || myLine.point1 == line.point2)
					continue;
				if (myLine.point2 == line.point1 || myLine.point2 == line.point2)
					continue;

				if (myLine.intersect (line)) {
					//print (myLine.point1 + ", " + myLine.point2 + " vs " + line.point1 + ", " + line.point2);
					return true;
				}
			}
		}
		return false;
		/*
		foreach (Line line in obstacles[0].edges) {
			if(myLine.point1 == line.point1 || myLine.point1 == line.point2)
				continue;
			if(myLine.point2 == line.point1 || myLine.point2 == line.point1)
				continue;

			print (myLine.point1 + ", " + myLine.point2 + " vs " + line.point1 + ", " + line.point2);
			if(myLine.intersect (line)) {
				return true;
			}
		}
		return false;
		*/
		/*
		foreach (Obstacle obs in obstacles) {
			foreach(Line line in obs.edges) {
				if(line.intersect (myLine))
					return true;
			}
		}
		return false;
		*/
    }
    
    
    void OnDrawGizmos() {
		if (polyData != null) {
			for(int i = 0; i <= 22; i++) {
				Gizmos.color = Color.white;
				Gizmos.DrawCube (polyData.nodes[i], Vector3.one);
			}
			
			Gizmos.color = Color.green;
			Gizmos.DrawCube (polyData.start, Vector3.one);
			
			Gizmos.color = Color.red;
			Gizmos.DrawCube (polyData.end, Vector3.one);

			Gizmos.color = Color.cyan;
			foreach(Obstacle ob in obstacles) {
				foreach(Line line in ob.edges) {
					Gizmos.DrawLine (line.point1, line.point2);
				}
			}
		}

		if (walkableLines != null) {
			foreach(Line line in walkableLines) {
				Gizmos.color = Color.red;
				Gizmos.DrawLine(line.point1, line.point2);
			}
		}
	}	
}
