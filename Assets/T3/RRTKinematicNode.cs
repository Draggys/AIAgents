using UnityEngine;
using System.Collections;

public class RRTKinematicNode {


	public Vector3 position;
	public RRTKinematicNode parent;

	public RRTKinematicNode(Vector3 pos){
		position = pos;
		parent = null;
		}

	public void setParent(RRTKinematicNode par){
		parent = par;
		}



}
