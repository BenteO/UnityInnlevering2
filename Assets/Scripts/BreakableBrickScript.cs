﻿using UnityEngine;
using System.Collections;

public class BreakableBrickScript: MonoBehaviour {

	Animator anim = null;
	public Mario Mario;
	private int MarioHealth;

	void Start() {
		anim = GetComponent<Animator>();
		Mario = GameObject.Find("Mario").GetComponent<Mario>();
	}

	void Update() {
		anim.SetBool("isFalling", Mario.isFalling);
		anim.SetInteger("Mario Health", Mario.Health);
	}

	void OnCollisionEnter2D(Collision2D other) {
		anim.SetBool("MarioJumpUnder", true);
	}

	void OnCollisionExit2D(Collision2D other) {
		anim.SetBool("MarioJumpUnder", false);
	}
}