using UnityEngine;
using System.Collections;

public class TeleportManager : MonoBehaviour {

	public bool active = true;

	public Transform destination = null;

	[Header("Triggering for")]
	public GameObject target = null;
	public string targetTag = "Player";

	[HideInInspector] public bool skipOne = false;

	void Start () {
		;
	}

	void OnTriggerEnter(Collider c) {
		if (active && destination != null && !skipOne) {
			if ((targetTag != "" && c.transform.root.tag == targetTag) || (target != null && c.transform.root == target)) {
				c.transform.root.position = destination.position + (c.transform.root.position - transform.position);
				TeleportManager tm = destination.transform.GetComponent<TeleportManager> ();
				if (tm) tm.skipOne = true;
			}
		}
	}

	void OnTriggerExit(Collider c) {
		skipOne = false;
	}
}
