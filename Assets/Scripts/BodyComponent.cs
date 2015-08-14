using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

public class BodyComponent : MonoBehaviour {
	
	public float hp;
	public bool isCore;
	public int points; //given after being destroyed
	public Material onHitMaterial;
	public float onHitRenderTime = 0.1f;
	[SerializeField] FaceType[] faceTypes = new FaceType[6];
	//[SerializeField] float scale;

	bool hasMoved;
	//BodyComponent masterBodyComponent;
	BossController bossController;
	Vector3 location;
	Rigidbody rigdBody;
	Renderer renderer;
	Material defaultMaterial;
	float onHitTimer;
	GameController gameController;

	void Start() {
		rigdBody = this.GetComponent<Rigidbody>();
		renderer = this.GetComponent<Renderer>();
		defaultMaterial = renderer.material;
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>();

		for (int i = 0; i < 6; i++){
			FaceType face = faceTypes[i];
			if (face != null) {
				face.Init(this, i);
			}
		}
	}

	void Update() {
		hasMoved = false;

		if (onHitTimer > 0.00f) {
			onHitTimer -= Time.deltaTime;
			if (onHitTimer <= 0.00f) {
				renderer.material = defaultMaterial;
			}
		}
	}


	public void Init(BossController bossController, Vector3 location) {
		this.bossController = bossController;
		this.location = location;

		/*
		this.masterBodyComponent = masterBodyComponent;
		if (this.masterBodyComponent != null) {
			this.transform.parent = masterBodyComponent.transform;
		}
		*/
	}



	public void Move(Vector3 velocity) {
		if (!hasMoved) {
			hasMoved = true;

			rigdBody.AddForce(velocity, ForceMode.VelocityChange);

			foreach (FaceType face in faceTypes) {
				if (face != null) {
					if (face is FaceTypeConnector) {
						FaceTypeConnector ftc = (FaceTypeConnector)face;
						if (ftc.GetConnectedFace() != null) {
							BodyComponent bc = ftc.GetConnectedFace().GetBodyComponent();
							if (!bc.HasMoved()) {
								bc.Move (velocity);
							}
						}
					}
				}
			}
		}
	}



	public bool HasMoved() {
		return hasMoved;
	}


	public FaceType[] GetFaceTypes() {
		return faceTypes;
	}

	/*
	public float GetScale() {
		return scale;
	}
	*/

	/*
	public BodyComponent GetMasterBodyComponent(){
		return masterBodyComponent;
	}
	*/
	
	public FaceType AddFace(int side,  GameObject facePrefab) {
		if (side < 0 || side > 6) {
			return null;
		}

		if (faceTypes[side] != null) {
			return null;
		}
		GameObject face = (GameObject)Instantiate(facePrefab);
		FaceType faceType = face.GetComponent<FaceType> ();
		faceType.Init (this, side);
		faceTypes [side] = faceType;

		return faceType;
	}

	public void OnHit() {
		Destroy ();
	}

	public void Destroy() {
		foreach (FaceType face in faceTypes) {
			if (face != null) {
				face.Destroy();
			}
		}

		bossController.RemoveFromBodyComponentList (location);

		gameController.AddScore (points);

		Explosive explosive = GetComponent<Explosive> ();
		if (explosive != null) {
			explosive.Explode();
		}

		Destroy (gameObject);
	}

	public void OnFightStart() {
		foreach (FaceType face in faceTypes) {
			if (face != null) {
				face.OnFightStart();
			}
		}
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "PlayerBullet") {
			PlayerBullet pb = collision.gameObject.GetComponent<PlayerBullet>();

			if (isCore) {
				//check to see if no other connected bodies
				foreach (FaceType face in faceTypes) {
					if (face != null) {
						if (face is FaceTypeConnector) {
							FaceTypeConnector ftc = (FaceTypeConnector)face;
							if (ftc.GetConnectedFace() != null) {
								if (!ftc.GetConnectedFace().GetBodyComponent().isCore) {
									pb.OnBounce();
									return;
								}
							}
						}
					}
				}
			}

			OnHit(pb.GetDamage());
			pb.OnHit();
		}
	}

	public void OnHit(float damage) {
		renderer.material = onHitMaterial;
		onHitTimer = onHitRenderTime;

		hp -= damage;
		if (hp <= 0) {
			Destroy ();
		}
	}
}
