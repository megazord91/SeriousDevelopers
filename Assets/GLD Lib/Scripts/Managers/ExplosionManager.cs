using UnityEngine;
using System.Collections;

public class ExplosionManager : MonoBehaviour {

	[HideInInspector] public float blastRadius = 1;
	[HideInInspector] public float flareRadius = 10;
	[HideInInspector] public float flareTime = .5f;
	[HideInInspector] public float pushForce = 0f;
	[HideInInspector] public Transform pushTarget = null;
	[HideInInspector] public Transform spareTarget = null;
	[HideInInspector] public Color centerColor = Color.red;
	[HideInInspector] public Color borderColor = Color.yellow;

	void Start () {
		ParticleSystem ps = GetComponent<ParticleSystem> ();
		ps.startLifetime = flareTime;
		ps.startSpeed = flareRadius / flareTime;
		ps.startSize = Mathf.Min (ps.startSize, flareRadius + blastRadius);

		var shape = ps.shape;
		shape.radius = blastRadius;

		var col = ps.colorOverLifetime;
		Gradient grad = new Gradient();
		grad.SetKeys( new GradientColorKey[] { new GradientColorKey(centerColor, 0.0f), new GradientColorKey(borderColor, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) } );
		col.color = grad;

		if (pushForce > 0f) {
			if (pushTarget != null) {
				DoPush (pushTarget.transform);
			} else {
				foreach (GameObject go in GameObject.FindObjectsOfType<GameObject>()) {
					if (go != transform.gameObject && go.transform != spareTarget) {
						if ((go.transform.position - transform.position).magnitude < (blastRadius + flareRadius)) {
							DoPush (go.transform);
							if (go.GetComponent<ExplosionGenerator> () != null)
								DoPropagate (go.transform);
						}
					}
				}
			}
		}

		Destroy (gameObject, (flareRadius / ps.startSpeed) + 0.1f);
	}

	private void DoPush(Transform t) {
		Rigidbody rb = t.GetComponent<Rigidbody> ();
		if (rb != null) {
			Vector3 pushDir = (t.position - transform.position);
			float d = pushDir.magnitude;
			pushDir = pushDir.normalized;
			pushDir.y = Mathf.Abs (pushDir.y);
			if (d <= blastRadius)
				pushDir *= pushForce;
			else if (d <= blastRadius + flareRadius)
				pushDir *= ((1f - ((d - blastRadius) / flareRadius)) * pushForce);
			else
				return;
			rb.velocity = pushDir;
		}
	}

	private void DoPropagate(Transform t) {
		Debug.Log (t);
		ExplosionGenerator eg = t.GetComponent<ExplosionGenerator> ();
		eg.Detonate (null);
		Destroy (t.gameObject, eg.flareTime);
	}
}
