using UnityEngine;
using System.Collections;

public class Node {
	public bool walkable;
	public Vector3 worldPosition;
	public int gridPosX;
	public int gridPosY;

	public Node(bool walkable, Vector3 worldPosition, int gridPosX, int gridPosY) {
		this.walkable = walkable;
		this.worldPosition = worldPosition;
		this.gridPosX = gridPosX;
		this.gridPosY = gridPosY;
	}
}
