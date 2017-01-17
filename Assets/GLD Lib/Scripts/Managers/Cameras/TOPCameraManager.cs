using UnityEngine;
using System.Collections;

public class TOPCameraManager : MonoBehaviour {

	void Start () {
		PerspectiveManager pm = GetComponentInParent<PerspectiveManager> ();
		if (pm) pm.TOP = GetComponent<Camera> ();
	}

	/*
	void Update () {
		;
	}
	*/
}
