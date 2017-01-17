using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CeilingBuilder : EditorWindow {

	static public float height = 5f;

	static private int pWidth = 250;
	static private int pHeight = 100;
	static private int eSize = 110;
	static private int hSpan = 10;
	static private int vSpan = 1;

	void OnGUI() {
		minSize = new Vector2 ((float)pWidth, (float)pHeight);

		EditorGUILayout.BeginHorizontal ();
		GUILayout.Space (hSpan);
		if (GUILayout.Button ("Create", GUILayout.Width(eSize))) {
			Transform[] tt = GLDTools.GetSelection();
			foreach (Transform t in tt) {
				if (t.tag == "Floor") {
					GameObject go = GLDTools.NoCloneName((GameObject)Instantiate (Resources.Load ("Environment/Ceiling"), t.position, t.rotation));
					Align (go.transform);
					GLDTools.FixTiling (go.transform);
					Selection.activeGameObject = go;
				}
			}
		}
		GUILayout.Space (hSpan);
		if (GUILayout.Button ("Align", GUILayout.Width(eSize))) {
			Transform[] tt = GLDTools.GetSelection();
			foreach (Transform t in tt) {
				if (t.tag == "Ceiling") {
					Align (t);
				}
			}
		}
		GUILayout.Space (hSpan);
		EditorGUILayout.EndHorizontal ();

		GUILayout.Space (vSpan);

		EditorGUILayout.BeginHorizontal ();
		GUILayout.Space (hSpan);
		if (GUILayout.Button ("Set", GUILayout.Width(eSize))) {
			Transform[] tt = GLDTools.GetSelection ();
			foreach (Transform t in tt) {
				if (t.tag == "Ceiling") {
					RaycastHit h;
					if (Physics.Raycast (t.position, t.up, out h) && h.transform.root.tag == "Floor") {
						t.position = h.point + h.transform.root.up * height;
					}
				}
			}
		}
		GUILayout.Space (hSpan);
		EditorGUIUtility.labelWidth = eSize / 2f;
		height = EditorGUILayout.FloatField ("Height", height, GUILayout.Width(eSize));
		GUILayout.Space (hSpan);
		EditorGUILayout.EndHorizontal ();
	}

	private void Align(Transform t) {
		GameObject[] ff = GameObject.FindGameObjectsWithTag ("Floor");
		if (ff.Length == 0) return;
		GameObject f = ff [0];
		for (int i = 1; i < ff.Length; i += 1)
			if ((t.position - ff [i].transform.position).magnitude < (t.position - f.transform.position).magnitude)
				f = ff [i];

		t.rotation = f.transform.rotation;
		t.position = f.transform.position + f.transform.up * height;
		t.Rotate(new Vector3(0f, 0f, 180f));
		t.localScale = f.transform.localScale / 10f;
	}
}