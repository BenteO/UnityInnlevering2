using UnityEngine;
using System.Collections;

public class AudioManager: MonoBehaviour {

	// This
	public static AudioManager audioManager;
	
	// Audio Sources
	public AudioSource FXSource;
	public AudioSource MusicSource;

	// Checks if there already exists an audioManager
	void Awake() {
		if(audioManager == null) {
			DontDestroyOnLoad(this.gameObject);
			audioManager = this;
		} else if(audioManager != this) {
			Destroy(this.gameObject);
		}
	}

	// Sets volume
	void Start() {
		FXSource.volume = 0.5f;
		MusicSource.volume = 0.5f;
	}

	// Plays the effect
	public void PlayFX(AudioClip clip) {
		FXSource.clip = clip;
		FXSource.Play();
	}

	// Plays the song
	public void PlayMusic(AudioClip clip) {
		MusicSource.clip = clip;
		MusicSource.Play();
	}

	// Unpauses the song
	public void UnpauseMusic() {
		MusicSource.Play();
	}

	// Pauses the song
	public void PauseMusic() {
		MusicSource.Pause();
	}
}
