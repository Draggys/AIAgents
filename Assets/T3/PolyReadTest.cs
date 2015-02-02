using UnityEngine;
using System.Collections;

public class PolyReadTest : MonoBehaviour {

	PolyMapLoader map;
	// Use this for initialization
	void Start () {
		map = new PolyMapLoader ("x", "y", "goalPos", "startPos", "button");	

		/*
		 * Debugging
		map.polyData.printNodes ();
		map.polyData.printStart ();
		map.polyData.printEnd ();
		map.polyData.printButtons ();
		*/
	}

	void OnDrawGizmos() {
		if (map.polyData.nodes != null) {
			foreach(Vector3 node in map.polyData.nodes) {
				Gizmos.color = Color.white;
				Gizmos.DrawCube (node, Vector3.one);
			}
		}
		Gizmos.color = Color.green;
		Gizmos.DrawCube (map.polyData.start, Vector3.one);
		
		Gizmos.color = Color.red;
		Gizmos.DrawCube (map.polyData.end, Vector3.one);

	}	
}
	