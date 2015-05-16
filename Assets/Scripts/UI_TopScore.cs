using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_TopScore: MonoBehaviour {

	public GameObject UIText;
	Text thisTopScore;

	// Loads Topscore
	void Start() {
		thisTopScore = GetComponent<Text>();
		GameController.gameController.loadScore();
		thisTopScore.text = "TOP- " + GameController.gameController.topScore.ToString("D6");
	}
}
