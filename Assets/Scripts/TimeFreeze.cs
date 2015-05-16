using UnityEngine;
using System.Collections;

public class TimeFreeze: MonoBehaviour {

	Animator anim;

	void Start() {
		anim = GetComponent<Animator>();
	}
	// Stops time and sets the Animator to UnscaledTime to keep animating regardless of timeScale
	public void timeStop() {
		if(Time.timeScale == 1) {
			anim.updateMode = AnimatorUpdateMode.UnscaledTime;
			Time.timeScale = 0;
		}
	}

	// Starts time and sets Animator to Normal
	public void timeStart() {
		Time.timeScale = 1;
		anim.updateMode = AnimatorUpdateMode.Normal;
	}

	// Method if Mario dead animation runs
	public void Dead() {
		GameController.gameController.healthZero();
	}
}
