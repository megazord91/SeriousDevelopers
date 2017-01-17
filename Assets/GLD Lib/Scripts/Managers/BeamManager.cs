using UnityEngine;
using System.Collections;

public class BeamManager : MonoBehaviour {

	public bool active = false;

	private bool previous = false;
	private Light myLight;
	private bool firstUpdate = true;

	void Start () {
		myLight = GetComponent<Light> ();
		previous = !active;
		firstUpdate = true;
	}

	void Update () {
		if (firstUpdate || previous != active) {
			if (myLight != null) myLight.enabled = active;
			previous = active;
			firstUpdate = false;
		}
	}
}
