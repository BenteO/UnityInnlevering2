using UnityEngine;
using System.Collections;

public class Fireball: MonoBehaviour {

	float bounceHeight = 1;
	float timeToApex = .2f;
	float gravity = -10;
	float bounceVelocity;
	public float moveVelocity;
	public bool hit = false;
	bool bounce = false;

	Vector3 velocity;
	Vector3 interactionVector = new Vector3(0.01f, 0.01f, 0);

	Animator anim;
	Controller2D controller;
	InteractionController interaction;

	void Start() {
		anim = GetComponentInChildren<Animator>();
		controller = GetComponent<Controller2D>();
		interaction = GetComponent<InteractionController>();
		velocity.y = gravity;
	}

	// Update is called once per frame
	void Update() {
		// Interaction
		interaction.detect(interactionVector);

		if(controller.collisions.right || controller.collisions.left || interaction.collisions.left || interaction.collisions.right || interaction.collisions.above || interaction.collisions.below) {
			moveVelocity = 0;
			StartCoroutine("hitAndDestroy");
		}

		// To stop gravity from accumulating
		if(controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;
		}

		// bounce
		if(controller.collisions.below) {
			gravity = -(2 * bounceHeight) / Mathf.Pow(timeToApex, 2); // s = v0 * t + 1/2 * a * t^2 -> a = 2s/t^2. Siden gravitasjonen fungerer mot positiv retning (opp) gjør vi den negativ
			bounceVelocity = Mathf.Abs(gravity) * timeToApex; // v = a * t. Vi tar absoluttverdien av gravity for å alltid få positiv jumpVelocity
			velocity.y = bounceVelocity;
			bounce = true;
		}

		if(bounce) {
			velocity.y += gravity * Time.deltaTime;
		}

		velocity.x = moveVelocity;
		controller.move(velocity * Time.deltaTime);


		if(transform.position.x < -0.5 || transform.position.y < -0.5) {
			Destroy(this.gameObject);
		}
	}

	IEnumerator hitAndDestroy() {
		velocity = Vector3.zero;
		gravity = 0;
		hit = true;
		anim.SetBool("Hit", hit);
		yield return new WaitForSeconds(0.2f);
		Destroy(this.gameObject);
	}
}
