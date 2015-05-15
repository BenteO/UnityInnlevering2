using UnityEngine;
using System.Collections;

public class CastleFlag: MonoBehaviour {

	public Animator anim;

	// Update is called once per frame
	void Update() {
		anim.SetBool("RaiseFlag", GameController.gameController.raiseFlag);
	}
}
