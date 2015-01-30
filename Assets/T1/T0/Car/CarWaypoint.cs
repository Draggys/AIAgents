using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarWaypoint : MonoBehaviour {

	public List<Vector3> path;
	public int model;
	public float vel;
	GameObject rightFrontWheel;

	void Start () {
		model = 0;
		vel = 0.01f;
		Vector3 pos1 = new Vector3 (0, 0, 30);
		Vector3 pos2 = new Vector3 (30, 0, 30);
		Vector3 pos3 = new Vector3 (30, 0, 0);
		path = new List<Vector3> ();
		path.Add (pos1);
		path.Add (pos3);
		path.Add (pos2);

		rightFrontWheel = GameObject.Find ("RightTopWheel");
		
		StartCoroutine ("Move", model);
	}
	public int index = 1;
	IEnumerator Move(int model) {
		Vector3 	current = path[index];
		while (true) {
			if(transform.position == current) {
				index++;
				if(index >= path.Count) {
					yield break;
				}
				current = path[index];
			}
			// Kinematic car model
			else if(model == 0) {
				Quaternion phi = Quaternion.LookRotation (rightFrontWheel.transform.position - transform.position);
				rightFrontWheel.transform.rotation = Quaternion.RotateTowards(rightFrontWheel.transform.rotation,
				                                                              phi, 100 * Time.deltaTime);

				float theta =  Mathf.Tan (phi.y);
				print ("theta = " + theta);
				transform.rotation = Quaternion.AngleAxis (theta, Vector3.up);

				//float theta = (vel / 2) * Mathf.Tan (phi.y);
				//transform.eulerAngles = new Vector3(0, theta, 0);

				yield return null;
			}
			else {
				yield break;
			}
		}
	}

	public void OnDrawGizmos() {
		if (path != null) {
			for(int i = 0; i < path.Count; i++) {
				Gizmos.color = Color.white;
				Gizmos.DrawCube (path[i], Vector3.one);
			}

			Gizmos.color = Color.red;
			Gizmos.DrawLine (rightFrontWheel.transform.position, path[1]);

			Gizmos.color = Color.blue;
			Gizmos.DrawLine (transform.position, path[1]);
		}
	}
}
