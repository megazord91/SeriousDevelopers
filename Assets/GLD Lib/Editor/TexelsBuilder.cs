using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

enum CurveType { Up, Down };


public class TexelsBuilder : EditorWindow {

	float sx = 1f, sz = 1f, h = 1f;
	int tx = 1, tz = 1;
	bool curve = false;
	CurveType updown = CurveType.Up;

	static private int pWidth = 250;
	static private int pHeight = 100;
	static private int eSize = 110;
	static private int hSpan = 10;
	static private int vSpan = 1;

	void OnGUI() {

		minSize = new Vector2 ((float)pWidth, (float)pHeight);

		EditorGUILayout.BeginHorizontal ();
		GUILayout.Space (hSpan);
		EditorGUIUtility.labelWidth = eSize;
		EditorGUILayout.PrefixLabel ("Texels size");
		EditorGUIUtility.labelWidth = eSize * 0.2f;
		sx = EditorGUILayout.FloatField ("X", sx, GUILayout.Width((int) (eSize * 0.5f)));
		sz = EditorGUILayout.FloatField ("Z", sz, GUILayout.Width((int) (eSize * 0.5f)));
		GUILayout.Space (hSpan);
		EditorGUILayout.EndHorizontal ();

		GUILayout.Space (vSpan);

		EditorGUILayout.BeginHorizontal ();
		GUILayout.Space (hSpan);
		EditorGUIUtility.labelWidth = eSize;
		EditorGUILayout.PrefixLabel ("N. of texels");
		EditorGUIUtility.labelWidth = eSize * 0.2f;
		tx = EditorGUILayout.IntField ("X", tx, GUILayout.Width((int) (eSize * 0.5f)));
		tz = EditorGUILayout.IntField ("Z", tz, GUILayout.Width((int) (eSize * 0.5f)));
		GUILayout.Space (hSpan);
		EditorGUILayout.EndHorizontal ();

		GUILayout.Space (vSpan);

		curve = EditorGUILayout.BeginToggleGroup ("Curve", curve);

		EditorGUIUtility.labelWidth = eSize / 2f;
		EditorGUILayout.BeginHorizontal ();
		GUILayout.Space (hSpan);
		updown = (CurveType) EditorGUILayout.EnumPopup("Direction", updown, GUILayout.Width(eSize));
		GUILayout.Space (hSpan);
		EditorGUIUtility.labelWidth = eSize / 2.5f;
		h = EditorGUILayout.FloatField ("height", h, GUILayout.Width((int) (eSize * 0.8)));
		GUILayout.Space (hSpan);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.EndToggleGroup ();

		GUILayout.Space (vSpan);

		EditorGUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button ("Build", GUILayout.Width(eSize))) {
			Transform[] tt = GLDTools.GetSelection();
			Vector3 origin = tt.Length == 1 ? tt[0].position : Vector3.zero;
			Vector3 nc = origin - new Vector3 ((((float)tx) / 2f) * sx, 0f, (((float)tz) / 2f) * sz);
			Vector3 sc = new Vector3 (sx, 0.02f, sz);
			GameObject root = new GameObject ();
			root.name = "Texels";
			root.transform.position = origin;
			root.transform.rotation = Quaternion.identity;
			for (int i = 0; i < tx; i += 1) {
				for (int j = 0; j < tz; j += 1) {
					GameObject go = GLDTools.NoCloneName ((GameObject)Instantiate (Resources.Load ("Environment/Floor"), nc + new Vector3 ((float)i * sx, 0f, (float)j * sz), Quaternion.identity));
					go.transform.localScale = sc;
					go.transform.parent = root.transform;
					if (curve) {
						float d;
						Vector3 s = go.transform.localScale;
						if (updown == CurveType.Down) {
							float di = (Mathf.Sin (((float)i * Mathf.PI) / (float)(tx - 1)) - 1f) * Mathf.Abs (h);
							float dj = (Mathf.Sin (((float)j * Mathf.PI) / (float)(tz - 1)) - 1f) * Mathf.Abs (h);
							d = Mathf.Min (di, dj);
							s.y = Mathf.Max(h + d, 0.02f);
							go.transform.Translate(new Vector3(0f, ((d - h)/ 2f), 0f));
						} else {
							float di = h - Mathf.Sin (((float)i * Mathf.PI) / (float)(tx - 1)) * Mathf.Abs (h);
							float dj = h - Mathf.Sin (((float)j * Mathf.PI) / (float)(tz - 1)) * Mathf.Abs (h);
							d = Mathf.Max (di, dj);
							s.y = Mathf.Max(d, 0.02f);
							go.transform.Translate(new Vector3(0f, d / 2f, 0f));
						}
						go.transform.localScale = s;
					}
				}
			}
		}
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal ();
	}
}