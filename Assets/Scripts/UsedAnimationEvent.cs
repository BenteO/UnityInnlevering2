using UnityEngine;
using System.Collections;

public class UsedAnimationEvent: MonoBehaviour {
	
	// Components
	QuestionBlockScript QBScript;
	BreakableBrickScript BBScript;
	InvicibleBlockScript IBScript;

	// Gets script that exists
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

	// Sets bool to script that exists
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
