// Origina script taken from https://www.assetstore.unity3d.com/en/#!/content/59471

using UnityEngine;
using System.Collections.Generic;

public enum LightningBoltAnimationMode { NONE, RANDOM, LOOP, BACKFORTH }

public class LightningGenerator : MonoBehaviour {

    [Range(0, 8)] public int generations = 6;
    [Range(0.01f, 1.0f)] public float boltDuration = 0.05f;
	[Range(0.01f, 5f)] public float totalDuration = 1.5f;
    [Range(0.0f, 1.0f)] public float chaosFactor = 0.15f;
    [Range(1, 64)] public int rowsInTexture = 1;
    [Range(1, 64)] public int columnsInTexture = 1;
	[Range(0.01f, 250f)] public float maxDistance = 250f; 

	public LightningBoltAnimationMode AnimationMode = LightningBoltAnimationMode.RANDOM;

	private Vector3 startPos;
	private Vector3 endPos;

	private System.Random randomGenerator = new System.Random();
	private LineRenderer lr;
	private List<KeyValuePair<Vector3, Vector3>> segments = new List<KeyValuePair<Vector3, Vector3>> ();
	private int startIndex;
	private Vector2 size;
	private Vector2[] offsets;
	private int animationOffsetIndex;
	private int animationPingPongDirection = 1;

	private void NormalVector(ref Vector3 directionNormalized, out Vector3 side) {
		if (directionNormalized == Vector3.zero) {
			side = Vector3.right;
		} else {
			// use cross product to find any perpendicular vector around directionNormalized:
			// 0 = x * px + y * py + z * pz
			// => pz = -(x * px + y * py) / z
			// for computational stability use the component farthest from 0 to divide by
			float x = directionNormalized.x;
			float y = directionNormalized.y;
			float z = directionNormalized.z;
			float px, py, pz;
			float ax = Mathf.Abs (x), ay = Mathf.Abs (y), az = Mathf.Abs (z);
			if (ax >= ay && ay >= az) {
				// x is the max, so we can pick (py, pz) arbitrarily at (1, 1):
				py = 1.0f;
				pz = 1.0f;
				px = -(y * py + z * pz) / x;
			} else if (ay >= az) {
				// y is the max, so we can pick (px, pz) arbitrarily at (1, 1):
				px = 1.0f;
				pz = 1.0f;
				py = -(x * px + z * pz) / y;
			} else {
				// z is the max, so we can pick (px, py) arbitrarily at (1, 1):
				px = 1.0f;
				py = 1.0f;
				pz = -(x * px + y * py) / z;
			}
			side = new Vector3 (px, py, pz).normalized;
		}
	}

	private void GenerateLightningBolt(Vector3 start, Vector3 end, int generation, int totalGenerations, float offsetAmount) {
		if (generation < 0 || generation > 8) {
			return;
		}

		segments.Add (new KeyValuePair<Vector3, Vector3> (start, end));
		if (generation == 0) {
			return;
		}

		Vector3 randomVector;
		if (offsetAmount <= 0.0f) {
			offsetAmount = (end - start).magnitude * chaosFactor;
		}

		while (generation-- > 0) {
			int previousStartIndex = startIndex;
			startIndex = segments.Count;
			for (int i = previousStartIndex; i < startIndex; i++) {
				start = segments [i].Key;
				end = segments [i].Value;

				// determine a new direction for the split
				Vector3 midPoint = (start + end) * 0.5f;

				// adjust the mid point to be the new location
				RandomVector (ref start, ref end, offsetAmount, out randomVector);
				midPoint += randomVector;

				// add two new segments
				segments.Add (new KeyValuePair<Vector3, Vector3> (start, midPoint));
				segments.Add (new KeyValuePair<Vector3, Vector3> (midPoint, end));
			}

			// halve the distance the lightning can deviate for each generation down
			offsetAmount *= 0.5f;
		}
	}

