using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class Mario: MonoBehaviour {

	// Jumping
	float jumpHeight = 4.4f;
	float timeToJumpApex = .5f;
	float gravity;
	float jumpVelocity;

	// Movement
	float accelerationTimeAirborne = 1f;
	float accelerationTimeGrounded = 0.3f;
	float moveSpeed = 5;
	float moveSpeedRun = 5;
	float moveSpeedSprint = 10;
	Vector3 velocity;
	float velocityXSmoothing;
	private bool facingRight = true;
	public bool turning = false;

	// Ingame Variables
	public bool invincible = false;
	public bool transforming = false;
	public bool shooting = false;
	public bool crouching = false;
	int fireballAmount = 0;
	bool gameFinish = false;

	// GameObject Components
	public Animator anim;
	Controller2D controller;
	public GameObject fireball;
	public GameObject fireballSpawner;
	GameObject attatchedFireball = null;
	BoxCollider2D boxCollider;

	// Collisions
	public bool hitUp;
	public bool hitDown;
	public bool hitLeft;
	public bool hitRight;
	public bool interaction;


	void Start() {
		boxCollider = GetComponent<BoxCollider2D>();
		controller = GetComponent<Controller2D>();
		// MATH!
		gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2); // s = v0 * t + 1/2 * a * t^2 -> a = 2s/t^2. Siden gravitasjonen fungerer mot positiv retning (opp) tar vi den negative verdien
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex; // v = a * t. Vi tar absoluttverdien av gravity for å alltid få positiv jumpVelocity
		anim.SetInteger("Health", GameController.health);
	}

	void Update() {
		// BoxCollider2D resize
		if(GameController.health == 1) {
			boxCollider.size = new Vector2(boxCollider.size.x, 1);
			boxCollider.offset = new Vector2(0, 0);
		} else if(GameController.health <= 2 && !crouching){
			boxCollider.size = new Vector2(boxCollider.size.x, 2);
			boxCollider.offset = new Vector2(0, 0.5f);
		}
		// Collisions 
		hitUp = controller.collisions.above;
		hitDown = controller.collisions.below;
		hitLeft = controller.collisions.left;
		hitRight = controller.collisions.right;
		interaction = controller.collisions.interaction;
		// Checks if Mario is touching the ground and sends bool to animator.
		anim.SetBool("isGrounded", hitDown);

		// To stop gravity from accumulating
		if(controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;
		}

		// Input
		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		if(Input.GetButtonDown("Jump") && controller.collisions.below) {
			velocity.y = jumpVelocity;
		}
		velocity.y += gravity * Time.deltaTime;

		moveSpeed= (Input.GetButton("Run"))? moveSpeedSprint : moveSpeedRun;

		// Movement speed calculation
		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, controller.collisions.below ? accelerationTimeGrounded : accelerationTimeAirborne); // Dampens the movement so we don't get a sudden start and stop
		controller.move(velocity * Time.deltaTime);
		if(velocity.x < 1f && velocity.x > -1f && input.x == 0) { // To avoid "infinite fractions"
			velocity.x = 0f;
		}

		// Crouching
		if(Input.GetKey(KeyCode.S) && GameController.health >= 2) {
			boxCollider.size = new Vector2(boxCollider.size.x, 1.375f);
			boxCollider.offset = new Vector2(0, 0.1875f);
			crouching = true;
			moveSpeed = 0;
		} else {
			crouching = false;
			moveSpeed = 5;
		}
		anim.SetBool("Crouching", crouching);

		turning = ((velocity.x > 4 && input.x < 0) || (velocity.x < -4 && input.x > 0)) ? true : false;
		anim.SetBool("Turning", turning);
		anim.SetFloat("Speed", velocity.x); // Sends velocity.x to animator to get the correct animation.

		// FLIP!
		if(input.x > 0 && !facingRight && controller.collisions.below) {
			flip();
		} else if(input.x < 0 && facingRight && controller.collisions.below) {
			flip();
		}

		if(transform.position.y < -1) {
			GameController.health = 0;
		}

		if(Input.GetKeyDown(KeyCode.B) && fireballAmount < 2 && GameController.health == 3) {
			StartCoroutine("fire");
		}

		fireballAmount = GameObject.FindGameObjectsWithTag("Fireball").Length;


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
		if(GameController.health < 3) {
			GameController.health++;
			anim.SetInteger("Health", GameController.health);
			transform.position = new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z);
			transforming = true;
			anim.SetBool("Transforming", transforming);
			if(Time.timeScale == 1) {
				yield return new WaitForSeconds(0.1f);
			}
			transforming = false;
			anim.SetBool("Transforming", transforming);
			Time.timeScale = 1;
		}
		yield return null;
		print("Transform finished");
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
}