using UnityEngine;
using System.Collections;

public class NPCGrenadeLauncer : _WeaponUseBehaviour {

	new void Start () {
		base.Start ();
		ParabolicFireGenerator pfg = GetComponentInChildren<ParabolicFireGenerator> ();
		if (pfg == null)
			enabled = false;
		else {
			Fire = pfg.Fire;
		}
	}
}