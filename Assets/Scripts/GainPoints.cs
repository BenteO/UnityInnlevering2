using UnityEngine;
using System.Collections;

public class GainPoints: MonoBehaviour {

	public static GainPoints gainPoints;

	public GameObject[] PointPrefabs = new GameObject[9];
	public Vector3 PointPrefabSpawn;
	GameObject AttatchedPointPrefab;

	void Awake() {
		gainPoints = this;
	}

	// Multiplied amount
	public void increaseScoreMultiplier(int points) {
		int totalPoints = points * GameController.gameController.scoreMultiplier;
		GameController.gameController.score += totalPoints;
		for(int i = 0; i < PointPrefabs.Length; i++) {
			if((PointPrefabs[i].ToString().Replace(" (UnityEngine.GameObject)", "")) == (totalPoints.ToString() + " Parent")) {
				AttatchedPointPrefab = (GameObject) Instantiate(PointPrefabs[i], PointPrefabSpawn, Quaternion.identity);
			}
		}
		print("Gained " + totalPoints);
	}
	// Fixed amount
	public void increaseScoreFixed(int points) {
		GameController.gameController.score += points;
		if(points != 50) {
			for(int i = 0; i < PointPrefabs.Length; i++) {
				if((PointPrefabs[i].ToString().Replace(" (UnityEngine.GameObject)", "")) == (points.ToString() + " Parent")) {
					AttatchedPointPrefab = (GameObject) Instantiate(PointPrefabs[i], PointPrefabSpawn, Quaternion.identity);
				}
			}
		}
		print("Gained " + points);
	}
}
