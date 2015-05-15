using UnityEngine;
using System.Collections;

public class GainPointsTextPopup: MonoBehaviour {

	public static GainPointsTextPopup textPopup;
	int currentScore = 0;
	int lastScore = 0;
	public int difference = 0;

	// Use this for initialization
	void Awake() {
		textPopup = this;
	}

	// Update is called once per frame
	void Update() {
		currentScore = GameController.gameController.score;
		difference = currentScore - lastScore;
		lastScore = GameController.gameController.score;
	}
}
