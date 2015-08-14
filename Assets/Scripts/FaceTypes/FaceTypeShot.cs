using UnityEngine;
using System.Collections;

public class FaceTypeShot : FaceType {

	public static float generateChance = 0.5f;

	public float maxMissAngle = 20.0f;
	public GameObject shotPrefab;

	protected override void Ready() {
	}

	protected override void Activate() {
		if (player != null) {
			Vector3 direction = player.transform.position - transform.position;
			Quaternion rotation = Quaternion.LookRotation (direction);
			rotation *= Quaternion.Euler(0.0f, Random.Range(-maxMissAngle/2.0f, maxMissAngle/2.0f), 0.0f);
			Instantiate (shotPrefab, this.transform.position, rotation);
		}
	}

	protected override void ActivateEnd() {
	}
}
