using UnityEngine;
using System.Collections;

public class LightsaberManager : MonoBehaviour {

	public bool active = false;
	private bool previous = false;

	private LineRenderer line;
	private CapsuleCollider cc;
	private Light l;

	void Start () {
		line = GetComponentInChildren<LineRenderer> ();
		cc = GetComponentInChildren<CapsuleCollider> ();
		l = GetComponentInChildren<Light> ();
	}
	
	void Update () {
		if (previous != active) {
			if (line != null) line.enabled = active;
			if (cc != null) cc.enabled = active;
			if (l != null) l.enabled = active;
			previous = active;
		}
	}
}
