using UnityEngine;
using System.Collections;

public class Mario : MonoBehaviour {
	public float maxSpeed = 8;
	//public float acceleration = 0.5f;
	public float jumpSpeed = 0.5f;

	private bool facingRight = true;

	private Animator anim;
	private Transform GroundCheck;
	public LayerMask GroundLayers;

	void Start () {
		anim = GetComponent<Animator>();
		GroundCheck = transform.FindChild("GroundCheck");
	}

	void Update () {
		bool isGrounded = Physics2D.OverlapPoint(GroundCheck.position, GroundLayers);

		if (Input.GetButton("Jump")) {

			if(isGrounded) {
				Debug.Log("Jump!");
				GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
				isGrounded = false;
			}
		}

		anim.SetBool("isGrounded", isGrounded);

		float h = Input.GetAxis("Horizontal");
		anim.SetFloat("Speed", h);
		transform.Translate (h * maxSpeed * Time.deltaTime, 0, 0);

		if (h > 0 && !facingRight) Flip();
		else if (h < 0 && facingRight) Flip();
	}

	void Flip () {
		Debug.Log("Flip");
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

}