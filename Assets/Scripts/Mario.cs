﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class Mario: MonoBehaviour {

	// Jumping
	float jumpHeight = 1.5f;
	float timeToJumpApex = 0.2f;
	float gravity;
	float jumpVelocity;
	float jumpVelocityFraction = 18.4f;
	int jumpHoldCount = 0;
	int jumpHoldCountMax = 35;

	// Movement
	float accelerationTimeAirborne = 0.4f;
	float accelerationTimeGrounded = 0.4f;
	float moveSpeed = 5f;
	float moveSpeedRun = 5f;
	float moveSpeedSprint = 9;
	public Vector3 velocity;
	float velocityXSmoothing;
	private bool facingRight = true;
	public bool turning = false;

	// GameObject Components
	public Animator anim;
	Controller2D controller;
	InteractionController interaction;
	BoxCollider2D boxCollider;
	public GameObject fireball;
	public GameObject fireballSpawner;
	GameObject attatchedFireball = null;

	// Collisions for any other objects that need access
	public bool hitUp;
	public bool hitDown;
	public bool hitLeft;
	public bool hitRight;
	public bool interUp;
	public bool interDown;
	public bool interLeft;
	public bool interRight;

	// Ingame Variables
	public bool invincible = false;
	public bool transforming = false;
	public bool shooting = false;
	public bool crouching = false;
	public bool pipe = false;
	int fireballAmount = 0;
	bool gameFinish = false;
	Vector3 interactionVector = new Vector3(0.01f, 0.01f, 0);
	bool dead = false;
	bool canControl = true;
	bool recentlyDamaged = false;


	void Start() {
		boxCollider = GetComponent<BoxCollider2D>();
		controller = GetComponent<Controller2D>();
		interaction = GetComponent<InteractionController>();
		// MATH!
		gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2); // s = v0 * t + 1/2 * a * t^2 -> a = 2s/t^2. Siden gravitasjonen fungerer mot positiv retning (opp) tar vi den negative verdien
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex; // v = a * t. Vi tar absoluttverdien av gravity for å alltid få positiv jumpVelocity
		anim.SetInteger("Health", GameController.gameController.health);
		if(GameController.gameController.fromPipe && Application.loadedLevelName == "1-1") {
			transform.position = new Vector3(163.5f, 4f, 0);
		}
	}

	void Update() {
		anim.SetBool("Star", GameController.gameController.star);
		// BoxCollider2D resize
		if(GameController.gameController.health == 1) {
			boxCollider.size = new Vector2(boxCollider.size.x, 1);
			boxCollider.offset = new Vector2(0, 0);
		} else if(GameController.gameController.health <= 2 && !crouching){
			boxCollider.size = new Vector2(boxCollider.size.x, 2);
			boxCollider.offset = new Vector2(0, 0.5f);
		}

		// Collisions 
		hitUp = controller.collisions.above;
		hitDown = controller.collisions.below;
		hitLeft = controller.collisions.left;
		hitRight = controller.collisions.right;
		// Interactions
		interUp = interaction.collisions.above;
		interDown = interaction.collisions.below;
		interLeft = interaction.collisions.left;
		interRight = interaction.collisions.right;
		interaction.detect(interactionVector);

		// Checks if Mario is touching the ground and sends bool to animator.
		if(!pipe) {
			anim.SetBool("isGrounded", hitDown);
		}
		
		// To stop gravity from accumulating
		if(controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;
		}

// Losing health
		// Out of bounds
		if(transform.position.y < -1 && !dead) {
			canControl = false;
			dead = true;
			velocity.x = 0;
			velocity.y = 0;
			GameController.gameController.healthZero();
		}
		// Touches enemy
		if(!GameController.gameController.star) {
			if((interaction.tagCollisions.left == "KoopaTrooperShellMoving" || interaction.tagCollisions.right == "KoopaTrooperShellMoving" || interaction.tagCollisions.left == "Enemy" || interaction.tagCollisions.right == "Enemy" || interaction.tagCollisions.above == "Enemy") && !interaction.collisions.below && (interaction.tagCollisions.below != "Enemy")) {
				if(GameController.gameController.health > 1 && !recentlyDamaged) {
					GameController.gameController.health = 1;
					recentlyDamaged = true;
					StartCoroutine("transformCoroutine");
				} else if(GameController.gameController.health == 1 && !recentlyDamaged) {
					GameController.gameController.health = 0;
					canControl = false;
				}
			}
		}
		anim.SetInteger("Health", GameController.gameController.health);

// Input
		if(canControl) {
			// Bounce off enemies
			if(interaction.tagCollisions.below == "Enemy") {
				Vector3 thisPosition = transform.position;
				thisPosition.y += 0.3f;
				transform.position = thisPosition;
				velocity.y = jumpVelocity;
				StopCoroutine("multiplierUp");
				StartCoroutine("multiplierUp");
			}

			Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
			// Jumping
			if(Input.GetButtonDown("Jump") && controller.collisions.below) {
				velocity.y = jumpVelocity;
			}
			// Jump higher
			if(Input.GetButton("Jump") && (jumpHoldCount < jumpHoldCountMax)) {
				velocity.y += jumpVelocity / jumpVelocityFraction;
				jumpHoldCount++;
			}

			// Resets or maxes jumpHoldCount to stop mario from flying away
			if(controller.collisions.below) {
				jumpHoldCount = 0;
				GameController.gameController.scoreMultiplier = 1;
			}
			if(controller.collisions.above || Input.GetButtonUp("Jump")) {
				jumpHoldCount = jumpHoldCountMax;
			}
			velocity.y += gravity * Time.deltaTime;

			// Changes moveSpeed

			if(Input.GetButton("Run")) {
				if(controller.collisions.below) {
					moveSpeed = moveSpeedSprint;
					jumpVelocityFraction = 16.8f;
				} else if(!controller.collisions.below && velocity.x > 4f) {
					moveSpeed = moveSpeedSprint;
					jumpVelocityFraction = 16.8f;
				}
			} else {
				moveSpeed = moveSpeedRun;
				jumpVelocityFraction = 18.4f;
			}

			// Movement speed calculation
			float targetVelocityX = input.x * moveSpeed;
			velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, controller.collisions.below ? accelerationTimeGrounded : accelerationTimeAirborne); // Dampens the movement so we don't get a sudden start and stop
			controller.move(velocity * Time.deltaTime);
			if(velocity.x < 1f && velocity.x > -1f && input.x == 0) { // To avoid "infinite fractions"
				velocity.x = 0f;
			}

// Fireball
			if(Input.GetKeyDown(KeyCode.B) && fireballAmount < 2 && GameController.gameController.health >= 3) {
				StartCoroutine("fire");
			}
			fireballAmount = GameObject.FindGameObjectsWithTag("Fireball").Length;
// Crouching
			if(Input.GetKey(KeyCode.S) && GameController.gameController.health >= 2) {
				boxCollider.size = new Vector2(boxCollider.size.x, 1.375f);
				boxCollider.offset = new Vector2(0, 0.1875f);
				crouching = true;
				moveSpeed = 0;
			} else {
				crouching = false;
				moveSpeed = moveSpeedRun;
			}
			anim.SetBool("Crouching", crouching);

			// Turning
			turning = ((velocity.x > 3 && input.x < 0) || (velocity.x < -3 && input.x > 0)) ? true : false;
			anim.SetBool("Turning", turning);
			anim.SetFloat("Speed", velocity.x); // Sends velocity.x to animator to get the correct animation.

			// FLIP!
			if(input.x > 0 && !facingRight && controller.collisions.below) {
				flip();
			} else if(input.x < 0 && facingRight && controller.collisions.below) {
				flip();
			}
		}
		// Accessing pipes
		if(transform.position.y == 6 && (transform.position.x > 57.2f && transform.position.x < 57.8f) && Input.GetKeyDown(KeyCode.S) && Application.loadedLevelName == "1-1") {
			canControl = false;
			pipe = true;
			anim.SetBool("PipeDown", pipe);
			GameController.gameController.pipeDown();
		}
		if(transform.position.x >= 12.1f && (transform.position.y >= 2f && transform.position.y < 2.1f) && (Input.GetAxisRaw("Horizontal") > 0) && Application.loadedLevelName == "1-1Underground") {
			transform.position = new Vector3(12.1f, 2f, 0);
			canControl = false;
			pipe = true;
			anim.SetBool("PipeRight", pipe);
			GameController.gameController.fromPipe = true;
			GameController.gameController.pipeUp();
		}

		// Finishing the Level
		if(transform.position.x >= 197.5f && !gameFinish) {
			gameFinish = true;
			transform.position = new Vector3(197.5f, transform.position.y, transform.position.z);
			StartCoroutine("poleFinish");
			print("poleFinish");
		}
	}

	void flip() {
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	IEnumerator fire() {
		shooting = true;
		anim.SetBool("Shooting", shooting);
		attatchedFireball = (GameObject) Instantiate(fireball, fireballSpawner.transform.position, Quaternion.identity);
		if(facingRight) {
			attatchedFireball.GetComponent<Fireball>().moveVelocity = 10;
		} else if(!facingRight) {
			attatchedFireball.GetComponent<Fireball>().moveVelocity = -10;
		}
		yield return new WaitForSeconds(0.5f);
		shooting = false;
		anim.SetBool("Shooting", shooting);
		attatchedFireball = null;
	}

	IEnumerator transformCoroutine() {
		transform.position = new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z);
		transforming = true;
		anim.SetBool("Transforming", transforming);
		if(Time.timeScale == 1) {
			yield return new WaitForSeconds(0.01f);
		}
		transforming = false;
		anim.SetBool("Transforming", transforming);
		Time.timeScale = 1;
		if(recentlyDamaged) {
			StartCoroutine("invulnerable");
		}
		yield return null;	
	}

	IEnumerator poleFinish() {
		velocity.x = 0;
		velocity.y = 0;
		anim.SetBool("Climbing", true);
		yield return new WaitForSeconds(1);
		anim.SetBool("Climbing", false);
		if(transform.position.y <= 3) {
			anim.SetBool("GameFinish", true);
		}
	}

	IEnumerator multiplierUp() {
		yield return new WaitForSeconds(0.1f);
		GameController.gameController.scoreMultiplier++;
	}

	IEnumerator invulnerable() {
		print("invulnerable start");
		float tempTime = 0;
		do {
			this.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = !this.gameObject.GetComponentInChildren<SpriteRenderer>().enabled;
			yield return new WaitForFixedUpdate();
			tempTime += Time.deltaTime;
		} while(tempTime < 3);
		if(tempTime > 3) {
			recentlyDamaged = false;
			this.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;
		}
		print("invulnerable end");
		yield return null;
	}
}