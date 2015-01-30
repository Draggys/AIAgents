using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DiscreteMovement : MonoBehaviour {

	AStar astar;
	Grid grid;

	float timeToGo;
	public float delaySec;

	public void RequestPath() {
		Node startNode = grid.NodeFromWorldPoint (grid.mapData.start);
		Node endNode = grid.NodeFromWorldPoint (grid.mapData.end);
		astar.AStarSearch (startNode, endNode);
	}

	public void RequestPath(Node start, Node end) {
		astar.AStarSearch (start, end);
	}

	void Start () {
		astar = new AStar ();
		grid = GameObject.FindGameObjectWithTag ("Grid").GetComponent<Grid> ();
		RequestPath ();

		timeToGo = Time.fixedTime;
		delaySec = 0.5f;
	}


	void FixedUpdate() {
		if (Time.fixedTime >= timeToGo) {
			timeToGo = Time.fixedTime + delaySec;
			if (grid.path.Count > 0) {
				transform.position = grid.path[0].worldPosition;
				grid.path.RemoveAt (0);
			}
		}
	}
}
