using UnityEditor;
using UnityEngine;

public class GLDMenu : MonoBehaviour {

	[MenuItem ("GLD/Floor")]
	static void Floor() {
		EditorWindow.GetWindow(typeof(FloorBuilder));
	}

	[MenuItem ("GLD/Texels")]
	static void Texels() {
		EditorWindow.GetWindow(typeof(TexelsBuilder));
	}

	[MenuItem ("GLD/Ceiling")]
	static void Ceiling() {
		EditorWindow.GetWindow(typeof(CeilingBuilder));
	}

	[MenuItem ("GLD/Walls")]
	static void Walls() {
		EditorWindow.GetWindow(typeof(WallBuilder));
	}

	[MenuItem ("GLD/Bridges")]
	static void Bridges() {
		EditorWindow.GetWindow(typeof(BridgeBuilder));
	}

	[MenuItem ("GLD/Fix Tiling")]
	static void FixTextures () {
		foreach (Transform t in GLDTools.GetSelection()) GLDTools.FixTiling (t);
	}

	[MenuItem ("GLD/Debug/Show")]
	static void DebugShow () {
		Transform[] tt = GLDTools.GetSelection();
		foreach (Transform t in tt) {
			HideInfo[] hii = t.GetComponentsInChildren<HideInfo> (true);
			foreach (HideInfo hi in hii) {
				hi.transform.gameObject.SetActive (true);
			}
		}
	}

	[MenuItem ("GLD/Debug/Hide")]
	static void DebugHide () {
		Transform[] tt = GLDTools.GetSelection();
		foreach (Transform t in tt) {
			HideInfo[] hii = t.GetComponentsInChildren<HideInfo> ();
			foreach (HideInfo hi in hii) {
				if (hi.hideMe)
					hi.transform.gameObject.SetActive (false);
			}
		}
	}

}
