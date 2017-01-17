using UnityEngine;
using System.Collections;

public class OutlineObject : MonoBehaviour {

	private Renderer myRenderer;
	private Color originalColor;

	public Color outlineColor = Color.yellow;

	void Start() {
		myRenderer = GetComponent<Renderer>();
		originalColor = myRenderer.material.color;
	}

	void OnMouseEnter() {
		myRenderer.material.color = outlineColor;
	}

	void OnMouseExit() {
		myRenderer.material.color = originalColor;
	}
}
