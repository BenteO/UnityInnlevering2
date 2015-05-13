using UnityEngine;
using System.Collections;

public class MovingEnemy : MonoBehaviour {

	EnemyScript enemyScript;

	public float startSpeed = 0;
	bool speedSet = false;

	void Start () {
		enemyScript = GetComponentInParent<EnemyScript>();
	}

	// If camera sees the object, give it speed (IF ANY VIEWPORT SEES THE OBJECT, IT GETS SPEED, SO ONLY GAME VIEW VISIBLE)
	void OnWillRenderObject() {
		if(!speedSet) {
			setSpeed();
			speedSet = true;
		}
	}
	
	public void setSpeed() {
		enemyScript.moveSpeed = startSpeed;
	}
}
