using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Lives: MonoBehaviour {

	public GameObject UIText;
	Text thisLives;

	void Start() {
		thisLives = GetComponent<Text>();
	}

	// Updats lives text on Death Scene
	void Update() {
		thisLives.text = GameController.gameController.lives.ToString("D1");
	}
}
