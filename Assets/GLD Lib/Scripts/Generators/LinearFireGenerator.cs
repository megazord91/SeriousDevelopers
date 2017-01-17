using UnityEngine;
using System.Collections;

public class LinearFireGenerator : MonoBehaviour {

	public Transform ammo = null;
	[Range(0.01f, 100f)] public float speed = 75;

	public void Fire(Transform t) {
		if (ammo != null) {
			Transform bullet = (Transform) Instantiate (ammo, transform.position, transform.parent.rotation);
			Vector3 aim;
			if (t == null)
				aim = transform.forward;
			else {
				Collider c = t.GetComponentInChildren<Collider> ();
				Vector3 dir = c != null ? c.transform.position : transform.position;
				aim = (dir - transform.position).normalized;
			}
			bullet.GetComponent<Rigidbody> ().velocity = aim * speed;
		}
	}

}
