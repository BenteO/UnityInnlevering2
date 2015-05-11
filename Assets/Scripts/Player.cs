using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class Player: MonoBehaviour {

	float jumpHeight = 5;
	float timeToJumpApex = .5f;
	float accelerationTimeAirborne = 0.5f;
	float accelerationTimeGrounded = 0.3f;
	float moveSpeed = 6;

	float gravity;
	float jumpVelocity;
	Vector3 velocity;
	float velocityXSmoothing;

	Controller2D controller;

	void Start() {
		controller = GetComponent<Controller2D>();

		gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		print("Gravity: " + gravity + " Jump Velocity: " + jumpVelocity);
	}

	void Update() {

		if(controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;
		}

		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		if(Input.GetButtonDown("Jump") && controller.collisions.below) {
			velocity.y = jumpVelocity;
		}

		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
		;
		velocity.y += gravity * Time.deltaTime;
		controller.move(velocity * Time.deltaTime);
	}
}
