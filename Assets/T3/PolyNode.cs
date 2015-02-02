using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PolyNode {
	public Vector3 pos;
	public List<Vector3> neighbours;

	public PolyNode() {
		neighbours = new List<Vector3> ();
	}
}
