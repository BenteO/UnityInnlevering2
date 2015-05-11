using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class EnemyScript: MonoBehaviour {

	public float moveSpeed;
	public float gravity;

	Controller2D controller;
	Vector3 enemySpeed;

	// Use this for initialization
	void Start() {
		controller = GetComponent<Controller2D>();
	}

	// Update is called once per frame
	void Update() {
		if(controller.collisions.below) {
			enemySpeed.y = 0;
		}
		if(controller.collisions.right) {
			moveSpeed = -Mathf.Abs(moveSpeed);
		} else if(controller.collisions.left) {
			moveSpeed = Mathf.Abs(moveSpeed);
		}

		enemySpeed.x = moveSpeed * Time.deltaTime;
		enemySpeed.y = gravity * Time.deltaTime;
		controller.move(enemySpeed);
	}
}
