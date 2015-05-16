using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Coin: MonoBehaviour {

	public GameObject UIText;
	Text thisCoins;

	void Start() {
		thisCoins = GetComponent<Text>();
	}

	// Updates cointext
	void Update() {
		thisCoins.text = GameController.gameController.coins.ToString("D2");
	}
}
