using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FaceTypeConnector : FaceType {

	public float disconnectionForce = 5.0f;

	FaceTypeConnector connectedFace;
	FixedJoint fixedJoint;

	public void ConnectToFace(FaceTypeConnector connectedFace) {
		this.connectedFace = connectedFace;
		connectedFace.SetConnectedFace (this);
		int connectedSide = connectedFace.GetSide ();
		Vector3 connectedFacePos = connectedFace.transform.position;
		//bodyComponent.transform.parent = connectedFace.GetBodyComponent ().transform;

		//fixedJoint = (FixedJoint)gameObject.AddComponent (typeof(FixedJoint));
		//fixedJoint.connectedBody = connectedFace.gameObject.GetComponent<Rigidbody> ();

		this.bodyComponent.transform.position = connectedFacePos;

		switch (connectedSide) {
		case 0:
			PositionSelf(Vector3.up, new Vector3(0.0f, 0.0f, 0.0f));
			break;
		case 5:
			PositionSelf(Vector3.down, new Vector3(0.0f, 0.0f, 180.0f));
			break;
		case 1:
			PositionSelf(Vector3.right, new Vector3(0.0f, 90.0f, 0.0f));
			break;
		case 4:
			PositionSelf(Vector3.left, new Vector3(0.0f, -90.0f, 0.0f));
			break;
		case 2:
			PositionSelf(Vector3.forward, new Vector3(0.0f, 0.0f, 90.0f));
			break;
		case 3:
			PositionSelf(Vector3.back, new Vector3(0.0f, 0.0f, -90.0f));
			break;
		}

		this.bodyComponent.transform.parent = this.transform;

		fixedJoint = (FixedJoint)bodyComponent.gameObject.AddComponent (typeof(FixedJoint));
		fixedJoint.connectedBody = connectedFace.GetBodyComponent().gameObject.GetComponent<Rigidbody> ();
	}

	public void Disconnect() {
		if (connectedFace != null) {
			/*
			if (bodyComponent.transform.parent == connectedFace.GetBodyComponent ().transform) {
				bodyComponent.transform.parent = bodyComponent.transform;
			}
			*/

			if (fixedJoint != null) {
				Destroy(fixedJoint);
			}

			FaceTypeConnector ftc = connectedFace;
			connectedFace = null;
			ftc.Disconnect();
		}
	}

	void PositionSelf(Vector3 pos, Vector3 dir) {
		this.bodyComponent.transform.localPosition -=  pos * ((this.bodyComponent.transform.localScale.x / 2.0f) + (this.transform.localScale.x / 2.0f));

		this.bodyComponent.transform.localRotation = Quaternion.LookRotation (pos);

		switch (side) {
		case 0:
			this.bodyComponent.transform.localRotation *= Quaternion.Euler(-90, 0, 0);
			break;
		case 5:
			this.bodyComponent.transform.localRotation *= Quaternion.Euler(90, 0, 0);
			break;
		case 1:
			this.bodyComponent.transform.localRotation *= Quaternion.Euler(0, 90, 0);
			break;
		case 4:
			this.bodyComponent.transform.localRotation *= Quaternion.Euler(0, -90, 0);
			break;
		case 2:
			this.bodyComponent.transform.localRotation *= Quaternion.Euler(180, 0, 0);
			break;
		case 3:
			this.bodyComponent.transform.localRotation *= Quaternion.Euler(0, 0, 0);
			break;
		}
	}

	public void SetConnectedFace(FaceTypeConnector ftc) {
		this.connectedFace = ftc;
	}

	public FaceTypeConnector GetConnectedFace() {
		return connectedFace;
	}

	public void Destroy() {
		if (connectedFace != null) {
			Vector3 velocity = new Vector3(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)) * disconnectionForce;
			connectedFace.GetBodyComponent().Move(velocity);
		}

		Disconnect ();

		base.Destroy ();
	}

}
