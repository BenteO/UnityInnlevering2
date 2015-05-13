using UnityEngine;
using System.Collections;

public class UsedAnimationEvent: MonoBehaviour {

	QuestionBlockScript QBScript;
	BreakableBrickScript BBScript;
	InvicibleBlockScript IBScript;

	// Use this for initialization
	void Start() {
		if(GetComponentInParent<QuestionBlockScript>()) {
			QBScript = GetComponentInParent<QuestionBlockScript>();
		}
		if(GetComponentInParent<BreakableBrickScript>()) {
			BBScript = GetComponentInParent<BreakableBrickScript>();
		}
		if(GetComponentInParent<InvicibleBlockScript>()) {
			IBScript = GetComponentInParent<InvicibleBlockScript>();
		}
	}

	public void use() {
		if(QBScript != null) {
			QBScript.used = true;
		}
		if(BBScript != null) {
			BBScript.used = true;
		}
		if(IBScript != null) {
			IBScript.used = true;
		}
	}
}
