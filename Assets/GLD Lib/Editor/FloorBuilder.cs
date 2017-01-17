using UnityEngine;
using UnityEditor;
using System.Collections;

public class FloorBuilder : EditorWindow {

	static private int pWidth = 250;
	static private int pHeight = 75;
	static private int eSize = 110;
	static private int hSpan = 10;
	static private int vSpan = 1;

	private float fx = 20f;
	private float fz = 20f;
	private float tx = 200f;
	private float tz = 200f;
	private float rHeight = 5f;

	void OnGUI() {
		minSize = new Vector2 ((float)pWidth, (float)pHeight);

		EditorGUILayout.BeginHorizontal ();
		GUILayout.Space (hSpan);
		if (GUILayout.Button ("Create", GUILayout.Width(eSize))) {
			Transform[] tt = GLDTools.GetSelection();
			Vector3 origin = tt.Length == 1 ? tt[0].position : Vector3.zero;
			GameObject go = GLDTools.NoCloneName ((GameObject) Instantiate (Resources.Load ("Environment/Floor"), origin, Quaternion.identity));
			go.transform.localScale = new Vector3 (fx, 0.02f, fz);
			go.transform.position = origin;
			GLDTools.FixTiling (go.transform);
		}
		GUILayout.Space (hSpan);
		EditorGUIUtility.labelWidth = eSize * 0.2f;
		fx = EditorGUILayout.FloatField ("X", fx, GUILayout.Width((int) (eSize * 0.5f)));
		fz = EditorGUILayout.FloatField ("Z", fz, GUILayout.Width((int) (eSize * 0.5f)));
		GUILayout.Space (hSpan);
		EditorGUILayout.EndHorizontal ();

		GUILayout.Space (vSpan);

		EditorGUILayout.BeginHorizontal ();
		GUILayout.Space (hSpan);
		if (GUILayout.Button ("Terrain", GUILayout.Width(eSize))) {
			Transform[] tt = GLDTools.GetSelection();
			Vector3 origin = tt.Length == 1 ? tt[0].position : Vector3.zero;
			TerrainData terrainData = new TerrainData();
			terrainData.size = new Vector3( tx, Mathf.Max(tx, tz), tz );
			GameObject go = Terrain.CreateTerrainGameObject( terrainData );
			go.transform.position = new Vector3 (-tx / 2f, 0f, -tz / 2f) + origin;

		}
		GUILayout.Space (hSpan);
		EditorGUIUtility.labelWidth = eSize * 0.2f;
		tx = EditorGUILayout.FloatField ("X", tx, GUILayout.Width((int) (eSize * 0.5f)));
		tz = EditorGUILayout.FloatField ("Z", tz, GUILayout.Width((int) (eSize * 0.5f)));
		GUILayout.Space (hSpan);
		EditorGUILayout.EndHorizontal ();

		GUILayout.Space (vSpan);

		EditorGUILayout.BeginHorizontal ();
		GUILayout.Space (hSpan);
		if (GUILayout.Button ("Railings", GUILayout.Width(eSize))) {
			Transform[] tt = GLDTools.GetSelection();
			foreach (Transform t in tt) {
				if (t.tag == "Floor") {
					Quaternion q = t.rotation;
					t.rotation = Quaternion.identity;
					Vector3 sz = t.localScale;

					GameObject n = GLDTools.NoCloneName ((GameObject) GameObject.Instantiate (Resources.Load ("Environment/Railing"), t.position + new Vector3(0f, rHeight / 2f, sz.z / 2f), Quaternion.identity));
					GameObject s = GLDTools.NoCloneName ((GameObject) GameObject.Instantiate (Resources.Load ("Environment/Railing"), t.position + new Vector3(0f, rHeight / 2f, -sz.z / 2f), Quaternion.identity));
					GameObject e = GLDTools.NoCloneName ((GameObject) GameObject.Instantiate (Resources.Load ("Environment/Railing"), t.position + new Vector3(sz.x / 2f, rHeight / 2f, 0f), Quaternion.identity));
					GameObject w = GLDTools.NoCloneName ((GameObject) GameObject.Instantiate (Resources.Load ("Environment/Railing"), t.position + new Vector3(-sz.x / 2f, rHeight / 2f, 0f), Quaternion.identity));

					n.transform.localScale = new Vector3 (sz.x, rHeight, 0.02f);
					s.transform.localScale = new Vector3 (sz.x, rHeight, 0.02f);
					e.transform.localScale = new Vector3 (0.02f, rHeight, sz.z);
					w.transform.localScale = new Vector3 (0.02f, rHeight, sz.z);

					n.transform.parent = t;
					s.transform.parent = t;
					e.transform.parent = t;
					w.transform.parent = t;

					t.rotation = q;
				}
			}
		}
		GUILayout.Space (hSpan);
		EditorGUIUtility.labelWidth = eSize / 2f;
		rHeight = EditorGUILayout.FloatField ("Height", rHeight, GUILayout.Width(eSize));
		GUILayout.Space (hSpan);
		EditorGUILayout.EndHorizontal ();

	}


}
