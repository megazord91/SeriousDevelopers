using UnityEngine;
using UnityEditor;
using System.Collections;

public class WallBuilder : EditorWindow {

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
			Vector3 origin = tt.Length == 1 ? tt[0].position : Vector3.zero;
			GameObject go = GLDTools.NoCloneName ((GameObject) Instantiate (Resources.Load ("Environment/Wall"), origin, Quaternion.identity));
			go.transform.position += go.transform.up * (go.transform.localScale.y / 2f);
		}
		GUILayout.Space (hSpan);
		if (GUILayout.Button ("Put Down", GUILayout.Width(eSize))) {
			Transform[] tt = GLDTools.GetSelection ();
			foreach (Transform t in tt) {
				if (t.tag == "Wall") {
					RaycastHit h;
					if (Physics.Raycast (t.position, -t.up, out h)) {
						t.localRotation = h.transform.root.localRotation;
						t.position = h.point + t.localRotation * (Vector3.up * t.localScale.y / 2f);
					} else if (Physics.Raycast (t.position, t.up, out h) && h.transform.root.tag == "Floor") {
						t.localRotation = h.transform.root.localRotation;
						t.position = h.point + t.localRotation * (Vector3.up * ((t.localScale.y / 2f) + h.transform.root.localScale.y)) ;
					}
				}
			}
		}
		GUILayout.Space (hSpan);
		EditorGUILayout.EndHorizontal ();

		GUILayout.Space (vSpan);

		EditorGUILayout.BeginHorizontal ();
		GUILayout.Space (hSpan);
		if (GUILayout.Button ("Span Vertical", GUILayout.Width(eSize))) {
			Transform[] tt = GLDTools.GetSelection ();
			foreach (Transform t in tt) {
				if (t.tag == "Wall") {
					RaycastHit h;
					Quaternion backupRot = t.localRotation;
					Vector3 backupPos = t.position;
					Vector3 v;
					if (Physics.Raycast (t.position, -t.up, out h) && h.transform.root.tag == "Floor") {
						t.localRotation = h.transform.root.localRotation;
						t.position = h.point + t.localRotation * (Vector3.up * t.localScale.y / 2f);
						v = h.point;
						if (Physics.Raycast (t.position, t.up, out h) && h.transform.root.tag == "Ceiling") {
							t.position = (v + h.point) / 2f;
							Vector3 tmp = t.transform.root.localScale;
							tmp.y = (v - h.point).magnitude;
							t.localScale = tmp;
						} else {
							t.localRotation = backupRot;
							t.position = backupPos;
						}
					} 
				}
			}
		}
		GUILayout.Space (hSpan);
		if (GUILayout.Button ("Span Horizontal", GUILayout.Width(eSize))) {
			Transform[] tt = GLDTools.GetSelection ();
			foreach (Transform t in tt) {
				if (t.tag == "Wall") {
					RaycastHit h;
					Vector3 lft, rgt;
					if (Physics.Raycast (t.position, -t.right, out h)) {
						lft = h.point;
						if (Physics.Raycast (t.position, t.right, out h)) {
							rgt = h.point;
							t.position = (lft + rgt) / 2f;
							Vector3 tmp = t.localScale;
							tmp.x = (rgt - lft).magnitude;
							t.localScale = tmp;
						}
					}
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
				if (t.tag == "Wall") {
					Vector3 s = t.localScale;
					t.position = t.position + t.up * ( height - s.y ) / 2f;
					s.y = height;
					t.localScale = s;
				}
			}
		}
		GUILayout.Space (hSpan);
		EditorGUIUtility.labelWidth = eSize / 2f;
		height = EditorGUILayout.FloatField ("Height", height, GUILayout.Width(eSize));
		GUILayout.Space (hSpan);
		EditorGUILayout.EndHorizontal ();

	}

}
