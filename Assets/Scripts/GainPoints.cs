using UnityEngine;
using System.Collections;

public class GainPoints: MonoBehaviour {
	// For scripts
	public static void increaseScoreStatic(int points) {
		int totalPoints = points * GameController.gameController.scoreMultiplier;
		GameController.gameController.score += totalPoints;
		print("Gained " + totalPoints);
	}
	// Fixed amount
	public static void increaseScoreFixed(int points) {
		GameController.gameController.score += points;
		print("Gained " + points);
	}

	// For objects
	public void increaseScore(int points) {
		int totalPoints = points * GameController.gameController.scoreMultiplier;
		GameController.gameController.score += totalPoints;
		print("Gained " + totalPoints);
	}
}
