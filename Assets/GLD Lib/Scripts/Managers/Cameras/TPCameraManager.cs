
// original script at http://wiki.unity3d.com/index.php?title=SmoothFollowWithCameraBumper

using UnityEngine;
using System.Collections;

public class TPCameraManager : MonoBehaviour {

	public float damping = 5.0f;
	public bool smoothRotation = true;
	public float rotationDamping = 10.0f;

	public Vector3 bumperRayOffset; // allows offset of the bumper ray from target origin
	public Vector3 targetLookAtOffset; // allows offsetting of camera lookAt, very useful for low bumper heights

	public float bumperCameraHeight = 1.0f; // adjust camera height while bumping


	private Transform target;
	private Vector3 basePosition;
	private float bumperDistanceCheck;

	private void Start() {
		target = transform.parent;
		PerspectiveManager pm = GetComponentInParent<PerspectiveManager> ();
		if (pm) pm.TP = GetComponent<Camera> ();
		basePosition = transform.localPosition;
		bumperDistanceCheck = (transform.localPosition - bumperRayOffset).magnitude;
	}

	private void LateUpdate() {
		Vector3 wantedPosition = target.TransformPoint(basePosition);

		// check to see if there is anything behind the target
		RaycastHit hit;
		Vector3 back = target.transform.TransformDirection(Vector3.back); 

		// cast the bumper ray out from rear and check to see if there is anything behind
		if (Physics.Raycast(target.TransformPoint(bumperRayOffset), back, out hit, bumperDistanceCheck) && hit.transform != target) {
			// clamp wanted position to hit position
			wantedPosition.x = hit.point.x;
			wantedPosition.z = hit.point.z;
			wantedPosition.y = Mathf.Lerp(hit.point.y + bumperCameraHeight, wantedPosition.y, Time.deltaTime * damping);
		} 

		transform.position = Vector3.Lerp(transform.position, wantedPosition, Time.deltaTime * damping);

		Vector3 lookPosition = target.TransformPoint(targetLookAtOffset);

		if (smoothRotation) {
			Quaternion wantedRotation = Quaternion.LookRotation(lookPosition - transform.position, target.up);
			transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);
		} 
		else 
			transform.rotation = Quaternion.LookRotation(lookPosition - transform.position, target.up);
	}
	
}
