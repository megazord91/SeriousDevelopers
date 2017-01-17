using UnityEngine;
using System.Collections;

public delegate void DoorPanelCallback ();

public enum DoorPanelStatus { OPEN, CLOSE, MOVING };

public class DoorPanelManager : MonoBehaviour {

	private Vector3 homePos;
	private Vector3 startPos;
	private Vector3 targetPos;

	private Quaternion homeRot;
	private Quaternion startRot;
	private Quaternion targetRot;

	private float movementTime;
	private float baseTime;
	[HideInInspector] public DoorPanelCallback DoneCall;
	[HideInInspector] public DoorPanelCallback NotifyClick;
	[HideInInspector] public DoorPanelStatus status;
	private DoorPanelStatus pending;

	void Start() {
		homePos = transform.localPosition;
		homeRot = transform.root.localRotation;
		status = DoorPanelStatus.CLOSE;
		pending = DoorPanelStatus.CLOSE;
	}

	void Update() {
		if (status == DoorPanelStatus.MOVING) {
			if (transform.localPosition != targetPos || transform.root.localRotation != targetRot) {
				float f = (Time.time - baseTime) / movementTime;
				if (f > 1f)
					f = 1f;
				if (transform.localPosition != targetPos) {
					transform.localPosition = Vector3.Lerp (startPos, targetPos, f);
				}
				if (transform.root.localRotation != targetRot) {
					transform.root.localRotation = Quaternion.Lerp (startRot, targetRot, f);
				}
				if (f >= 1f) {
					transform.localPosition = targetPos;
					transform.root.localRotation = targetRot;
					status = pending;
					if (DoneCall != null) DoneCall ();
				}
			} 
		}
	}

	void OnMouseDown() {
		if (NotifyClick != null) NotifyClick ();
	}

	public void Open(DoorMode dm, float t) {
		if (status != DoorPanelStatus.OPEN && pending != DoorPanelStatus.OPEN) {
			startPos = transform.localPosition;
			startRot = transform.root.localRotation;
			switch (dm) {
			case DoorMode.SLIDE_UP:
				targetPos = homePos + Vector3.up;
				targetRot = homeRot;
				break;
			case DoorMode.SLIDE_DOWN:
				targetPos = homePos + Vector3.down;
				targetRot = homeRot;
				break;
			case DoorMode.SLIDE_RIGHT:
				targetPos = homePos + Vector3.right;
				targetRot = homeRot;
				break;
			case DoorMode.SLIDE_LEFT:
				targetPos = homePos + Vector3.left;
				targetRot = homeRot;
				break;
			case DoorMode.SWING_RIGHT:
				targetPos = homePos + transform.InverseTransformPoint (transform.position + (Vector3.forward + Vector3.left) * transform.root.localScale.x / 2f);
				targetRot = Quaternion.Euler(new Vector3(0f, 90f, 0f)) * homeRot;
				break;
			case DoorMode.SWING_LEFT:
				targetPos = homePos + transform.InverseTransformPoint (transform.position + (Vector3.forward + Vector3.right) * transform.root.localScale.x / 2f);
				targetRot = Quaternion.Euler (new Vector3 (0f, -90f, 0f)) * homeRot;
				break;
			default:
				return;
			}
			baseTime = Time.time;
			movementTime = t;
			status = DoorPanelStatus.MOVING;
			pending = DoorPanelStatus.OPEN;
		}
	}

	public void Close(float t) {
		if (status != DoorPanelStatus.CLOSE && pending != DoorPanelStatus.CLOSE) {
			startPos = transform.localPosition;
			startRot = transform.root.localRotation;
			targetPos = homePos;
			targetRot = homeRot;
			baseTime = Time.time;
			movementTime = t;
			status = DoorPanelStatus.MOVING;
			pending = DoorPanelStatus.CLOSE;
		}
	}

}
