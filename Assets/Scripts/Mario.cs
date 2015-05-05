using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class Mario: MonoBehaviour {

	// Jumping
	float jumpHeight = 4.2f;
	float timeToJumpApex = .5f;
	float gravity;
	float jumpVelocity;
	public bool isFalling = false;

	// Movement
	float accelerationTimeAirborne = 0.7f;
	float accelerationTimeGrounded = 0.5f;
	float moveSpeed = 5;
	float moveSpeedRun = 5;
	float moveSpeedSprint = 10;
	Vector3 velocity;
	float velocityXSmoothing;
	private bool facingRight = true;
	public bool turning = false;



	// Ingame Variables
	public int health = 1;
	public int lives = 1;
	public bool invincible = false;
	public bool transforming = false;
	public bool shooting = false;
	public bool crouching = false;
	int fireballAmount = 0;

	// GameObject Components
	public Animator anim;
	Controller2D controller;
	public GameObject fireball;
	public GameObject fireballSpawner;
	GameObject attatchedFireball = null;


	void Start() {
		controller = GetComponent<Controller2D>();
		// MATH!
		gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2); // s = v0 * t + 1/2 * a * t^2 -> a = 2s/t^2. Siden gravitasjonen fungerer mot positiv retning (opp) tar vi den negative verdien
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex; // v = a * t. Vi tar absoluttverdien av gravity for å alltid få positiv jumpVelocity
	}

	void Update() {
		// Checks if Mario is touching the ground and sends bool to animator.
		bool isGrounded = (controller.collisions.below) ? true : false;
		anim.SetBool("isGrounded", isGrounded);

		// To stop gravity from accumulating
		if(controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;
		}

		// Input
		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		if(Input.GetButtonDown("Jump") && controller.collisions.below) {
			velocity.y = jumpVelocity;
		}

		if(Input.GetButton("Run")) {
			moveSpeed = moveSpeedSprint;
		} else {
			moveSpeed = moveSpeedRun;
		}

		// Movement speed calculation
		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne); // Dampens the movement so we don't get a sudden start and stop
		velocity.y += gravity * Time.deltaTime;
		controller.move(velocity * Time.deltaTime);
		if(velocity.x < 1f && velocity.x > -1f && input.x == 0) { // To avoid "infinite fractions"
			velocity.x = 0f;
		}
		if((velocity.x > 4 && input.x < 0) || (velocity.x < -4 && input.x > 0)) {
			turning = true;
		} else {
			turning = false;
		}
		anim.SetBool("Turning", turning);
		anim.SetFloat("Speed", velocity.x); // Sends velocity.x to animator to get the correct animation.

		// FLIP!
		if(input.x > 0 && !facingRight) {
			flip();
		} else if(input.x < 0 && facingRight) {
			flip();
		}

		if(health < 1)
			die();

		if(Input.GetKeyDown(KeyCode.B) && fireballAmount < 2 && health == 3) {
			StartCoroutine("fire");
		}
		fireballAmount = GameObject.FindGameObjectsWithTag("Fireball").Length;
	}

	void flip() {
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void die() {
		lives--;
		if(lives < 0) {
		}
			//endgame;
		else {
		}
		//respawn;
	}

	IEnumerator fire() {
		shooting = true;
		attatchedFireball = (GameObject) Instantiate(fireball, fireballSpawner.transform.position, Quaternion.identity);
		if(facingRight) {
			attatchedFireball.GetComponent<Fireball>().moveVelocity = 10;
		} else if(!facingRight) {
			attatchedFireball.GetComponent<Fireball>().moveVelocity = -10;
		}
		yield return new WaitForSeconds(1f);
		shooting = false;
		attatchedFireball = null;
	}

	void OnCollisionEnter2D(Collision2D other) {
		if(other.gameObject.tag == "Question Block" || other.gameObject.tag == "Breakable Brick") {
		}

		if(other.gameObject.tag == "Enemy") {
			if(!invincible) //midlertidig
				health--;
			DestroyObject(other.gameObject);
		}

		if(other.gameObject.tag == "Star") {
			invincible = true;
			DestroyObject(other.gameObject);
		}
		if(other.gameObject.tag == "Magic") {
			health = 2;
			DestroyObject(other.gameObject);
		}
		if(other.gameObject.tag == "Green") {
			lives++;
			DestroyObject(other.gameObject);
		}
		if(other.gameObject.tag == "Fire") {
			health = 3;
			DestroyObject(other.gameObject);
		}
	}
}