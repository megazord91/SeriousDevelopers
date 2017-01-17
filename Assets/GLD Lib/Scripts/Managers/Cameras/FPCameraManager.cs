using UnityEngine;
using System.Collections;

public class FPCameraManager : MonoBehaviour {

	void Start () {
		PerspectiveManager pm = GetComponentInParent<PerspectiveManager> ();
		if (pm) pm.FP = GetComponent<Camera> ();
	}

	/*
	void Update () {
		;
	}
	*/
}
