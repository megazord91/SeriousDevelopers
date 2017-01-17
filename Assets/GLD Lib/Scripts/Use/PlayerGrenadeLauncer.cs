using UnityEngine;
using System.Collections;

public class PlayerGrenadeLauncer : MonoBehaviour {

	public bool active = true;
	public KeyCode key = KeyCode.Backslash;

	private ParabolicFireGenerator pfg = null;

	void Start () {
		pfg = GetComponentInChildren<ParabolicFireGenerator> ();
	}
	
	void Update () {
		if (pfg && active && Input.GetKeyDown (key)) {
			pfg.Fire (null);
		}
	}
}
