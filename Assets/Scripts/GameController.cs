using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameController: MonoBehaviour {

	// This
	public static GameController gameController;

	// Flag Animator
	public Animator flag;

	// Game Variables
	public int health = 1;
	public int lives = 3;
	public int timer = 400;
	public int score = 0;
	public int topScore = 0;
	public int coins = 0;
	public bool fromPipe = false;
	public int scoreMultiplier = 1;
	bool timeIsUp = false;
	public bool star = false;
	public bool gameFinish = false;
	public bool fireworks1 = false;
	public bool fireworks2 = false;
	public bool fireworks3 = false;
	public bool raiseFlag = false;
	bool finishing = false;

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
		// Finishing the game
		if(gameFinish && !finishing) {
			finishing = true;
			StopCoroutine("Countdown");
			StartCoroutine("FinishingGame");
		}

		// Pause
		if(Input.GetButtonDown("Cancel")) {
			if(Time.timeScale == 1) {
				Time.timeScale = 0;
			} else if(Time.timeScale == 0) {
				Time.timeScale = 1;
			}
		}

		// Timer
		if(timer <= 0 && !timeIsUp && !gameFinish) {
			timeIsUp = true;
			lives--;
			StartCoroutine(TimeUp());
		}

		// Coins
		if(coins >= 100) {
			lives++;
			coins -= 100;
		}

		// Music
		// Overworld
		// 100 - raskere Overworld
		// Underground
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
			StopAllCoroutines();
			StartCoroutine(WaitLoadWaitLoad(2f, "Game Over Scene", "Main Scene"));
		} else {
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
		scoreMultiplier = 1;
		timeIsUp = false;
		star = false;
		gameFinish = false;
		fireworks1 = false;
		fireworks2 = false;
		fireworks3 = false;
		raiseFlag = false;
		finishing = false;
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
		Application.LoadLevel(sceneName);
		if(sceneName.Equals("1-1") || sceneName.Equals("1-1Underground")) {
			StartCoroutine("Countdown");
		} else {
			StopCoroutine("Countdown");
		}
		if(sceneName.Equals("Main Scene")) {
			restart();
		}
	}

	IEnumerator WaitLoadWaitLoad(float firstWait, string firstSceneName, string secondSceneName) {
		yield return new WaitForSeconds(firstWait);
		Application.LoadLevel(firstSceneName);
		if(firstSceneName.Equals("1-1") || firstSceneName.Equals("1-1Underground")) {
			StartCoroutine("Countdown");
		} else {
			StopCoroutine("Countdown");
		}
		if(firstSceneName.Equals("Main Scene")) {
			restart();
		}
		yield return new WaitForSeconds(3f);
		Application.LoadLevel(secondSceneName);
		if(secondSceneName.Equals("1-1") || secondSceneName.Equals("1-1Underground")) {
			StartCoroutine("Countdown");
		} else {
			StopCoroutine("Countdown");
		}
		if(secondSceneName.Equals("Main Scene")) {
			restart();
		}
	}

	IEnumerator marioInvincible() {
		yield return new WaitForSeconds(10);
		star = false;
	}

	IEnumerator FinishingGame() {
		int lastTimerDigit = timer % 10;
		do {
			score += 50;
			timer--;
			yield return new WaitForSeconds(0.01f);
		} while(timer > 0);
		if(timer <= 0) {
			raiseFlag = true;
			if(lastTimerDigit == 1) {
				fireworks1 = true;
				score += 500;
			} else if(lastTimerDigit == 3) {
				fireworks1 = true;
				score += 500;
				yield return new WaitForSeconds(1);
				fireworks2 = true;
				score += 500;
			} else if(lastTimerDigit == 6) {
				fireworks1 = true;
				score += 500;
				yield return new WaitForSeconds(1);
				fireworks2 = true;
				score += 500;
				yield return new WaitForSeconds(1);
				fireworks3 = true;
				score += 500;
			}
		}
		yield return new WaitForSeconds(1);
		saveScore();
		StartCoroutine(WaitLoad(1, "Main Scene"));
	}



	// Save
	public void saveScore() {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/scoreData.dat");

		ScoreData data = new ScoreData();
		if(topScore < score) {
			data.score = score;
		}
		bf.Serialize(file, data);
		file.Close();
	}

	// Load
	public void loadScore() {
		if(File.Exists(Application.persistentDataPath + "/scoreData.dat")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/scoreData.dat", FileMode.Open);
			ScoreData data = (ScoreData) bf.Deserialize(file);
			file.Close();
			topScore = data.score;
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