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
	public int scoreMultiplier = 1;
	bool timeIsUp = false;
	public bool star = false;
	public bool gameFinish = false;
	public bool fireworks1 = false;
	public bool fireworks2 = false;
	public bool fireworks3 = false;
	public bool raiseFlag = false;
	bool finishing = false;
	bool runningOutOfTime = false;
	bool playingFasterMusic = false;

	// Audio Clips
	public AudioClip[] audioClips = new AudioClip[11];

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
				AudioManager.audioManager.PauseMusic();
				AudioManager.audioManager.PlayFX(audioClips[10]);
			} else if(Time.timeScale == 0) {
				Time.timeScale = 1;
				AudioManager.audioManager.UnpauseMusic();
				AudioManager.audioManager.PlayFX(audioClips[10]);
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

		// Faster Music
		runningOutOfTime = (timer < 100) ? true : false;
		if(runningOutOfTime && !finishing) {
			if(Application.loadedLevelName == "1-1" && !playingFasterMusic) {
				playingFasterMusic = true;
				AudioManager.audioManager.PlayMusic(audioClips[1]);
			} else if(Application.loadedLevelName == "1-1Underground" && !playingFasterMusic) {
				playingFasterMusic = true;
				AudioManager.audioManager.PlayMusic(audioClips[3]);
			}
		}
	}

	// Going down to the Underworld
	public void pipeDown() {
		AudioManager.audioManager.PlayMusic(audioClips[9]);
		StopAllCoroutines();
		StartCoroutine(WaitLoad(3f, "1-1Underground"));
	}

	// Back up to the Overworld
	public void pipeUp() {
		AudioManager.audioManager.PlayMusic(audioClips[9]);
		StopAllCoroutines();
		StartCoroutine(WaitLoad(3f, "1-1"));
	}

	// IF health is Zero
	public void healthZero() {
		health = 1;
		lives--;
		star = false;
		if(lives <= 0) {
			StopAllCoroutines();
			StartCoroutine(WaitLoadWaitLoad(3f, "Game Over Scene", "Main Scene"));
		} else {
			StopAllCoroutines();
			StartCoroutine(WaitLoadWaitLoad(3f, "Death Scene", "1-1"));
		}
		timer = 400;
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

	// Coroutine if time is up
	IEnumerator TimeUp() {
		health = 0;
		yield return new WaitForSeconds(2f);
		Application.LoadLevel("TimeUp");
		if(lives > 0) {
			StartCoroutine(WaitLoadWaitLoad(3f, "Death Scene", "1-1"));
		} else {
			AudioManager.audioManager.PlayMusic(audioClips[5]);
			StartCoroutine(WaitLoadWaitLoad(3f, "Game Over Scene", "Main Scene"));
		}
	}

	// Coroutine to wait a set time, then loads scene
	IEnumerator WaitLoad(float wait, string sceneName) {
		yield return new WaitForSeconds(wait);
		Application.LoadLevel(sceneName);
		// Plays correct music
		if(sceneName.Equals("Game Over Scene")) {
			AudioManager.audioManager.PlayMusic(audioClips[5]);
		} else if(sceneName.Equals("1-1") && timer >= 100) {
			AudioManager.audioManager.PlayMusic(audioClips[0]);
		} else if(sceneName.Equals("1-1Underground") && timer >= 100) {
			AudioManager.audioManager.PlayMusic(audioClips[2]);
		} else if(sceneName.Equals("1-1") && runningOutOfTime) {
			AudioManager.audioManager.PlayMusic(audioClips[1]);
		} else if(sceneName.Equals("1-1Underground") && runningOutOfTime) {
			AudioManager.audioManager.PlayMusic(audioClips[3]);
		}
		if(sceneName.Equals("1-1") || sceneName.Equals("1-1Underground")) {
			StartCoroutine("Countdown");
		} else {
			StopCoroutine("Countdown");
		}
		if(sceneName.Equals("Main Scene")) {
			restart();
		}
	}

	// Coroutine that same as over, but twice. Cant run WaitLoad in a row, because they'll run at the same time
	IEnumerator WaitLoadWaitLoad(float firstWait, string firstSceneName, string secondSceneName) {
		yield return new WaitForSeconds(firstWait);
		Application.LoadLevel(firstSceneName);
		if(firstSceneName.Equals("1-1") || firstSceneName.Equals("1-1Underground")) {
			StartCoroutine("Countdown");
		} else {
			StopCoroutine("Countdown");
		}
		// Plays correct music
		if(firstSceneName.Equals("Game Over Scene")) {
			AudioManager.audioManager.PlayMusic(audioClips[5]);
		} else if(firstSceneName.Equals("1-1") && timer >= 100) {
			AudioManager.audioManager.PlayMusic(audioClips[0]);
		} else if(firstSceneName.Equals("1-1Underground") && timer >= 100) {
			AudioManager.audioManager.PlayMusic(audioClips[2]);
		} else if(firstSceneName.Equals("1-1") && runningOutOfTime) {
			AudioManager.audioManager.PlayMusic(audioClips[1]);
		} else if(firstSceneName.Equals("1-1Underground") && runningOutOfTime) {
			AudioManager.audioManager.PlayMusic(audioClips[3]);
		}
		if(firstSceneName.Equals("Main Scene")) {
			restart();
		}
		yield return new WaitForSeconds(5f);
		Application.LoadLevel(secondSceneName);
		if(secondSceneName.Equals("1-1") || secondSceneName.Equals("1-1Underground")) {
			StartCoroutine("Countdown");
		} else {
			StopCoroutine("Countdown");
		}
		// Plays correct music
		if(secondSceneName.Equals("Game Over Scene")) {
			AudioManager.audioManager.PlayMusic(audioClips[5]);
		} else if(secondSceneName.Equals("1-1") && timer >= 100) {
			AudioManager.audioManager.PlayMusic(audioClips[0]);
		} else if(secondSceneName.Equals("1-1Underground") && timer >= 100) {
			AudioManager.audioManager.PlayMusic(audioClips[2]);
		} else if(secondSceneName.Equals("1-1") && runningOutOfTime) {
			AudioManager.audioManager.PlayMusic(audioClips[1]);
		} else if(secondSceneName.Equals("1-1Underground") && runningOutOfTime) {
			AudioManager.audioManager.PlayMusic(audioClips[3]);
		}
		if(secondSceneName.Equals("Main Scene")) {
			restart();
		}
	}
	// Coroutine when mario gets a star. Lasts 10 seconds
	IEnumerator marioInvincible() {
		// Plays star music
		AudioManager.audioManager.PlayMusic(audioClips[4]);
		yield return new WaitForSeconds(10);
		star = false;
		// Plays correct music
		if(Application.loadedLevelName.Equals("1-1") && timer >= 100) {
			AudioManager.audioManager.PlayMusic(audioClips[0]);
		} else if(Application.loadedLevelName.Equals("1-1Underground") && timer >= 100) {
			AudioManager.audioManager.PlayMusic(audioClips[2]);
		} else if(Application.loadedLevelName.Equals("1-1") && runningOutOfTime) {
			AudioManager.audioManager.PlayMusic(audioClips[1]);
		} else if(Application.loadedLevelName.Equals("1-1Underground") && runningOutOfTime) {
			AudioManager.audioManager.PlayMusic(audioClips[3]);
		}
	}

	// Coroutine when we finish the game
	IEnumerator FinishingGame() {
		// Plays the flagpole music
		AudioManager.audioManager.PlayMusic(audioClips[6]);
		// Gets the last digit of the timer
		int lastTimerDigit = timer % 10;
		yield return new WaitForSeconds(1);
		// Plays the victory song
		AudioManager.audioManager.PlayMusic(audioClips[7]);
		// Raises score. 50 points per timeunit left
		do {
			score += 50;
			AudioManager.audioManager.PlayFX(audioClips[11]);
			timer--;
			yield return new WaitForSeconds(0.01f);
		} while(timer > 0);
		if(timer <= 0 && !AudioManager.audioManager.MusicSource.isPlaying) {
			// Raises the tiny flag on top of the castle
			raiseFlag = true;
			// Plays 1, 2 or 3 fireworks if the timer ends with 1, 3 or 6 respectively
			// Each firework is worth 500 points
			if(lastTimerDigit == 1) {
				fireworks1 = true;
				score += 500;
				AudioManager.audioManager.PlayMusic(audioClips[8]);
			} else if(lastTimerDigit == 3) {
				fireworks1 = true;
				score += 500;
				AudioManager.audioManager.PlayMusic(audioClips[8]);
				yield return new WaitForSeconds(1);
				fireworks2 = true;
				score += 500;
				AudioManager.audioManager.PlayMusic(audioClips[8]);
			} else if(lastTimerDigit == 6) {
				fireworks1 = true;
				score += 500;
				AudioManager.audioManager.PlayMusic(audioClips[8]);
				yield return new WaitForSeconds(1);
				fireworks2 = true;
				score += 500;
				AudioManager.audioManager.PlayMusic(audioClips[8]);
				yield return new WaitForSeconds(1);
				fireworks3 = true;
				score += 500;
				AudioManager.audioManager.PlayMusic(audioClips[8]);
			}
		}
		yield return new WaitForSeconds(5);
		// Saves score and reloads the main scene
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
		} else {
			data.score = topScore;
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
		}
	}
}

// Write to file
[Serializable]
class ScoreData {
	public int score;
}