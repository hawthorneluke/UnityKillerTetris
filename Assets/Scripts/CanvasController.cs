using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasController : MonoBehaviour {

	public Text textScore;
	public Text textTime;

	GameController gameController;

	void Start() {
		gameController = GameObject.FindWithTag ("GameController").GetComponent<GameController> ();
	}

	void Update () {
		textScore.text = "Score: " + gameController.GetScore ();
		textTime.text = "Time: " + gameController.GetTime ();
	}
}
