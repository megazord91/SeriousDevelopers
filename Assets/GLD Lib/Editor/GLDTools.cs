using UnityEditor;
using UnityEngine;
using System.Collections;

public class GLDTools {

	static public Transform[] GetSelection() {
		Transform[] tt = Selection.transforms;
		if (tt.Length == 0) return GameObject.FindObjectsOfType<Transform> ();
		return tt;
	}

	static public void FixTiling(Transform t) {
		TilingInfo[] tii = t.GetComponentsInChildren<TilingInfo> (); 
		foreach (TilingInfo ti in tii) {
			if (ti != null) {
				if (ti.ownMaterial == null) {
					ti.ownMaterial = new Material (ti.transform.GetComponent<Renderer> ().sharedMaterial);
				}
				Plane ap = ti.applicatonPlane;

				ti.ownMaterial.mainTextureScale = 
					new Vector2 (((ap == Plane.XY || ap == Plane.XZ || ap == Plane.XZ) ? ti.transform.root.localScale.x : ti.transform.root.localScale.z) / ti.scaleFactor, 
						((ap == Plane.XY || ap == Plane.ZY) ? ti.transform.root.localScale.y : ti.transform.root.localScale.z) / ti.scaleFactor);
				ti.transform.GetComponent<Renderer> ().material = ti.ownMaterial;
			}
		}
	}

	static public GameObject NoCloneName(GameObject go) {
		go.name = go.name.Substring (0, go.name.Length - 7);
		return go;
	}

}