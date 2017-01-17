using UnityEngine;
using System.Collections;

public class ExplosionGenerator : MonoBehaviour {

	public bool active = true;

	[Range(0.01f, 10f)] public float blastRadius = 5f;
	[Range(0.01f, 50f)] public float flareRadius = 5f;
	[Range(0.01f, 5f)] public float flareTime = .5f;
	[Range(0.01f, 20f)] public float pushForce = 0f;
	public bool pushOnlyTarget = false;
	public Color centerColor = Color.red;
	public Color borderColor = Color.yellow;


	public void Detonate(Transform t) {
		if (active) {
			ExplosionManager em = ((GameObject)Instantiate (Resources.Load ("Fire/Explosion"), transform.position, transform.rotation)).transform.GetComponent<ExplosionManager> ();
			em.blastRadius = blastRadius;
			em.flareRadius = flareRadius;
			em.centerColor = centerColor;
			em.borderColor = borderColor;
			em.flareTime = flareTime;
			em.pushForce = (pushOnlyTarget && t == null) ? 0f : pushForce;
			em.pushTarget = (em.pushForce > 0 && pushOnlyTarget) ? t : null;
			em.spareTarget = transform;
			active = false;
		}
	}

}
