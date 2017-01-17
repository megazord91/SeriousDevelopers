using UnityEngine;
using System.Collections;

public class BulletManager : MonoBehaviour {

	private Vector3 origin;

	[Range(0, 500)] public float maxDistance = 250;
	public bool canBounce = false;

	private DamageProvider dp;

	void Start () {
		origin = transform.position;
		dp = GetComponentInChildren<DamageProvider> ();
	}

	void Update () {
		if (Vector3.Distance(origin, transform.position) >= maxDistance) Destroy(gameObject);
	}
		
	void OnCollisionEnter(Collision col) {
		if (canBounce) {
			BulletBouncer bb = col.transform.GetComponent<BulletBouncer> ();
			if (bb != null && bb.active) {
				if (bb.target != null) {
					Rigidbody br = transform.GetComponent<Rigidbody> ();
					br.velocity = (bb.target.position - transform.position).normalized * br.velocity.magnitude;
				}
				return;
			}
		}
		ExplosionGenerator eg = GetComponent<ExplosionGenerator> ();
		if (eg != null) eg.Detonate (col.transform);
		if (dp != null) dp.ProvideDamage (col.transform);
		Destroy (gameObject);
	}
}