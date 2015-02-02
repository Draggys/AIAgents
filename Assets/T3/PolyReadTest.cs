using UnityEngine;
using System.Collections;

public class PolyReadTest : MonoBehaviour {

	PolyMapLoader map;
	PolyGraphCreator createdGraph;

	// Use this for initialization
	void Start () {
		map = new PolyMapLoader ("x", "y", "goalPos", "startPos", "button");	

		Debug.Log ("Lines size:" + map.polyData.lines.Count);
		Debug.Log ("Vertices size:" + map.polyData.nodes.Count);

		//THE CREATE GRAPH DOES NOT WORK 
		//createdGraph = new PolyGraphCreator (map.polyData);

		//Debug.Log ("Created Graph size:" + createdGraph.possibleLines.Count);

		/*
		 * Debugging
		map.polyData.printNodes ();
		map.polyData.printStart ();
		map.polyData.printEnd ();
		map.polyData.printButtons ();
		*/
	}

	void OnDrawGizmos() {

		map = new PolyMapLoader ("x", "y", "goalPos", "startPos", "button");

		//createdGraph = new PolyGraphCreator (map.polyData);

		if (map.polyData.nodes != null) {
			foreach(Vector3 node in map.polyData.nodes) {
				Gizmos.color = Color.blue;
				Gizmos.DrawCube (node, Vector3.one);
			}
			/*
			foreach(PolyNode fig in map.polyData.figures){
				Gizmos.color=Color.black;

				for(int i=0;i<fig.vertices.Count;i++){
					if(i<fig.vertices.Count-1){
						Gizmos.DrawLine(fig.vertices[i],fig.vertices[i+1]);
					}
					else{
						Gizmos.DrawLine(fig.vertices[i],fig.vertices[0]);
					}

				}
			}*/

			foreach(Line line in map.polyData.lines){
				Gizmos.color=Color.black;

				Gizmos.DrawLine(line.point1,line.point2);
			}

			/*
			foreach(Line line in createdGraph.possibleLines){

				Gizmos.color=Color.gray;

				Gizmos.DrawLine(line.point1,line.point2);

			}*/

		}
		Gizmos.color = Color.green;
		Gizmos.DrawCube (map.polyData.start, Vector3.one);
		
		Gizmos.color = Color.red;
		Gizmos.DrawCube (map.polyData.end, Vector3.one);




	}	
}
	