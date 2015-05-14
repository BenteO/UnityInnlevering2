using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Score : MonoBehaviour {

	public GameObject UIText;
	Text thisScore;

	// Use this for initialization
	void Start() {
		thisScore = GetComponent<Text>();
	}

	// Update is called once per frame
	void Update() {
		thisScore.text = GameController.gameController.score.ToString("D6");
	}
}
