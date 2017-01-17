using UnityEngine;
using System.Collections;

public class PlayerLightning : MonoBehaviour {

	public bool active = true;
	public KeyCode key = KeyCode.RightBracket;

	private LightningGenerator lg = null;

	void Start () {
		lg = GetComponentInChildren<LightningGenerator> ();
	}
	
	void Update () {
		if (lg && active && Input.GetKeyDown (key)) {
			RaycastHit h;
			if (Physics.Raycast (lg.transform.position, transform.forward * lg.maxDistance, out h, lg.maxDistance)) {
				lg.Fire (h.collider.transform);
			} else {
				lg.Fire (null);
			}
		}
	}
}
