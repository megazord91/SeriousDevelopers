using UnityEngine;
using System.Collections;

public enum Plane { XY, XZ, ZY }

public class TilingInfo : MonoBehaviour  {

	public Plane applicatonPlane = Plane.XY;

	[Range(0.01f, 20f)] public float scaleFactor = 10.0f;

	[HideInInspector] public Material ownMaterial;
}