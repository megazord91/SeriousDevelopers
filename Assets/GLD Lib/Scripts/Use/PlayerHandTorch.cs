using UnityEngine;
using System.Collections;

public class PlayerHandTorch : MonoBehaviour {

	public bool active = true;
	public KeyCode key = KeyCode.T;

	private HandTorchManager torch = null;

	void Start () {
		torch = GetComponentInChildren<HandTorchManager> ();
	}
	
	void Update () {
		if (torch && active && Input.GetKeyDown (key)) {
			torch.active = !torch.active;
		}
	}
}
