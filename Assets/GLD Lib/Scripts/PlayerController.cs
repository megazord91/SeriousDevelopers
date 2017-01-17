using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public bool active = true;

	[Range(0.0f, 20.0f)] public float movementSpeed = 4f;
	[Range(0.0f, 180.0f)] public float rotationSensitivity = 90f;

	[Space(12)]
	public bool canJump = true;
	public KeyCode jumpKey = KeyCode.Space;
	[Range(0.0f, 10.0f)] public float jumpHeight = 1.5f;

	private Rigidbody rb;

	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	void Update () {
		if (active) {
			if (rb) {
				rb.MovePosition (transform.position + transform.rotation * (Vector3.forward * movementSpeed * (Input.GetAxis ("Vertical") * Time.deltaTime)));
				rb.MoveRotation(Quaternion.Euler(0.0f, rotationSensitivity * (Input.GetAxis ("Horizontal") * Time.deltaTime), 0.0f) * transform.rotation);
			} else {
				transform.Translate (Vector3.forward * movementSpeed * (Input.GetAxis ("Vertical") * Time.deltaTime));
				transform.Rotate (0.0f, rotationSensitivity * (Input.GetAxis ("Horizontal") * Time.deltaTime), 0.0f, Space.World);
			}

			if (canJump && Input.GetKeyDown (jumpKey)) {
				if (rb) rb.MovePosition (transform.position + Vector3.up * jumpHeight);
				else transform.Translate (jumpHeight * Vector3.up);
			}

		}
	}
}
