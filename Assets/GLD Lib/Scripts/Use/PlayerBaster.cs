using UnityEngine;
using System.Collections;

public class PlayerBaster : MonoBehaviour {

	public bool active = true;
	public KeyCode key = KeyCode.Return;

	private LinearFireGenerator lfg = null;

	void Start () {
		lfg = GetComponentInChildren<LinearFireGenerator> ();
	}
	
	void Update () {
		if (lfg && active && Input.GetKeyDown (key)) {
			lfg.Fire (null);
		}
	}
}
