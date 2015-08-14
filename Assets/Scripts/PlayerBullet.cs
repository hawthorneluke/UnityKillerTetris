using UnityEngine;
using System.Collections;

public class PlayerBullet : MonoBehaviour {

	public float damage;
	public GameObject brokenWatermelonPrefab;
	public int brokenWatermelonPieces = 3;

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Floor") {
			OnHit();
		}
	}

	public void OnHit() {
		for (int i = 0; i < brokenWatermelonPieces; i++) {
			Instantiate (brokenWatermelonPrefab, transform.position, transform.localRotation);
		}
		Destroy (this.gameObject);
	}

	public void OnBounce() {
	}

	public float GetDamage() {
		return damage;
	}
	
}
