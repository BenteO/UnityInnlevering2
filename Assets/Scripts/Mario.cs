using UnityEngine;
using System.Collections;

public class Mario : MonoBehaviour {
	public float maxSpeed = 8;
	//public float acceleration = 0.5f;
	public float jumpForce = 700f;

	private bool facingRight = true;

	Animator anim;

	void Start () {
		anim = GetComponent<Animator>();
	}

	void Update () {
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
		float h = Input.GetAxis("Horizontal");
		anim.SetFloat("lSpeed", h);
	}

}