using UnityEngine;
using System.Collections;

public class FaceTypeEngine : FaceType {

	public float speed;

	void Move(Vector3 velocity) {
		//this.bodyComponent.Move (velocity);
		this.bodyComponent.gameObject.GetComponent<Rigidbody> ().AddForce (velocity, ForceMode.Acceleration);
	}

	void Update() {
		if (Input.GetKey (KeyCode.UpArrow)) {
			Move (Vector3.forward * speed);
		}
	}
	
}
