using UnityEngine;
using System.Collections;

public class Flag: MonoBehaviour {

	Animator anim;

	void Start() {
		anim = GetComponent<Animator>();
	}

	// Lowers the flag
	void Update() {
		anim.SetBool("GameFinish", GameController.gameController.gameFinish);
	}
}
