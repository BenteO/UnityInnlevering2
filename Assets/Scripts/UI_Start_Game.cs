using UnityEngine;
using System.Collections;

public class UI_Start_Game: MonoBehaviour {

	// Method to start game for button
	public void buttonStart() {
		GameController.gameController.startGame();
	}
}
