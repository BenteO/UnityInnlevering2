using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D), typeof(InteractionController))]
public class GoombaScript: MonoBehaviour {

	public float moveSpeed = 0;
	public float gravity;

	Controller2D controller;
	InteractionController interaction;
	Animator anim;
	Vector3 enemySpeed;
	Vector3 interactionVector = new Vector3(0.01f, 0.01f, 0);
	bool dead = false;

	void Start() {
		controller = GetComponent<Controller2D>();
		interaction = GetComponent<InteractionController>();
		anim = GetComponentInChildren<Animator>();
	}

	void Update() {
		if(dead) {
			this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
		}
		// Movement
		if(controller.collisions.below || controller.collisions.above) {
			enemySpeed.y = 0;
		}
		if(controller.collisions.right || controller.collisions.left) {
			moveSpeed = -moveSpeed;
		}

		enemySpeed.x = moveSpeed * Time.deltaTime;
		enemySpeed.y = gravity * Time.deltaTime;
		controller.move(enemySpeed);

		// Interaction
		interaction.detect(interactionVector);
		// Die
		if(GameController.gameController.star) {
			if((interaction.collisions.above || interaction.collisions.left) && (interaction.tagCollisions.above == "Player" || interaction.tagCollisions.left == "Player")) {
				moveSpeed = 0;
				anim.Play("DieFireRight");
				GainPoints.increaseScoreStatic(100);
			} else if((interaction.collisions.below || interaction.collisions.right) && (interaction.tagCollisions.below == "Player" || interaction.tagCollisions.right == "Player")) {
				moveSpeed = 0;
				anim.Play("DieFireLeft");
				GainPoints.increaseScoreStatic(100);
			}
		}

		if(transform.position.y < -1f) {
			Destroy(this.gameObject);
		}
		if(!dead) {
			if(interaction.collisions.above && interaction.tagCollisions.above == "Player" && !interaction.collisions.below) {
				moveSpeed = 0;
				anim.Play("DieJump");
				dead = true;
			}
			if(interaction.tagCollisions.above == "Fireball" || interaction.tagCollisions.left == "Fireball" || interaction.tagCollisions.left == "KoopaTrooperShellMoving" && this.gameObject.layer != LayerMask.NameToLayer("Ignore")) {
				moveSpeed = 0;
				anim.Play("DieFireRight");
				GainPoints.increaseScoreStatic(100);
				dead = true;
			} else if(interaction.tagCollisions.below == "Fireball" || interaction.tagCollisions.right == "Fireball" || interaction.tagCollisions.right == "KoopaTrooperShellMoving" && this.gameObject.layer != LayerMask.NameToLayer("Ignore")) {
				moveSpeed = 0;
				anim.Play("DieFireLeft");
				GainPoints.increaseScoreStatic(100);
				dead = true;
			}
		}
	}
}
