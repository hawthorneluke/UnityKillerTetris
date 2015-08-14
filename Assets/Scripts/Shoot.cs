using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {

	public GameObject bulletPrefab;
	public float speed;
	public float destroyTime;
	public float nextFireTime;

	float nextFireTimer;

	void Update () {
		nextFireTimer -= Time.deltaTime;

		if (nextFireTimer <= 0) {
			if (Input.GetButton ("Fire1")) {
				Fire ();
			}
		}
	}

	void Fire() {
		nextFireTimer = nextFireTime;
		float distance = Camera.main.farClipPlane;
		Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
		pos = Camera.main.ScreenToWorldPoint(pos);
		Vector3 heading = pos - transform.position;
		heading.Normalize ();
		Vector3 spawnPos = transform.position + heading * 1;
		spawnPos.y += 1.0f;
		GameObject bullet = (GameObject)Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
		bullet.transform.forward = heading;   
		Rigidbody rb = bullet.GetComponent<Rigidbody> ();
		rb.AddForce (bullet.transform.forward * speed);

		Destroy (bullet, destroyTime);
	}
}
