using UnityEngine;
using System.Collections;

public class GainPoints: MonoBehaviour {
	// For scripts
	public static void increaseScoreStatic(int points) {
		GameController.gameController.score += points;
	}

	// For objects
	public void increaseScore(int points) {
		GameController.gameController.score += points;
	}
}
