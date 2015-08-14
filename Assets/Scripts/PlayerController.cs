using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float hp = 1;
	public float lives = 5.0f;
	public float stunTime = 1.0f;
	public Material onHitMaterial;

	Vector3 startPos;
	GameController gameController;
	float stunnedTime;
	Material defaultMaterial;
	Renderer renderer;

	void Start() {
		gameController = GameObject.FindWithTag ("GameController").GetComponent<GameController> ();

		renderer = this.GetComponentInChildren<Renderer>();
		defaultMaterial = renderer.material;

		startPos = this.transform.position;
	}

	void Update() {
		if (stunnedTime > 0) {
			stunnedTime -= Time.deltaTime;
			if (stunnedTime <= 0) {
				renderer.material = defaultMaterial;
			}
		}
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Projectile") {
			Projectile projectile = collision.gameObject.GetComponent<Projectile>();
			OnHit(projectile);
		}
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag == "Projectile") {
			Projectile projectile = collider.gameObject.GetComponent<Projectile>();
			OnHit(projectile);
		}
	}

	void OnHit(Projectile projectile) {
		if (stunnedTime <= 0) {
			stunnedTime = stunTime;
			renderer.material = onHitMaterial;

			hp -= projectile.GetDamage();
			
			if (hp <= 0) {
				OnDie();
			}
		}
	}

	void OnDie() {
		gameController.MinusTime (lives);

		this.transform.position = startPos;

		//Destroy (this.gameObject);
	}
}
