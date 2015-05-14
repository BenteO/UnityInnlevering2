using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Time : MonoBehaviour {

	public GameObject UIText;
	Text thisTime;

	// Use this for initialization
	void Start() {
		thisTime = GetComponent<Text>();
	}

	// Update is called once per frame
	void Update() {
		thisTime.text = GameController.gameController.timer.ToString("D3");
	}
}
