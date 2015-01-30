using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

	public Transform player;
	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	Node[,] grid;

	float nodeDiameter;
	int gridSizeX;
	int gridSizeY;

	public MapData mapData;
	void Start () {
		MapLoader mapLoader = new MapLoader (new Vector2(20, 20), 0.5f);
		mapData = mapLoader.LoadMap ("A", "endPos", "startPos");

		nodeDiameter = mapData.nodeRadius * 2;
		gridWorldSize = mapData.gridWorldSize;
		// #nodes that can fit into the x-space
		gridSizeX = Mathf.RoundToInt (gridWorldSize.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt (gridWorldSize.y / nodeDiameter);
		print(gridSizeX);
		CreateGrid ();

		print (mapData.walkable [0, 0]);

		/*
		 * // Prints input
		for (int i = 0; i < gridSizeX; i++) {
			string line = "";
			for(int j = 0; j < gridSizeY; j++) {
				if(grid[i, j].walkable)
					line += "1" + " ";
				else
					line += "0" + " ";
			}
			print (line);
		}
		*/
	}

	public Node getNode(int gridPosX, int gridPosY) {
		return grid [gridPosX, gridPosY];
	}

	private bool validIndex(int x, int y) {
		if (0 <= x && x <= gridSizeX) {
			if(0 <= y && y <= gridSizeY) {
				return true;
			}
		}
		return false;
	}

	void CreateGrid() {
		grid = new Node[gridSizeX, gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2
						- Vector3.forward * gridWorldSize.y / 2;

		// Loop through all nodes to do collision check, see if walkable or not
		for (int x = 0; x < gridSizeX; x++) {
			for(int y = 0; y < gridSizeY; y++) {
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius)
					+ Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool walkable = mapData.walkable[x ,y];
				grid[x, y] = new Node(walkable, worldPoint, x, y);
			}

			/*
			for(int y = 0; y < gridSizeY; y++) {
				// want to get worldPosition for the collision checks
				// each point that a node is going to occupy worldPoint
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius)
					+ Vector3.forward * (y * nodeDiameter + nodeRadius);
				// Collision check, true if don't collide with anything in unwalkableMask
				bool walkable = !(Physics.CheckSphere (worldPoint, nodeRadius, unwalkableMask));
				// Populate grid with nodes
				grid[x,y] = new Node(walkable, worldPoint, x, y);
			}
			*/
		}
	}
	
	// Find node that player is currently standing on 
	// I.e convert a world position into a grid coordinate
	public Node NodeFromWorldPoint(Vector3 worldPosition) {
		// convert world position into a percentage for the x and why coordinate
		//  how far along the grid it is
		// far left 0, middle .5, right 1
	
		// add half grid world size then divide with whole grid world size
		float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
		float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;

		// clamp so that if player (worldPos) is outside grid for some reason we don't get a invalid 
		// index to the grid array
		percentX = Mathf.Clamp01 (percentX);
		percentY = Mathf.Clamp01 (percentY);

		// get x, y indexes in the grid array
		int x = Mathf.RoundToInt ((gridSizeX - 1) * percentX);
		int y = Mathf.RoundToInt ((gridSizeY - 1) * percentY);

		return grid[x, y];
	}

	public List<Node> path;
	void OnDrawGizmos() {
		// Draw gridWorldSize
		Gizmos.DrawWireCube (transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

		// Checks if createGrid workes

		if (grid != null) {
			Node playerNode = NodeFromWorldPoint(player.position);
			foreach (Node n in grid) {
				Gizmos.color = (n.walkable) ? Color.white : Color.red;
				if (playerNode == n)
					Gizmos.color = Color.blue;
				if(path != null) {
					if(path.Contains (n))
						Gizmos.color = Color.cyan;
				}
				Gizmos.DrawCube (n.worldPosition, Vector3.one * (nodeDiameter - .1f));
			}
		}
	}
	
	public float GetCost(Node from, Node to) {
		// TODO: Perhaps check if neighbours else return infinity

		float straightCost = 10f;
		float diagonalCost = 14f;

		if (from.gridPosX == to.gridPosX && from.gridPosY + 1 == to.gridPosY) // up
			return straightCost;
		if (from.gridPosX + 1 == to.gridPosX && from.gridPosY == to.gridPosY) // right
			return straightCost;
		if (from.gridPosX == to.gridPosX && from.gridPosY - 1 == to.gridPosY) // down
			return straightCost;
		if (from.gridPosX - 1 == to.gridPosX && from.gridPosY == to.gridPosY)
			return straightCost;

		return diagonalCost;
	}

	// Or perhaps Node should have a neighbour list
	public List<Node> GetNeighbours(Node currentNode) {
		List<Node> neighbours = new List<Node>();

		int neighX = currentNode.gridPosX - 1;
		int neighY = currentNode.gridPosY - 1;
		if(validIndex (neighX, neighY))
			neighbours.Add (NodeFromWorldPoint(currentNode.worldPosition + 
			                                   Vector3.left + Vector3.back));

		neighX = currentNode.gridPosX;
		neighY = currentNode.gridPosY + 1;
		if(validIndex (neighX, neighY))
			neighbours.Add (NodeFromWorldPoint(currentNode.worldPosition + 
			                                   Vector3.forward));

		neighX = currentNode.gridPosX + 1;
		neighY = currentNode.gridPosY + 1;
		if(validIndex (neighX, neighY))
			neighbours.Add (NodeFromWorldPoint(currentNode.worldPosition + 
			                                   Vector3.right + Vector3.forward));

		neighX = currentNode.gridPosX - 1;
		neighY = currentNode.gridPosY;
		if(validIndex (neighX, neighY))
			neighbours.Add (NodeFromWorldPoint(currentNode.worldPosition + 
			                                   Vector3.left));

		neighX = currentNode.gridPosX + 1;
		neighY = currentNode.gridPosY;
		if(validIndex (neighX, neighY))
			neighbours.Add (NodeFromWorldPoint(currentNode.worldPosition + 
			                                   Vector3.right));

		neighX = currentNode.gridPosX - 1;
		neighY = currentNode.gridPosY - 1;
		if(validIndex (neighX, neighY))
			neighbours.Add (NodeFromWorldPoint(currentNode.worldPosition + 
			                                   Vector3.left + Vector3.forward));

		neighX = currentNode.gridPosX;
		neighY = currentNode.gridPosY - 1;
		if(validIndex (neighX, neighY))
			neighbours.Add (NodeFromWorldPoint(currentNode.worldPosition + 
			                                   Vector3.back));

		neighX = currentNode.gridPosX + 1;
		neighY = currentNode.gridPosY - 1;
		if(validIndex (neighX, neighY))
			neighbours.Add (NodeFromWorldPoint(currentNode.worldPosition + 
			                                   Vector3.right + Vector3.back));

		return neighbours;
	}

}
