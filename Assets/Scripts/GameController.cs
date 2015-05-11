using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController: MonoBehaviour {

	// This
	public static GameController gameController;

	// Game Variables
	public static int health = 1;
	public static int lives = 3;
	public static int timer = 400;
	public static int score = 0;
	public static int coins = 0;

	// UI Components
	public GameObject playerScore;
	public GameObject playerCoins;
	public GameObject playerTime;
	Text scoreText, coinText, timeText;

	// Checks if another GameController exists and either keeps or destroys this
	void Awake() {
		if(gameController == null) {
			// Keeps the object in all scenes
			DontDestroyOnLoad(this.gameObject);
			gameController = this;
		} else if(gameController != this) {
			Destroy(this.gameObject);
		}
	}

	void Start() {
		// When the game starts, the GameController starts the timer
		StartCoroutine("Countdown");
		// Gets components
		scoreText = playerScore.GetComponent<Text>();
		coinText = playerCoins.GetComponent<Text>();
		timeText = playerTime.GetComponent<Text>();
	}

	void Update() {
		// Timer
		if(timer <= 0) {
			lives--;
		}

		// Health
		if(health <= 0) {
			lives--;
		}

		// Lives
		if(lives <= 0) {
			// Game Over
		}

		// Coins
		if(coins >= 100) {
			lives++;
			coins -= 100;
		}

		// Changes UI
		scoreText.text = score.ToString("D6");
		coinText.text = coins.ToString("D2");
		timeText.text = timer.ToString("D3");
	}

	// To restart the entire game
	void restart() {
		// Resets the variables
		health = 1;
		lives = 3;
		timer = 400;
		score = 0;
		coins = 0;

		// Reloads the scene
		Application.LoadLevel(0);
	}

	// Coroutine reduces timer by 1 each loop (The game is faster than real-time)
	IEnumerator Countdown() {
		while(timer > 0) {
			print("counting down");
			timer--;
			yield return new WaitForSeconds(0.4f);
		}
	}
}
