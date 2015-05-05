using UnityEngine;
using System.Collections;

public class QuestionBlockScript: MonoBehaviour {

	Animator anim = null;
	public Mario Mario;

	void Start() {
		anim = GetComponent<Animator>();
		Mario = GameObject.Find("Mario Parent").GetComponentInChildren<Mario>();
	}

	void Update() {
		anim.SetBool("isFalling", Mario.isFalling);
	}

	void OnCollisionEnter2D(Collision2D other) {
		anim.SetBool("MarioJumpUnder", true);
	}
	
	void OnCollisionExit2D(Collision2D other) {
		anim.SetBool("MarioJumpUnder", false);
	}
}
