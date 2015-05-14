using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Lives: MonoBehaviour {

	public GameObject UIText;
	Text thisLives;

	// Use this for initialization
	void Start() {
		thisLives = GetComponent<Text>();
	}

	// Update is called once per frame
	void Update() {
		thisLives.text = GameController.gameController.lives.ToString("D1");
	}
}
