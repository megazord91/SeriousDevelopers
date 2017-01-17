using UnityEngine;
using System.Collections;

public class NPCBlaster : _WeaponUseBehaviour {

	new void Start () {
		base.Start ();
		LinearFireGenerator lfg = GetComponentInChildren<LinearFireGenerator> ();
		if (lfg == null)
			enabled = false;
		else
			Fire = lfg.Fire;
	}
}
