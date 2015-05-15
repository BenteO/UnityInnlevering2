using UnityEngine;
using System.Collections;

public class MovingEnemy: MonoBehaviour {

	GoombaScript enemyScript;
	KoopaTrooperScript koopaTrooperScript;

	public float startSpeed = 0;
	bool speedSet = false;

	void Start() {
		if(GetComponentInParent<GoombaScript>()) {
			enemyScript = GetComponentInParent<GoombaScript>();
		}
		if(GetComponentInParent<KoopaTrooperScript>()) {
			koopaTrooperScript = GetComponentInParent<KoopaTrooperScript>();

		}
	}

	// If camera sees the object, give it speed (IF ANY VIEWPORT SEES THE OBJECT, IT GETS SPEED, SO ONLY GAME VIEW VISIBLE)
	void OnWillRenderObject() {
		if(!speedSet) {
			setSpeed();
			speedSet = true;
		}
	}

	public void setSpeed() {
		if(enemyScript != null) {
			enemyScript.moveSpeed = startSpeed;
		} else if(koopaTrooperScript != null) {
			koopaTrooperScript.moveSpeed = startSpeed;
		}
	}
}
