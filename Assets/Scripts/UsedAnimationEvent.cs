using UnityEngine;
using System.Collections;

public class UsedAnimationEvent: MonoBehaviour {

	QuestionBlockScript QBScript;
	BreakableBrickScript BBScript;

	// Use this for initialization
	void Start() {
		if(GetComponentInParent<QuestionBlockScript>()) {
			QBScript = GetComponentInParent<QuestionBlockScript>();
		}
		if(GetComponentInParent<BreakableBrickScript>()) {
			BBScript = GetComponentInParent<BreakableBrickScript>();
		}
	}

	public void use() {
		if(QBScript != null) {
			QBScript.used = true;
		}
		if(BBScript != null) {
			BBScript.used = true;
		}
	}
}