	public void RandomVector(ref Vector3 start, ref Vector3 end, float offsetAmount, out Vector3 result) {
		if (Camera.current != null && Camera.current.orthographic) {
			end.z = start.z;
			Vector3 directionNormalized = (end - start).normalized;
			Vector3 side = new Vector3 (-directionNormalized.y, directionNormalized.x, end.z);
			float distance = ((float)randomGenerator.NextDouble () * offsetAmount * 2.0f) - offsetAmount;
			result = side * distance;
		} else {
			Vector3 directionNormalized = (end - start).normalized;
			Vector3 side;
			NormalVector (ref directionNormalized, out side);

			// generate random distance
			float distance = (((float)randomGenerator.NextDouble () + 0.1f) * offsetAmount);

			// get random rotation angle to rotate around the current direction
			float rotationAngle = ((float)randomGenerator.NextDouble () * 360.0f);

			// rotate around the direction and then offset by the perpendicular vector
			result = Quaternion.AngleAxis (rotationAngle, directionNormalized) * side * distance;
		}
	}

	private void SelectOffsetFromAnimationMode() {
		int index;

		if (AnimationMode == LightningBoltAnimationMode.NONE) {
			lr.material.mainTextureOffset = offsets [0];
			return;
		} else if (AnimationMode == LightningBoltAnimationMode.BACKFORTH) {
			index = animationOffsetIndex;
			animationOffsetIndex += animationPingPongDirection;
			if (animationOffsetIndex >= offsets.Length) {
				animationOffsetIndex = offsets.Length - 2;
				animationPingPongDirection = -1;
			} else if (animationOffsetIndex < 0) {
				animationOffsetIndex = 1;
				animationPingPongDirection = 1;
			}
		} else if (AnimationMode == LightningBoltAnimationMode.LOOP) {
			index = animationOffsetIndex++;
			if (animationOffsetIndex >= offsets.Length) {
				animationOffsetIndex = 0;
			}
		} else {
			index = randomGenerator.Next (0, offsets.Length);
		}

		if (index >= 0 && index < offsets.Length) {
			lr.material.mainTextureOffset = offsets [index];
		} else {
			lr.material.mainTextureOffset = offsets [0];
		}
	}

	private void UpdateLineRenderer() {
		int segmentCount = (segments.Count - startIndex) + 1;
		lr.SetVertexCount (segmentCount);

		if (segmentCount < 1) {
			return;
		}

		int index = 0;
		lr.SetPosition (index++, segments [startIndex].Key);

		for (int i = startIndex; i < segments.Count; i++) {
			lr.SetPosition (index++, segments [i].Value);
		}

		segments.Clear ();

		SelectOffsetFromAnimationMode ();
	}

	public void UpdateFromMaterialChange() {
		size = new Vector2 (1.0f / (float)columnsInTexture, 1.0f / (float)rowsInTexture);
		lr.material.mainTextureScale = size;
		offsets = new Vector2[rowsInTexture * columnsInTexture];
		for (int y = 0; y < rowsInTexture; y++) {
			for (int x = 0; x < columnsInTexture; x++) {
				offsets [x + (y * columnsInTexture)] = new Vector2 ((float)x / columnsInTexture, (float)y / rowsInTexture);
			}
		}
	}
		
	private void Start() {
		lr = GetComponent<LineRenderer> ();
		lr.SetVertexCount (0);
		UpdateFromMaterialChange ();
	}

	private float timer;
	private float bigTimer;
	Vector3 end;

	private void Update() {
		if (bigTimer > 0f) {
			if (timer <= 0f) {
				timer = boltDuration + Mathf.Min (0.0f, timer);
				startIndex = 0;
				GenerateLightningBolt (transform.position, end, generations, generations, 0.0f);
				UpdateLineRenderer ();
			}
			timer -= Time.deltaTime;
			bigTimer -= Time.deltaTime;
		} else {
			lr.SetVertexCount (0);
		}
	}

	public void Fire(Transform t) {
		if (bigTimer <= 0f) {
			end = t == null ? transform.position + transform.root.forward * maxDistance : t.position;
			bigTimer = totalDuration;
		}
	}
}