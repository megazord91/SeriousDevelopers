using UnityEngine;
using System.Collections;

public class PlayerBeam : MonoBehaviour {

	public bool active = true;
	public KeyCode key = KeyCode.B;

	private BeamManager beam = null;

	void Start () {
		beam = GetComponentInChildren<BeamManager> ();
	}
	
	void Update () {
		if (beam && active && Input.GetKeyDown (key)) {
			beam.active = !beam.active;
		}
	}
}
