using UnityEngine;
using System.Collections;

public enum DoorMode { SLIDE_UP, SLIDE_DOWN, SLIDE_LEFT, SLIDE_RIGHT, SWING_LEFT, SWING_RIGHT }; //

public class DoorManager : _VigilantBehaviour {

	[Space(12)]
	public DoorMode openingMode = DoorMode.SWING_RIGHT;
	[Range(0.01f, 10f)] public float openingTime = 1.5f;
	[Range(0.01f, 10f)] public float closingTime = .5f;
	public bool openOnClick = false;

	[Space(12)]
	public bool keepOpen = false;
	[Range(0.01f, 10f)] public float stayOpenFor = 0f;
	public bool keepShutAfter = false;

	private DoorPanelManager pm;
	private float timeout = 0;
	private bool ignorePresence = false;

	void Reset() {
		sensingRange = 3f;
		needLineOfSight = false;
		active = true;
	}

	new void Start () {
		base.Start ();
		pm = GetComponentInChildren<DoorPanelManager> ();
		pm.DoneCall = Done;
		pm.NotifyClick = Click;
		SomeoneInside = Someone;
		NooneInside = Noone;
		EveryUpdate = AllUpdates;
	}

	private void Someone(Transform t) {
		if (!openOnClick && !ignorePresence) pm.Open (openingMode, openingTime);
	}

	private void Noone(Transform t) {
		if (!openOnClick) {
			pm.Close (closingTime);
			ignorePresence = false;
		}
	}

	private void AllUpdates(Transform t) {
		if (timeout != 0 && Time.time > timeout) {
			pm.Close (closingTime);
			timeout = 0f;
		}
	}
		
	private void Done() {
		if (pm.status == DoorPanelStatus.OPEN) {
			if (keepOpen)
				active = false;
			else if (stayOpenFor > 0f) {
				timeout = Time.time + stayOpenFor;
				ignorePresence = true;
			}
		} else if (pm.status == DoorPanelStatus.CLOSE) {
			if (keepShutAfter)
				active = false;
		}
	}
		
	private void Click() {
		if (active && openOnClick) {
			if (pm.status == DoorPanelStatus.CLOSE) pm.Open (openingMode, openingTime);
			else if (pm.status == DoorPanelStatus.OPEN) pm.Close (closingTime);
		}
	}
}
