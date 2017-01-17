using UnityEngine;
using System.Collections;

public class MineBehaviour : _VigilantBehaviour {

	void Reset() {
		sensingRange = 1f;
		heightOfSight = 0f;
		active = true;
	}

	new void Start () {
		base.Start ();
		SomeoneInside = TriggerTrap;
	}

	public void TriggerTrap(Transform t) {
		ExplosionGenerator eg = GetComponent<ExplosionGenerator> ();
		if (eg != null) eg.Detonate (t);
		active = false;
	}

	public void Reload() {
		if (!active) {
			active = true;
			ExplosionGenerator eg = GetComponent<ExplosionGenerator> ();
			if (eg != null) eg.active = true;
		}
	}

}
