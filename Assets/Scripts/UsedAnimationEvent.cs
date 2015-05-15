using UnityEngine;
using System.Collections;

public class UsedAnimationEvent: MonoBehaviour {

	QuestionBlockScript QBScript;
	BreakableBrickScript BBScript;
	InvicibleBlockScript IBScript;

	// Use this for initialization
	void Start() {
		if(GetComponent<QuestionBlockScript>()) {
			QBScript = GetComponent<QuestionBlockScript>();
		}
		if(GetComponent<BreakableBrickScript>()) {
			BBScript = GetComponent<BreakableBrickScript>();
		}
		if(GetComponent<InvicibleBlockScript>()) {
			IBScript = GetComponent<InvicibleBlockScript>();
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
