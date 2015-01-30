using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Waypoints : MonoBehaviour {

	List<Vector3> path;
	public float kinematic_vel = 20f;
	public float dynFx = 100f;
	public float dynFy = 100f;
	public float dynM = 5f;
	public float vel = 5f;

	// models from 1 - 3
	public int model = 1;

	// Use this for initialization
	void Start () {
		Vector3 pos1 = new Vector3 (1, 1, 1);
		Vector3 pos2 = new Vector3 (1, 1, 19*3);
		Vector3 pos3 = new Vector3 (19*3, 1, 19*3);
		Vector3 pos4 = new Vector3 (1, 1, 19*3);
		Vector3 pos5 = new Vector3 (19*3, 1, 1);
		path = new List<Vector3> ();
		path.Add (pos1);
		path.Add (pos2);
		path.Add (pos3);
		path.Add (pos4);
		path.Add (pos5);

		StartCoroutine ("Move", model);
	}
	
	IEnumerator Move(int model) {
		int index = 1;
		Vector3 current = path[index];
		while (true) {
			if(transform.position == current) {
				index++;
				if(index >= path.Count) {
					yield break;
				}
				current = path[index];
			}

			// Discrete point
			if(model == 0) { 
				transform.position = current;
				yield return new WaitForSeconds(0.5f);
			}
			// Kinematic point
			else if(model == 1) {
				transform.position = Vector3.MoveTowards (transform.position, current, kinematic_vel * Time.deltaTime);
				yield return null;
			}
			// Dynamic point
			else if(model == 2) {
				Vector3 dir;
				if(Vector3.Distance (current, transform.position) < 1) {
					dir = current - transform.position;
					transform.position = current;
				}
				else {
					dir = Vector3.Normalize (current - transform.position);
					dir.x = (dir.x * dynFx) / dynM * Time.deltaTime;
					dir.z = (dir.z * dynFy) / dynM * Time.deltaTime;

					transform.position = (transform.position + dir);
				}
				yield return null;
			}
			// Differential drive
			else if(model == 3) {
				Vector3 dir;
				if(Vector3.Distance (current, transform.position) < 1) {
					dir = current - transform.position;
					transform.position = current;
				}
				else {
					dir = Vector3.Normalize (current - transform.position);
					Quaternion theta = Quaternion.LookRotation (current - transform.position);

					dir.x = dir.x + Mathf.Cos (theta.y) * vel * Time.deltaTime;
					dir.z = dir.z + Mathf.Sin (theta.y) * vel * Time.deltaTime;

					if(theta != transform.rotation) {
						transform.rotation = Quaternion.RotateTowards (transform.rotation, theta, 1000 * Time.deltaTime);
					}
					else {
						transform.position = (transform.position + dir);
					}
				}
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
				Gizmos.color = Color.black;
				Gizmos.DrawCube (path[i], Vector3.one);

				if(i == 0) {
				//	Gizmos.DrawLine (transform.position, path[i]);
				}
				else {
					Gizmos.DrawLine (path[i-1], path[i]);
				}
			}
		}
	}
}
