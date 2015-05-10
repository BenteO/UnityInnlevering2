using UnityEngine;
using System.Collections;

public class TimeFreeze: MonoBehaviour {

	Animator anim;

	void Start() {
		anim = GetComponent<Animator>();
	}

	public void timeStop() {
		if(Time.timeScale == 0) {
			Time.timeScale = 1;
			anim.updateMode = AnimatorUpdateMode.Normal;
		} else if(Time.timeScale == 1) {
			Time.timeScale = 0;
			anim.updateMode = AnimatorUpdateMode.UnscaledTime;
		}
	}
}
