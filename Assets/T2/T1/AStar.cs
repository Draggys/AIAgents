using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar : MonoBehaviour {

	Grid grid;
	Ray ray;

	void Start () {
		grid = GameObject.FindGameObjectWithTag ("Grid").GetComponent<Grid> ();
	}
	
	void Update() {
		//BFS (transform.position, mousePos);
		AStarSearch (grid.NodeFromWorldPoint (transform.position), grid.NodeFromWorldPoint (new Vector3(19, 0, 19)));
		//print (grid.path.Count);
	}

	Node GetNode(Node currentNode, int dir) {
		List<Node> directions = grid.GetNeighbours (currentNode);
		return directions [dir];
	}

	void BFS(Vector3 startPos, Vector3 targetPos) {
		Node startNode = grid.NodeFromWorldPoint (startPos);
		Node targetNode = grid.NodeFromWorldPoint (targetPos);

		Queue<Node> frontier = new Queue<Node> ();
		frontier.Enqueue (startNode);
		Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node> ();
		cameFrom [startNode] = null;

		while (frontier.Count != 0) {
			Node currentNode = frontier.Dequeue();

			if(currentNode == targetNode) {
				ConstructPath (startNode, targetNode, cameFrom);
				break;
			}

			foreach(Node node in grid.GetNeighbours (currentNode)) {
				if(!cameFrom.ContainsKey (node)) {
					if(node.walkable) {
						frontier.Enqueue(node);
						cameFrom[node] = currentNode;
					}
				}
			}
		}
	}

	public void AStarSearch(Node startNode, Node targetNode) {
		PriorityQueue<Node, float> frontier = new PriorityQueue<Node, float> ();
		Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node> ();
		Dictionary<Node, float> costSoFar = new Dictionary<Node, float> ();

		frontier.Enqueue(startNode, 0f);
		cameFrom [startNode] = null;
		costSoFar [startNode] = 0;

		Node currentNode;
		while (frontier.Count() != 0) {
			currentNode = frontier.Dequeue ();

			if(currentNode == targetNode) {
				ConstructPath (startNode, targetNode, cameFrom);
				break;
			}

			foreach (Node node in grid.GetNeighbours(currentNode)) {
				float newCost = costSoFar[currentNode] + grid.GetCost (currentNode, node);
				if (!costSoFar.ContainsKey (node) || newCost < costSoFar[node]) {
					if(node.walkable) {
						costSoFar[node] = newCost;
						float priority = newCost + Heuristic (targetNode.worldPosition, node.worldPosition);
						frontier.Enqueue (node, priority);
						cameFrom[node] = currentNode;
					}
				}
			}
		}
	}

	float Heuristic(Vector3 A, Vector3 B) {
		return Mathf.Abs (A.x - B.x) + Mathf.Abs (A.y - B.y);
	}

	void ConstructPath(Node start, Node target, Dictionary<Node, Node> cameFrom) {
		Node currentNode = target;
		List<Node> path = new List<Node>();
		while (currentNode != start) {
			path.Add (currentNode);
			currentNode = cameFrom[currentNode];
		}
		path.Reverse ();

		grid.path = path;
	}
}
