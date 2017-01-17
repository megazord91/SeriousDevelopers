using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BridgeBuilder : EditorWindow {

	static float width = 2f;
	private float rHeight = 5f;

	// find sample points around the edge of a platform/transform
	private static Vector3[] SamplePoints(Transform t) {

		float w = t.localScale.x / 2f;
		float h = t.localScale.z / 2f;

		List<Vector3> r = new List<Vector3> ();
		Vector3 s = t.position + new Vector3 (-w, 0f, -h);
		Vector3 e = t.position + new Vector3 (-w, 0f, h);
		float d = (e - s).magnitude;
		for (float f = 0f; f < d; f += 1f) {
			r.Add (s + (((e - s).normalized * f)));
		}
		s = e;
		e = t.position + new Vector3 (w, 0f, h);
		d = (e - s).magnitude;
		for (float f = 0f; f < d; f += 1f) {
			r.Add (s + (((e - s).normalized * f)));
		}
		s = e;
		e = t.position + new Vector3 (w, 0f, -h);
		d = (e - s).magnitude;
		for (float f = 0f; f < d; f += 1f) {
			r.Add (s + (((e - s).normalized * f)));
		}
		s = e;
		e = t.position + new Vector3 (-w, 0f, -h);
		d = (e - s).magnitude;
		for (float f = 0f; f < d; f += 1f) {
			r.Add (s + (((e - s).normalized * f)));
		}
		return r.ToArray();
	}

	// find linked points bridging two set of samples
	private static Vector3[] ClosestPairs(Vector3[] v1, Vector3[] v2) {

		// distance between every bridging pair
		float[,] d = new float[v1.Length, v2.Length];
		for (int i = 0; i < v1.Length; i += 1) {
			for (int j = 0; j < v2.Length; j += 1) {
				d[i,j] = (v1[i] - v2[j]).magnitude;
			}
		}
		// for every point on first set calculate the closest point in the second set (with distance)
		int[] i2j = new int[v1.Length];
		float[] i2jd = new float[v1.Length];
		for (int i = 0; i < v1.Length; i += 1) {
			i2j [i] = 0;
			i2jd [i] = d [i, 0];
			for (int j = 1; j < v2.Length; j += 1) {
				if (i2jd [i] > d [i, j]) {
					i2j [i] = j;
					i2jd [i] = d [i, j];
				}
			}
		}
		// find the shortest bridging point
		int bp = 0;
		for (int i = 1; i < i2j.Length; i += 1) {
			if (i2jd[bp] > i2jd[i]) bp = i;
		}

		// find all couples with same distance as the bridging point
		float bd = i2jd[bp];
		bool[] sl = new bool[i2j.Length];
		int slc = 0;
		for (int i = 0; i < i2j.Length; i += 1) {
			if (sl [i] = (i2jd [i] == bd)) slc += 1; // }:)
		}

		int[] fi = new int[slc];
		int[] fj = new int[slc];
		int k = 0;
		for (int i = 0; i < sl.Length; i += 1) {
			if (sl [i]) {
				fi [k] = i;
				fj [k] = i2j[i];
				k += 1;
			}
		}

		// then, find the middle point
		Vector3 ms = Vector3.zero;
		Vector3 me = Vector3.zero;
		for (int i = 0; i < fi.Length; i += 1) {
			ms += v1 [fi [i]];
			me += v2 [fj [i]];
		}
		ms = ms / slc;
		me = me / slc;

		// and the dridge must go from [0] to [1] (maybe)
		Vector3[] r = new Vector3[2];
		r[0] = ms;
		r[1] = me;
		return r;
	}

	private GameObject Build(Transform t1, Transform t2) {
		Vector3[] se = ClosestPairs (SamplePoints (t1.transform), SamplePoints (t2.transform));
		Vector3 mp = (se [0] + se [1]) / 2f;
		GameObject br = GLDTools.NoCloneName((GameObject)Instantiate (Resources.Load ("Environment/Bridge"), mp, Quaternion.identity));

		float l = (se [0] - se [1]).magnitude;
		br.transform.localScale = new Vector3 (width, 0.02f, l);
		br.transform.rotation = Quaternion.FromToRotation (Vector3.forward, se [1] - se [0]);
		Vector3 r = br.transform.rotation.eulerAngles;
		r.z = 0f;
		br.transform.rotation = Quaternion.Euler (r);
		GLDTools.FixTiling (br.transform);
		return br;
	}

	static private int pWidth = 250;
	static private int pHeight = 70;
	static private int eSize = 110;
	static private int hSpan = 10;
	static private int vSpan = 1;

	void OnGUI() {
		minSize = new Vector2 ((float)pWidth, (float)pHeight);
		EditorGUILayout.LabelField ("Select 2 objects");
		GUILayout.Space (vSpan);

		EditorGUILayout.BeginHorizontal ();
		GUILayout.Space (hSpan);
		if (GUILayout.Button ("Build", GUILayout.Width(eSize))) {
			Transform[] tt = GLDTools.GetSelection ();
			if (tt.Length != 2) return;
			Build (tt[0], tt[1]);
		}
		GUILayout.Space (hSpan);
		EditorGUIUtility.labelWidth = eSize / 2f;
		width = EditorGUILayout.FloatField ("Width", width, GUILayout.Width(eSize));
		GUILayout.Space (hSpan);
		EditorGUILayout.EndHorizontal ();

		GUILayout.Space (vSpan);

		EditorGUILayout.BeginHorizontal ();
		GUILayout.Space (hSpan);
		if (GUILayout.Button ("Railings", GUILayout.Width(eSize))) {
			Transform[] tt = GLDTools.GetSelection();
			foreach (Transform t in tt) {
				Debug.Log (t.tag);
				if (t.tag == "Bridge") {
					Debug.Log ("A");
					Quaternion q = t.rotation;
					t.rotation = Quaternion.identity;
					Vector3 sz = t.localScale;

					//GameObject n = GLDTools.NoCloneName ((GameObject) GameObject.Instantiate (Resources.Load ("Environment/Railing"), t.position + new Vector3(0f, rHeight / 2f, sz.z / 2f), Quaternion.identity));
					//GameObject s = GLDTools.NoCloneName ((GameObject) GameObject.Instantiate (Resources.Load ("Environment/Railing"), t.position + new Vector3(0f, rHeight / 2f, -sz.z / 2f), Quaternion.identity));
					GameObject e = GLDTools.NoCloneName ((GameObject) GameObject.Instantiate (Resources.Load ("Environment/Railing"), t.position + new Vector3(sz.x / 2f, rHeight / 2f, 0f), Quaternion.identity));
					GameObject w = GLDTools.NoCloneName ((GameObject) GameObject.Instantiate (Resources.Load ("Environment/Railing"), t.position + new Vector3(-sz.x / 2f, rHeight / 2f, 0f), Quaternion.identity));

					//n.transform.localScale = new Vector3 (sz.x, rHeight, 0.02f);
					//s.transform.localScale = new Vector3 (sz.x, rHeight, 0.02f);
					e.transform.localScale = new Vector3 (0.02f, rHeight, sz.z);
					w.transform.localScale = new Vector3 (0.02f, rHeight, sz.z);

					//n.transform.parent = t;
					//s.transform.parent = t;
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