using UnityEngine;
using System.Collections;

public class UI_Start_Game: MonoBehaviour {

	public void buttonStart() {
		GameController.gameController.startGame();
	}
}
