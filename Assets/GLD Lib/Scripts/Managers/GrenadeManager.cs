using UnityEngine;
using System.Collections;

public class GrenadeManager : MonoBehaviour {

	[Range(0, 60)] public float time = 30;
	public bool onContact = false;

	private float deadline;
	private bool active;

	void Start () {
		deadline = Time.time + time;
		active = true;
	}

	void Update() {
		if (active && Time.time >= deadline) {
			ExplosionGenerator eg = GetComponent<ExplosionGenerator> ();
			if (eg != null) eg.Detonate (null);
			Destroy (gameObject, eg.flareTime);
			active = false;
		}
	}

	void OnCollisionEnter(Collision col) {
		if (onContact) {
			ExplosionGenerator eg = GetComponent<ExplosionGenerator> ();
			if (eg != null) eg.Detonate (null);
			Destroy (gameObject);
		}
	}
}