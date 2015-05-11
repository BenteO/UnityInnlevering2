using UnityEngine;
using System.Collections;

public class TimeFreeze: MonoBehaviour {

	Animator anim;

	void Start() {
		anim = GetComponent<Animator>();
	}

	public void timeStop() {
		if(Time.timeScale == 1) {
			anim.updateMode = AnimatorUpdateMode.UnscaledTime;
			Time.timeScale = 0;
		}
	}

	public void timeStart() {
		Time.timeScale = 1;
		anim.updateMode = AnimatorUpdateMode.Normal;
	}
}
