using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject bossPrefab;
	public float startDepth = 2.0f;

	BossController bossController;
	GameObject player;
	bool fightStarted = false;
	int score = 0;
	float time;

	void Start() {
		player = GameObject.FindWithTag ("Player");

		LoadNextBoss (startDepth);
	}
	
	public void AddScore(int points) {
		this.score += points;
	}

	public void MinusTime(float lives) {
		this.time -= bossController.GetBossTime() / lives;
	}

	public void LoadNextBoss(float depth) {
		GameObject boss = (GameObject)Instantiate (bossPrefab, this.transform.position, Quaternion.identity);
		bossController = boss.GetComponent<BossController> ();
		bossController.Init (depth);
		time = bossController.GetBossTime ();

		fightStarted = false;
	}

	void Update() {
		if (!fightStarted) {
			fightStarted = true;
			bossController.OnFightStart ();
		} else {
			time -= Time.deltaTime;

			if (time <= 0) {
				time = 0.00f;
				Destroy (player);
			}
		}
	}

	public int GetScore() {
		return score;
	}

	public float GetTime() {
		return time;
	}
}
