using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void WeaponAction (Transform t);

public abstract class _WeaponUseBehaviour : _VigilantBehaviour {

	[Space(12)]
	[Range(0.01f, 10f)] public float pauseTime = 1f;
	[Range(0.01f, 10f)] public float reloadTime = 0.5f;

	private PatrolBehaviour pb = null;
	private ChasingBehaviour cb = null;
	private float nextShot = 0f;
	private float moveAgain = 0f;

	protected WeaponAction Fire;

	new protected void Start () {
		base.Start ();	
		pb = GetComponent<PatrolBehaviour> ();
		cb = GetComponent<ChasingBehaviour> ();
		SomeoneInside = Attack;
		NooneInside = NoAttack;
		EveryUpdate = AllUpdates;
	}

	void Attack(Transform t) {
		transform.LookAt (t);
		if (Time.time > nextShot) {
			if (Fire != null) Fire (t);
			nextShot = Time.time + pauseTime;
			if (reloadTime > 0f) {
				moveAgain = Time.time + reloadTime;
				if (pb != null)
					pb.paused = true;
				if (cb != null)
					cb.paused = true;
			}
		}
	}

	void NoAttack(Transform t) {
		if (pb != null)
			pb.paused = false;
		if (cb != null)
			cb.paused = false;
	}

	void AllUpdates(Transform t) {
		if (reloadTime > 0f && Time.time > moveAgain) {
			if (pb != null) pb.paused = false;
			if (cb != null) cb.paused = false;
			reloadTime = 0f;
		}
	}
}