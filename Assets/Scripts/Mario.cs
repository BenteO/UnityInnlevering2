using UnityEngine;
using System.Collections;

public class Mario : MonoBehaviour {
	// Movement
	public float maxSpeed = 6;
	//public float acceleration = 0.5f;
	
	// Jumping
	public float jumpSpeed = 0.5f;
	private int jumpCount = 0;
	private int jumpCountMax = 25;
	public bool isFalling = false;

	private bool facingRight = true;

	private Animator anim;
	private Transform GroundCheck;
	public LayerMask GroundLayers;
	public int Health = 1;

	void Start () {
		anim = GetComponent<Animator>();
		GroundCheck = transform.FindChild("GroundCheck");
	}

	void Update () {
		bool isGrounded = Physics2D.OverlapPoint(GroundCheck.position, GroundLayers);
		isFalling = (GetComponent<Rigidbody2D>().velocity.y < -0.1f);

		if (Input.GetButtonDown("Jump") && isGrounded) {
			isGrounded = false;
			GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
		}
		if(Input.GetButton("Jump") && !isGrounded && jumpCount < jumpCountMax) {
			GetComponent<Rigidbody2D>().AddForce(Vector2.up * 0.5f, ForceMode2D.Impulse);
			jumpCount++;
		}
		if(Input.GetButtonUp("Jump") && !isGrounded && jumpCount < jumpCountMax) {
			jumpCount = jumpCountMax;
		}
		if(isGrounded) {
			maxSpeed = 6;
			jumpCount = 0;
		}
		anim.SetBool("isGrounded", isGrounded);

		float h = Input.GetAxis("Horizontal");
		anim.SetFloat("Speed", h);
		transform.Translate(h * maxSpeed * Time.deltaTime, 0, 0);

		if(h > 0 && !facingRight) {
			Flip();
		} else if(h < 0 && facingRight) {
			Flip();
		}
	}

	void Flip () {
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void OnCollisionEnter2D(Collision2D other) {
		if(other.gameObject.tag == "Question Block" || other.gameObject.tag == "Breakable Brick") {
			jumpCount = jumpCountMax;
		}
	}
}