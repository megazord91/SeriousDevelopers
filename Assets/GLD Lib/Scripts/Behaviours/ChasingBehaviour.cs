using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChasingBehaviour : _VigilantBehaviour {

	[Space(12)]
	public bool goTowardTarget = false;
	[Range(0f, 100f)] public float speed = 4f;
	[Range(0f, 10f)] public float stopAt = 2f; 

	private PatrolBehaviour pb;

	new void Start () {
		base.Start ();
		pb = GetComponent<PatrolBehaviour> ();
		SomeoneInside = Chase;
		NooneInside = NoChase;
	}

	private void Chase(Transform t) {
		if (t == null)
			return;
		if (pb != null) pb.paused = true;
		transform.LookAt (t);
		if (goTowardTarget && (transform.position - t.position).magnitude > stopAt) {
			transform.Translate (Vector3.forward * (speed * Time.deltaTime));
		}
	}

	private void NoChase(Transform t) {
		if (pb != null) pb.paused = false;
	}
}