using UnityEngine;
using System.Collections;

public class FaceTypeChargeLaser : FaceType {

	public static float generateChance = 0.1f;

	public GameObject chargeLaserReadyPrefab;

	GameObject chargeLaserReadyInstance;
	GameObject chargeLaserInstance;

	protected override void Ready() {
		if (player != null) {
			Quaternion rotation = Quaternion.LookRotation(player.transform.position - this.transform.position);
			rotation *= Quaternion.Euler(90, 0, 0);
			chargeLaserReadyInstance = (GameObject)Instantiate (chargeLaserReadyPrefab, this.transform.position, rotation);

			Vector3 scale = chargeLaserReadyInstance.transform.localScale;
			scale.y = 1000.0f;
			chargeLaserReadyInstance.transform.localScale = scale;
		}
	}

	protected override void Activate() {
		if (chargeLaserReadyInstance != null) {

			if (player != null) {
				chargeLaserInstance = (GameObject)Instantiate (chargeLaserReadyPrefab, chargeLaserReadyInstance.transform.position, chargeLaserReadyInstance.transform.rotation);
				
				Vector3 scale = chargeLaserInstance.transform.localScale;
				scale.y = 1000.0f;
				scale.x = 50.0f;
				scale.z = 50.0f;
				chargeLaserInstance.transform.localScale = scale;

				chargeLaserInstance.GetComponentInChildren<CapsuleCollider>().enabled = true;
			}

			Destroy (chargeLaserReadyInstance);
			chargeLaserReadyInstance = null;
		}
	}

	protected override void ActivateEnd() {
		if (chargeLaserInstance != null) {
			Destroy (chargeLaserInstance);
			chargeLaserInstance = null;
		}
	}

	void OnDestroy() {
		if (chargeLaserReadyInstance != null) {
			Destroy (chargeLaserReadyInstance);
			chargeLaserReadyInstance = null;
		}

		if (chargeLaserInstance != null) {
			Destroy (chargeLaserInstance);
			chargeLaserInstance = null;
		}
	}
}
