using UnityEngine;
using System.Collections;

public class Shot : Projectile {

	public float speed;
	public float destroyTime;

	void Start() {
		Destroy (this.gameObject, destroyTime);
	}

	void Update () {
		transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Player") {
			Destroy (this.gameObject);
		}
	}
}
