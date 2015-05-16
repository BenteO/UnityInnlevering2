using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Score: MonoBehaviour {

	public GameObject UIText;
	Text thisScore;

	void Start() {
		thisScore = GetComponent<Text>();
	}

	// Updates scoretext
	void Update() {
		thisScore.text = GameController.gameController.score.ToString("D6");
	}
}
