using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxController))]
public class BreakableBrickScript: MonoBehaviour {

	// Components
	Animator anim;
	BoxController controller;

	// Variables
	bool activate = false;
	public GameObject itemInBlock;
	public bool used = false;
	public int itemAmount = 1;
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
		// Sets animator parameters
		anim.SetInteger("Mario Health", GameController.gameController.health);
		anim.SetInteger("Coins", itemAmount);

		// Detector
		controller.detector(detectorLength);

		// Activate
		if(Mario.hitUp && controller.collisions.below) {
			activate = true;
		} else {
			activate = false;
		}
		anim.SetBool("MarioJumpUnder", activate);
		// Use
		if(used) {
			AudioManager.audioManager.PlayFX(bump);
			if(itemInBlock != null) {
				AudioManager.audioManager.PlayFX(spawnSound);
				item = (GameObject) Instantiate(itemInBlock, thisPosition, Quaternion.identity);
				itemAmount--;
			}
			used = false;
		}
	}
}
