using UnityEngine;
using System.Collections;

public class PatrolBehaviour : MonoBehaviour {

	public bool active = false;
	[HideInInspector] public bool paused;

	public bool lookTarget = true;
	public bool loop = true;

	[Range(0f, 100f)] public float speed = 4f;

	public Transform[] rallyPoints;

	private int current;

	void Start () {
		paused = false;
		current = 0;
	}
		
	void Update () {
		if (active && !paused) {
			if (rallyPoints[current] != null && (transform.position - rallyPoints [current].position).magnitude > (speed / 20f)) {
				if (lookTarget) {
					transform.LookAt (rallyPoints [current]);
					transform.Translate (Vector3.forward * (speed * Time.deltaTime));
					transform.LookAt (new Vector3 (rallyPoints [current].position.x, transform.position.y, rallyPoints [current].position.z));
				} else {
					transform.Translate ((rallyPoints [current].position - transform.position).normalized * (speed * Time.deltaTime));
				}
/* // why the @$#% this is not working ?
				if (lookNext) transform.LookAt (references [current]);
				transform.Translate ((references [current].position - transform.position).normalized * (speed * Time.deltaTime));
				if (lookNext) transform.LookAt (new Vector3 (references [current].position.x, transform.position.y, references [current].position.z));
*/

			} else {
				current = ((current + 1) % rallyPoints.Length);
				if (!loop && current == 0)
					active = false;
			}
		}
	}
}
