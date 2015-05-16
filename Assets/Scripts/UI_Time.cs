using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Time: MonoBehaviour {

	public GameObject UIText;
	Text thisTime;

	void Start() {
		thisTime = GetComponent<Text>();
	}

	// Updates timer
	void Update() {
		thisTime.text = GameController.gameController.timer.ToString("D3");
	}
}
