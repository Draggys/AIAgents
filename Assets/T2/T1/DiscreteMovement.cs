using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DiscreteMovement : MonoBehaviour {

	AStar astar;
	Grid grid;

	float timeToGo;
	public float delaySec;
	List<Node> path = new List<Node>();

	public void RequestPath() {
		Node startNode = grid.grid [Convert.ToInt32(grid.mapData.start.x), Convert.ToInt32 (grid.mapData.start.y)];
		Node endNode = grid.grid [Convert.ToInt32 (grid.mapData.end.x), Convert.ToInt32 (grid.mapData.end.y)];


		startNode = grid.grid [0, 1];
		endNode = grid.grid [19, 17];
		path = astar.AStarSearch (startNode, endNode);
	}

	public void RequestPath(Node start, Node end) {
		path = astar.AStarSearch (start, end);
	}

	void Start () {
		grid = GameObject.FindGameObjectWithTag ("Grid").GetComponent<Grid> ();
		astar = new AStar ();
		timeToGo = Time.fixedTime;
		delaySec = 0.5f;

		RequestPath ();
	}
	
	void FixedUpdate() {
		if (Time.fixedTime >= timeToGo) {
			timeToGo = Time.fixedTime + delaySec;
				if (path.Count > 0) {
				transform.position = path[0].worldPosition;
				path.RemoveAt (0);
			}
		}
	}

	void OnDrawGizmos() {
		foreach (Node n in path) {
			Gizmos.color = Color.black;
			Gizmos.DrawCube (n.worldPosition, Vector3.one * (0.5f - .1f));
		}
	}
}
