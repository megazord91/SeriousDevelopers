using UnityEngine;
using System.Collections;

public class LiftManager : MonoBehaviour {

	public bool active = true;

	[Header("Where we go (precedence on Transform)")]
	[Range(-100f, 100f)] public float height = 10f;
	public Transform destination = null;

	[Space(12)]
	[Range(0f, 10f)] public float speed = 3f;
	[Range(0f, 120f)] public float pauseAtStart = 0f;
	[Range(0f, 120f)] public float pauseAtEnd = 0f;
	public bool oneWay = false;
	public bool waitForExitAtEnd = false;

	[Header("Triggering for")]
	public bool automatic = false;
	public GameObject target = null;
	public string targetTag = "Player";

	private bool onDuty = false;
	private bool going = false;
	private bool waitToGoBack = false;
	private bool comingBack = false;

	private Vector3 basePosition;
	private Vector3 targetPosition;
	private float startTime;
	private Rigidbody myBody;

	void Start () {
		basePosition = transform.position;
		myBody = transform.GetComponent<Rigidbody> ();
	}

	void FixedUpdate() {
		if (automatic && !onDuty) StartLift ();
		if (onDuty && active) {
			float fixedStep = speed * Time.fixedDeltaTime;
			if (!going && !comingBack && !waitToGoBack) {
				if (Time.time > startTime) {
					going = true;
				}
			} else if (waitToGoBack) {
				if (Time.time > startTime) {
					waitToGoBack = false;
					comingBack = true;
				}
			} else if (going || comingBack) {
				// smooth up first and last meters when traveling
				float dt = (targetPosition - transform.position).magnitude;
				float ds = (basePosition - transform.position).magnitude;
				if (dt < speed) fixedStep = (fixedStep * (dt / speed)) + 0.001f;
				else if (ds < speed) fixedStep = (fixedStep * (ds / speed)) + 0.001f;
				if (going) {
					myBody.MovePosition(transform.position + (targetPosition - transform.position).normalized * fixedStep);
					if (dt < fixedStep) {
						startTime = Time.time + pauseAtEnd;
						going = false;
						waitToGoBack = true;
						if (oneWay || waitForExitAtEnd) active = false;
					}
				}  else {
					myBody.MovePosition(transform.position + (basePosition - transform.position).normalized * fixedStep);
					if (ds < fixedStep) {
						comingBack = false;
						if (!automatic) onDuty = false;
					}
				}
			}
		}
	}

	private void StartLift() {
		targetPosition = destination != null ? destination.position : transform.position + (Vector3.up * height);
		startTime = Time.time + pauseAtStart;
		onDuty = true;
		going = false;
		waitToGoBack = false;
		comingBack = false;
	}

	void OnTriggerEnter(Collider c) {
		if (!automatic && active && !onDuty && (c.transform.root.tag == targetTag || c.transform.root == target)) {
			StartLift ();
		}
	}

	void OnTriggerExit(Collider c) {
		if (!automatic && waitForExitAtEnd && waitToGoBack && !active) active = true;
	}

}
