using UnityEngine;
using System.Collections;

public enum Perspective {
	firstPerson, thirdPerson, topView, topViewFixed
}

public class PerspectiveManager : MonoBehaviour {

	public Perspective view = Perspective.thirdPerson;
	private Perspective previous;
	private bool firstUpdate;

	[HideInInspector] public Camera FP;
	[HideInInspector] public Camera TP;
	[HideInInspector] public Camera TOP;
	[HideInInspector] public Camera FIX;

	void Start () {
		previous = view;
		firstUpdate = true;
	}
	
	void FixedUpdate () {
		if (firstUpdate || view != previous) {
			if (FP) FP.enabled = false; 
			if (TP) TP.enabled = false;
			if (TOP) TOP.enabled = false;
			if (FIX) FIX.enabled = false;
			if (FP && view == Perspective.firstPerson) FP.enabled = true;
			else if (TP && view == Perspective.thirdPerson) TP.enabled = true;
			else if (TOP && view == Perspective.topView) TOP.enabled = true;
			else if (FIX && view == Perspective.topViewFixed) FIX.enabled = true; 
			previous = view;
			firstUpdate = false;
		}
		if (view == Perspective.topViewFixed) {
			FIX.transform.rotation = Quaternion.LookRotation (Vector3.down);
		}
	}
}
