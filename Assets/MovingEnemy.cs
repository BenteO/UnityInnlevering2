using UnityEngine;
using System.Collections;

public class MovingEnemy : MonoBehaviour {

	EnemyScript enemyScript;

	public float startSpeed = 0;

	void Start () {
		enemyScript = GetComponentInParent<EnemyScript>();
	}

	public void setSpeed() {
		enemyScript.moveSpeed = startSpeed;
	}
}
