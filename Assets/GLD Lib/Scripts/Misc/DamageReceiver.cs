using UnityEngine;
using System.Collections;

public class DamageReceiver : MonoBehaviour {

	public bool active = true;

	private StatsInfo si;

	void Start() {
		si = GetComponentInChildren<StatsInfo> ();
	}

	public void ReceiveDamage(int i) {
		if (active && si != null) {
			si.HP -= i;
			if (si.HP <= 0)
				Destroy (si.gameObject);
		}
	}
}
