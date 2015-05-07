using UnityEngine;
using System.Collections;

public class QuestionBlockAnimationEvent : MonoBehaviour {

	QuestionBlockScript QBScript;

	// Use this for initialization
	void Start () {
		QBScript = GetComponentInParent<QuestionBlockScript>();
	}

	public void use() {
		QBScript.used = true;
	}
}
