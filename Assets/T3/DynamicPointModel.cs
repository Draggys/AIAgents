using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DynamicPointModel : MonoBehaviour, Model {
	
	public float dynFx ;
	public float dynFy ;
	public float accX;
	public float accY;
	public float dynM ;

	List<PolyNode> path;

	public DynamicPointModel() {
		accX = 0.1f;
		accY = 0.1f;
	}
	
	public void SetPath(List<PolyNode> path) {
		this.path = path;
	}
	
	public void StartCoroutineMove() {
		StartCoroutine ("Move");
	}
	
	public IEnumerator Move() {
		int index = 0;
		float velX = 0;
		float velY = 0;
		float dynVel = 0;
		Vector3 current = path[index].pos;
		Vector3 dir;
		while (true) {
			if(GameObject.Find ("Player").transform.position == current) {
				index++;
				if(index >= path.Count)
					yield break;
				current = path[index].pos;
				velX=0;
				velY=0;
				dynVel=0;
			}
			
			if (Vector3.Distance (current, GameObject.Find ("Player").transform.position) < velX * Time.deltaTime) {
				dir = current - GameObject.Find ("Player").transform.position;
				GameObject.Find ("Player").transform.position = current;
				yield return new WaitForSeconds (0.5f);
			}
			else {
				velX = velX + accX;
				dir = Vector3.Normalize (current - GameObject.Find ("Player").transform.position);

				//Since the vector is normalized it should be the same velocity for both
				dir.x = (dir.x * velX) * Time.deltaTime;
				dir.z = (dir.z * velX) * Time.deltaTime;
				
				GameObject.Find ("Player").transform.position = (GameObject.Find ("Player").transform.position + dir);
			}
			yield return null;
		}
	}
}
