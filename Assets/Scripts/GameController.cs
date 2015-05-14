using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameController: MonoBehaviour {

	// This
	public static GameController gameController;

	// Game Variables
	public int health = 1;
	public int lives = 3;
	public int timer = 400;
	public int score = 0;
	public int topScore = 0;
	public int coins = 0;
	public bool fromPipe = false;
	bool timeIsUp = false;

	// Checks if another GameController exists and either keeps or destroys this
	void Awake() {
		// Sets resolution
		Screen.fullScreen = false;
		Screen.SetResolution(512, 448, false);

		if(gameController == null) {
			// Keeps the object in all scenes
			DontDestroyOnLoad(this.gameObject);
			gameController = this;
		} else if(gameController != this) {
			Destroy(this.gameObject);
		}
	}

	void Update() {

		print(Time.timeScale);
		// Pause
		if(Input.GetButtonDown("Cancel")) {
			if(Time.timeScale == 1) {
				Time.timeScale = 0;
			} else if(Time.timeScale == 0) {
				Time.timeScale = 1;
			}
		}

		// Timer
		if(timer <= 0 && !timeIsUp) {
			timeIsUp = true;
			lives--;
			StartCoroutine(TimeUp());
		}

		// Coins
		if(coins >= 100) {
			lives++;
			coins -= 100;
		}
	}

	public void pipeDown() {
		StopAllCoroutines();
		StartCoroutine(WaitLoad(3f, "1-1Underground"));
	}

	public void pipeUp() {
		StopAllCoroutines();
		StartCoroutine(WaitLoad(3f, "1-1"));
	}

	public void healthZero() {
		health = 1;
		lives--;
		timer = 400;
		if(lives <= 0) {
			print("lives <= 0");
			StopAllCoroutines();
			StartCoroutine(WaitLoadWaitLoad(2f, "Game Over Scene", "Main Scene"));
		} else {
			print("lives > 0");
			StopAllCoroutines();
			StartCoroutine(WaitLoadWaitLoad(2f, "Death Scene", "1-1"));
		}
	}

	// To restart the entire game
	void restart() {
		// Resets the variables
		health = 1;
		lives = 3;
		timer = 400;
		score = 0;
		coins = 0;
		fromPipe = false;

		// Reloads main menu
		Application.LoadLevel("Main Scene");
	}

	public void startGame() {
		StartCoroutine(WaitLoadWaitLoad(0, "Death Scene", "1-1"));
		StartCoroutine("Countdown");
	}

	// Coroutine reduces timer by 1 each loop (The game is faster than real-time)
	IEnumerator Countdown() {
		yield return new WaitForSeconds(1f);
		while(timer > 0) {
			yield return new WaitForSeconds(0.4f);
			timer--;
		}
	}

	IEnumerator TimeUp() {
		health = 0;
		yield return new WaitForSeconds(2f);
		Application.LoadLevel("TimeUp");
		if(lives > 0) {
			StartCoroutine(WaitLoadWaitLoad(3f, "Death Scene", "1-1"));
		} else {
			StartCoroutine(WaitLoadWaitLoad(3f, "Game Over Scene", "Main Scene"));
		}
	}

	IEnumerator WaitLoad(float wait, string sceneName) {
		yield return new WaitForSeconds(wait);
		print("Changing to " + sceneName);
		Application.LoadLevel(sceneName);
		if(sceneName.Equals("1-1") || sceneName.Equals("1-1Underground")) {
			StartCoroutine("Countdown");
		} else {
			StopCoroutine("Countdown");
		}
	}

	IEnumerator WaitLoadWaitLoad(float firstWait, string firstSceneName, string secondSceneName) {
		yield return new WaitForSeconds(firstWait);
		print("Changing to " + firstSceneName);
		Application.LoadLevel(firstSceneName);
		if(firstSceneName.Equals("1-1") || firstSceneName.Equals("1-1Underground")) {
			StartCoroutine("Countdown");
		} else {
			StopCoroutine("Countdown");
		}
		yield return new WaitForSeconds(3f);
		print("Changing to " + secondSceneName);
		Application.LoadLevel(secondSceneName);
		if(secondSceneName.Equals("1-1") || secondSceneName.Equals("1-1Underground")) {
			StartCoroutine("Countdown");
		} else {
			StopCoroutine("Countdown");
		}
	}

	// Save
	public void saveScore() {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/scoreData.dat");

		ScoreData data = new ScoreData();
		data.score = score;
		bf.Serialize(file, data);
		file.Close();
		print("Saved");
	}

	// Load
	public void loadScore() {
		if(File.Exists(Application.persistentDataPath + "/scoreData.dat")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/scoreData.dat",FileMode.Open);
			ScoreData data = (ScoreData) bf.Deserialize(file);
			file.Close();
			topScore = data.score;
			print("Loaded");
		} else {
			topScore = 0;
		}
	}
}

// Write to file
[Serializable]
class ScoreData {
	public int score;
}