using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxController))]
public class InvicibleBlockScript: MonoBehaviour {

	// Components
	Animator anim;
	Mario mario;
	SpriteRenderer spriteRenderer;
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
		mario = GameObject.Find("Mario Parent").GetComponentInChildren<Mario>();
		controller = GetComponent<BoxController>();
		thisPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void Update() {
		// Detector
		controller.detector(detectorLength);
		// If Mario interacts up and his velocity is positive and this detects interaction
		if(Mario.interUp && (mario.velocity.y > 0) && controller.collisions.interaction) {
			activate = true;
			spriteRenderer.color = new Vector4(255, 255, 255, 255);
			controller.collisionMask = LayerMask.GetMask("Player", "Fireball", "Items");
			this.gameObject.layer = 0;
		} else {
			activate = false;
		}
		anim.SetBool("MarioJumpUnder", activate);
		if(used) {
			AudioManager.audioManager.PlayFX(bump);
			AudioManager.audioManager.PlayFX(spawnSound);
			spriteRenderer.color = new Vector4(255, 255, 255, 255);
			controller.collisionMask = LayerMask.GetMask("Player", "Fireball", "Items");
			this.gameObject.layer = 0;
			AudioManager.audioManager.PlayFX(spawnSound);
			item = (GameObject) Instantiate(itemInBlock, thisPosition, Quaternion.identity);
			used = false;
			Destroy(GetComponent<UsedAnimationEvent>());
		}
	}
}
