using UnityEngine;
using System.Collections;

public class FIXCameraManager : MonoBehaviour {

	private Transform dad;

	void Start () {
		PerspectiveManager pm = GetComponentInParent<PerspectiveManager> ();
		if (pm) pm.FIX = GetComponent<Camera> ();
		dad = transform.parent;
		transform.parent = null;
		transform.rotation = Quaternion.LookRotation (Vector3.down);
	}
	
	void Update () {
		if (dad) {
			transform.position = new Vector3 (dad.position.x, transform.position.y, dad.position.z);
		}
	}
}
