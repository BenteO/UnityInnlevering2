using UnityEngine;
using System.Collections;

public class GainPoints: MonoBehaviour {

	// This
	public static GainPoints gainPoints;

	public GameObject[] PointPrefabs = new GameObject[9];
	public Vector3 PointPrefabSpawn;
	GameObject AttatchedPointPrefab;

	void Awake() {
		gainPoints = this;
	}

	// Multiplied amount
	public void increaseScoreMultiplier(int points) {
		// Multiplies
		int totalPoints = points * GameController.gameController.scoreMultiplier;
		// Increases score
		GameController.gameController.score += totalPoints;
		// Spawns score text
		for(int i = 0; i < PointPrefabs.Length; i++) {
			if((PointPrefabs[i].ToString().Replace(" (UnityEngine.GameObject)", "")) == (totalPoints.ToString() + " Parent")) {
				AttatchedPointPrefab = (GameObject) Instantiate(PointPrefabs[i], PointPrefabSpawn, Quaternion.identity);
			}
		}
	}
	// Fixed amount
	public void increaseScoreFixed(int points) {
		// Increases Score
		GameController.gameController.score += points;
		// Spawns Score Text except if the score is 50 (Bricks) or we're underground (floating coins dont show points gained)
		if(points != 50 && !Application.loadedLevelName.Equals("1-1Underground")) {
			for(int i = 0; i < PointPrefabs.Length; i++) {
				if((PointPrefabs[i].ToString().Replace(" (UnityEngine.GameObject)", "")) == (points.ToString() + " Parent")) {
					AttatchedPointPrefab = (GameObject) Instantiate(PointPrefabs[i], PointPrefabSpawn, Quaternion.identity);
				}
			}
		}
	}
}
