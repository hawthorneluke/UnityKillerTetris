using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossController : MonoBehaviour {

	public GameObject corePrefab;
	public GameObject bodyPrefab;
	public GameObject faceTypeConnector;
	public GameObject faceTypeEngine;
	public GameObject faceTypeShot;
	public GameObject faceTypeChargeLaser;
	public float timeMultiplier = 1.0f;
	public float depthMultiplier = 1.5f;

	//List<BodyComponent> bodyComponents = new List<BodyComponent>();
	Dictionary<Vector3, BodyComponent> bodyComponents = new Dictionary<Vector3, BodyComponent>();
	Dictionary<Vector3, FaceType> weapons = new Dictionary<Vector3, FaceType>();

	GameController gameController;
	float depth;

	void Start() {
		gameController = GameObject.FindWithTag ("GameController").GetComponent<GameController>();
	}

	public void Init(float depth) {
		this.depth = depth;
		Vector3 location = Vector3.zero;
		BodyComponent bc = NewBodyComponent (location, true);
		bc.transform.localPosition = Vector3.zero;
		Generate (bc, depth, location);
		
		
		/*
		BodyComponent bc = NewBodyComponent (new Vector3(0, 0, 0));
		bc.transform.localPosition = Vector3.zero;
		int side = 3;
		FaceTypeConnector ftc = (FaceTypeConnector)bc.AddFace (side, faceTypeConnector);


		bc = NewBodyComponent (new Vector3(1, 0, 0));
		side = 2;
		FaceTypeConnector ftc2 = (FaceTypeConnector)bc.AddFace (side, faceTypeConnector);
		ftc2.ConnectToFace (ftc);
		side = 0;
		ftc = (FaceTypeConnector)bc.AddFace (side, faceTypeConnector);

		bc = NewBodyComponent (new Vector3(2, 0, 0));
		side = 5;
		ftc2 = (FaceTypeConnector)bc.AddFace (side, faceTypeConnector);
		ftc2.ConnectToFace (ftc);
		*/
	}

	void Update() {
		int coreCount = 0;
		foreach (BodyComponent bc in bodyComponents.Values) {
			if (bc.isCore) {
				coreCount++;
			}
		}

		if (coreCount <= 0) {
			OnDefeated();
		}
	}

	void OnDefeated() {
		List<BodyComponent> values = new List<BodyComponent> (bodyComponents.Values);
		foreach (BodyComponent bc in values) {
			bc.Destroy();
		}
		bodyComponents.Clear ();
		weapons.Clear ();

		gameController.LoadNextBoss (depth * depthMultiplier);

		Destroy (this.gameObject);
	}

	void Generate(BodyComponent bodyComponent, float size, Vector3 location) {
		for (int i = 0; i < 6; i++) {
			if (bodyComponent.GetFaceTypes()[i] == null) {
				if (Random.Range(0.0f, size) > 0.5f) {

					Vector3 newLocation = location;
					
					switch(i) {
					case 0:
						newLocation.y -= 1;
						break;
					case 5:
						newLocation.y += 1;
						break;
					case 1:
						newLocation.x -= 1;
						break;
					case 4:
						newLocation.x += 1;
						break;
					case 2:
						newLocation.z -= 1;
						break;
					case 3:
						newLocation.z += 1;
						break;
					}
					
					
					//if there's space
					if (!bodyComponents.ContainsKey(newLocation) && !weapons.ContainsKey(newLocation)) {
						
						if (Random.Range(0.0f, size) < 1.0f) {

							//weapon
							FaceType weapon = null;

							if (Random.Range(0.0f, 1.0f) < FaceTypeChargeLaser.generateChance) {
								weapon = (FaceType)bodyComponent.AddFace (i, faceTypeChargeLaser);

							}
							else if (Random.Range(0.0f, 1.0f) < FaceTypeShot.generateChance) {
								weapon = (FaceType)bodyComponent.AddFace (i, faceTypeShot);
							}

							if (weapon != null) {
								weapons.Add (newLocation, weapon);
								weapon.OnReadyActivation();
							}

						}
						else {
							//connector

							FaceTypeConnector ftc = (FaceTypeConnector)bodyComponent.AddFace (i, faceTypeConnector);

							BodyComponent newBc;
							if (Random.Range(0.0f, 1.0f) < size / 30.0f) {
								//core
								newBc = NewBodyComponent(newLocation, true);
							}
							else {
								newBc = NewBodyComponent(newLocation);
							}
							int side = 5 - i;
							FaceTypeConnector ftc2 = (FaceTypeConnector)newBc.AddFace (side, faceTypeConnector);
							ftc2.ConnectToFace (ftc);

							//make connections to existing blocks
							/*
							for (int j = 0; j < 6; j++) {
								Vector3 loc = newLocation;
								switch(j) {
								case 0:
									loc.y -= 1;
									break;
								case 5:
									loc.y += 1;
									break;
								case 1:
									loc.x -= 1;
									break;
								case 4:
									loc.x += 1;
									break;
								case 2:
									loc.z -= 1;
									break;
								case 3:
									loc.z -= 1;
									break;
								}

								BodyComponent aroundBc;
								if (bodyComponents.TryGetValue(loc, out aroundBc)) {
									side = 5 - j;
									if (aroundBc.GetFaceTypes()[side] == null) {
										FaceTypeConnector aroundFtc = (FaceTypeConnector)aroundBc.AddFace (side, faceTypeConnector);
										FaceTypeConnector thisFtc = (FaceTypeConnector)newBc.AddFace (j, faceTypeConnector);
										thisFtc.ConnectToFace (aroundFtc);
									}
								}
							}
							*/

							//go deeper, size halves
							Generate(newBc, size / 2.0f, newLocation);

						}
					}
				}
			}
		}
	}

	BodyComponent NewBodyComponent(Vector3 location, bool isCore = false) {
		GameObject body;

		if (isCore) {
			body = (GameObject)Instantiate (corePrefab);
		} else {
			body = (GameObject)Instantiate (bodyPrefab);
		}

		body.transform.parent = this.transform;
		body.transform.localPosition = new Vector3 (10000, 10000, 10000);
		BodyComponent bc = body.GetComponent<BodyComponent> ();
		bc.Init (this, location);

		bodyComponents.Add (location, bc);

		return bc;
	}

	public void OnFightStart() {
		foreach (BodyComponent bc in bodyComponents.Values) {
			bc.OnFightStart();
		}
	}

	public void RemoveFromBodyComponentList(Vector3 location) {
		bodyComponents.Remove (location);
	}

	public float GetBossTime() {
		return depth * 60.0f * timeMultiplier;
	}
}
