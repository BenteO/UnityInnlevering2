using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class EnemyScript: MonoBehaviour {

	public float moveSpeed = 0;
	public float gravity;

	Controller2D controller;
	InteractionController interaction;
	Animator anim;
	Vector3 enemySpeed;
	Vector3 interactionVector = new Vector3(0.01f, 0.01f, 0);

	void Start() {
		controller = GetComponent<Controller2D>();
		interaction = GetComponent<InteractionController>();
		anim = GetComponentInChildren<Animator>();
	}

	void Update() {
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
		if(interaction.collisions.above && interaction.tagCollisions.above == "Player") {
			print("die");
			moveSpeed = 0;
			anim.Play("DieJump");
		}
	}
}
