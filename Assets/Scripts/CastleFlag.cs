using UnityEngine;
using System.Collections;

public class CastleFlag: MonoBehaviour {

	public Animator anim;

	// Raise the flag
	void Update() {
		anim.SetBool("RaiseFlag", GameController.gameController.raiseFlag);
	}
}
