using UnityEngine;
using System.Collections;

public class Fireworks: MonoBehaviour {

	// Components
	public Animator fireworks1;
	public Animator fireworks2;
	public Animator fireworks3;


	void Update() {
		if(fireworks1 != null) {
			fireworks1.SetBool("Fireworks", GameController.gameController.fireworks1);
		}
		if(fireworks2 != null) {
			fireworks2.SetBool("Fireworks", GameController.gameController.fireworks2);
		}
		if(fireworks3 != null) {
			fireworks3.SetBool("Fireworks", GameController.gameController.fireworks3);
		}
	}
}
