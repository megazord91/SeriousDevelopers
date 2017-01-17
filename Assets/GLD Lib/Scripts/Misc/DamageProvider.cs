using UnityEngine;
using System.Collections;

public class DamageProvider : MonoBehaviour {

	public bool active = true;

	public int multiplicity = 1;
	public int dice = 6;
	public int bonus = 0;

	public void ProvideDamage(Transform t) {
		if (active) {
			DamageReceiver dr = t.root.GetComponentInChildren<DamageReceiver> ();
			if (dr != null) {
				int dmg = 0;
				for (int i = 0; i < multiplicity; i += 1)
					dmg += Random.Range (1, dice);
				dmg += bonus;
				dr.ReceiveDamage (dmg);
			}
		}
	}

}
