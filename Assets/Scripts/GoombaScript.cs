using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D), typeof(InteractionController))]
public class GoombaScript: MonoBehaviour {

	// Components
	Controller2D controller;
	InteractionController interaction;
	Animator anim;

	// Variables
	public float moveSpeed = 0;
	public float gravity;
	Vector3 enemySpeed;
	Vector3 interactionVector = new Vector3(0.01f, 0.01f, 0);
	bool dead = false;

	// Audio Clips
	public AudioClip stomped;
	public AudioClip fired;

	void Start() {
		controller = GetComponent<Controller2D>();
		interaction = GetComponent<InteractionController>();
		anim = GetComponentInChildren<Animator>();
	}

	void Update() {
		// Stop gravity accumulation
		if(controller.collisions.below || controller.collisions.above) {
			enemySpeed.y = 0;
		}
		// Changes direction
		if(controller.collisions.right || controller.collisions.left) {
			moveSpeed = -moveSpeed;
		}
		// Movement
		enemySpeed.x = moveSpeed * Time.deltaTime;
		enemySpeed.y = gravity * Time.deltaTime;
		controller.move(enemySpeed);

		// Interaction
		interaction.detect(interactionVector);
		
		// Die
		// Out of bounds
		if(transform.position.y < -1f) {
			Destroy(this.gameObject);
		}
		if(!dead) {
			// If star is active
			if(GameController.gameController.star) {
				if((interaction.collisions.above || interaction.collisions.left) && (interaction.tagCollisions.above == "Player" || interaction.tagCollisions.left == "Player")) {
					AudioManager.audioManager.PlayFX(fired);
					moveSpeed = 0;
					anim.Play("DieFireRight");
					GainPoints.gainPoints.PointPrefabSpawn = this.transform.position;
					GainPoints.gainPoints.increaseScoreFixed(100);
					dead = true;
				} else if((interaction.collisions.below || interaction.collisions.right) && (interaction.tagCollisions.below == "Player" || interaction.tagCollisions.right == "Player")) {
					AudioManager.audioManager.PlayFX(fired);
					moveSpeed = 0;
					anim.Play("DieFireLeft");
					GainPoints.gainPoints.PointPrefabSpawn = this.transform.position;
					GainPoints.gainPoints.increaseScoreFixed(100);
					dead = true;
				}
			}
			// Stomped
			if(interaction.collisions.above && interaction.tagCollisions.above == "Player" && !interaction.collisions.below) {
				AudioManager.audioManager.PlayFX(stomped);
				moveSpeed = 0;
				anim.Play("DieJump");
				GainPoints.gainPoints.PointPrefabSpawn = this.transform.position;
				GainPoints.gainPoints.increaseScoreMultiplier(100);
				dead = true;
			}
			// Fireballed or Shelled
			if(interaction.tagCollisions.above == "Fireball" || interaction.tagCollisions.left == "Fireball" || interaction.tagCollisions.left == "KoopaTrooperShellMoving" && this.gameObject.layer != LayerMask.NameToLayer("Ignore")) {
				AudioManager.audioManager.PlayFX(fired);
				moveSpeed = 0;
				anim.Play("DieFireRight");
				GainPoints.gainPoints.PointPrefabSpawn = this.transform.position;
				GainPoints.gainPoints.increaseScoreFixed(100);
				dead = true;
			} else if(interaction.tagCollisions.below == "Fireball" || interaction.tagCollisions.right == "Fireball" || interaction.tagCollisions.right == "KoopaTrooperShellMoving" && this.gameObject.layer != LayerMask.NameToLayer("Ignore")) {
				AudioManager.audioManager.PlayFX(fired);
				moveSpeed = 0;
				anim.Play("DieFireLeft");
				GainPoints.gainPoints.PointPrefabSpawn = this.transform.position;
				GainPoints.gainPoints.increaseScoreFixed(100);
				dead = true;
			}
		}
	}
}
