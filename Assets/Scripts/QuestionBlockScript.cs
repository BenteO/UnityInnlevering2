using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxController))]
public class QuestionBlockScript: MonoBehaviour {

	// Components
	Animator anim;
	BoxController controller;

	// Variables
	bool activate = false;
	public GameObject itemInBlock;
	public bool used = false;
	GameObject item = null;
	Vector3 thisPosition;
	Vector3 detectorLength = new Vector3(0, -0.2f, 0);

	// Audio Clips
	public AudioClip spawnSound;
	public AudioClip bump;

	void Start() {
		anim = GetComponent<Animator>();
		controller = GetComponent<BoxController>();
		thisPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
	}

	void Update() {
		// Detector
		controller.detector(detectorLength);
		// Activates if Mario hit something above and this hit something below
		if(Mario.hitUp && controller.collisions.below) {
			activate = true;
		} else {
			activate = false;
		}
		anim.SetBool("MarioJumpUnder", activate);
		// Spawns the item
		if(used) {
			AudioManager.audioManager.PlayFX(bump);
			if(itemInBlock != null) {
				AudioManager.audioManager.PlayFX(spawnSound);
				item = (GameObject) Instantiate(itemInBlock, thisPosition, Quaternion.identity);
			}
			used = false;
			// To avoid getting used again
			Destroy(GetComponent<UsedAnimationEvent>());
		}
	}
}
