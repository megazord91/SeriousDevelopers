using UnityEngine;
using System.Collections;

public class HandTorchManager : MonoBehaviour {

	public bool active = false;
	private bool previous = false;
	private bool firstUpdate = true;

	private BoxCollider myCollider;
	private Light myLight;
	private ParticleSystem torchEffect;

	void Start () {
		myCollider = GetComponent<BoxCollider> ();
		myLight = GetComponent<Light> ();
		torchEffect = GetComponent<ParticleSystem> ();
		previous = !active;
		firstUpdate = true;
	}
	
	void Update () {
		if (firstUpdate || previous != active) {
			if (myCollider != null) myCollider.enabled = active;
			if (myLight != null) myLight.enabled = active;
			if (active && torchEffect != null)
				torchEffect.Play ();
			else
				torchEffect.Stop ();
			previous = active;
			firstUpdate = false;
		}
	}
}
