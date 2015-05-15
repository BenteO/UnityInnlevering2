using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D), typeof(InteractionController))]
public class KoopaTrooperScript: MonoBehaviour {

	public float moveSpeed = 0;
	public float gravity;

	Vector3 enemySpeed;
	Vector3 interactionVector = new Vector3(0.01f, 0.01f, 0);
	public bool inShell = false;
	bool dead = false;

	Controller2D controller;
	InteractionController interaction;
	Animator anim;
	public GameObject thisChild;

	void Start() {
		controller = GetComponent<Controller2D>();
		interaction = GetComponent<InteractionController>();
		anim = GetComponentInChildren<Animator>();
	}

	void Update() {
		if(inShell && gameObject.tag == "KoopaTrooperStatic" && !dead) {
			if(interaction.collisions.left) {
				moveSpeed = 10;
				anim.Play("ShellMoving");
				controller.collisionMask = LayerMask.GetMask("Default", "Ground");
				StartCoroutine("waitABit");
				dead = true;
			} else if(interaction.collisions.right) {
				moveSpeed = -10;
				anim.Play("ShellMoving");
				controller.collisionMask = LayerMask.GetMask("Default", "Ground");
				StartCoroutine("waitABit");
				dead = true;
			}
		}
		// Movement
		if(controller.collisions.below || controller.collisions.above) {
			enemySpeed.y = 0;
		}
		if(controller.collisions.right || controller.collisions.left) {
			moveSpeed = -moveSpeed;
			Vector3 thisScale = transform.localScale;
			thisScale.x *= -1;
			transform.localScale = thisScale;
		}

		enemySpeed.x = moveSpeed * Time.deltaTime;
		enemySpeed.y = gravity * Time.deltaTime;
		controller.move(enemySpeed);

		// Interaction
		interaction.detect(interactionVector);

		// Die
		if(GameController.gameController.star) {
			if(!dead) {
				if((interaction.collisions.above || interaction.collisions.left) && (interaction.tagCollisions.above == "Player" || interaction.tagCollisions.left == "Player")) {
					moveSpeed = 0;
					anim.Play("DieFireRight");
					GainPoints.gainPoints.PointPrefabSpawn = this.transform.position;
					GainPoints.gainPoints.increaseScoreMultiplier(200);
					dead = true;
				} else if((interaction.collisions.below || interaction.collisions.right) && (interaction.tagCollisions.below == "Player" || interaction.tagCollisions.right == "Player")) {
					moveSpeed = 0;
					anim.Play("DieFireLeft");
					GainPoints.gainPoints.PointPrefabSpawn = this.transform.position;
					GainPoints.gainPoints.increaseScoreMultiplier(200);
					dead = true;
				}
			}
		}

		if(interaction.collisions.above && interaction.tagCollisions.above == "Player" && !interaction.collisions.below && !inShell) {
			moveSpeed = 0;
			inShell = true;
			anim.Play("ShellStatic");
			GainPoints.gainPoints.PointPrefabSpawn = this.transform.position;
			GainPoints.gainPoints.increaseScoreMultiplier(100);
		}
		if(transform.position.y < -1f || (((transform.position.x - Camera.main.transform.position.x) > 15 || (transform.position.x - Camera.main.transform.position.x) < -15) && this.gameObject.layer == LayerMask.NameToLayer("KoopaTrooperShellMoving"))) {
			Destroy(this.gameObject);
		}
		if(!dead) {
			if(interaction.tagCollisions.above == "Fireball" || interaction.tagCollisions.left == "Fireball") {
				moveSpeed = 0;
				anim.Play("DieFireRight");
				GainPoints.gainPoints.PointPrefabSpawn = this.transform.position;
				GainPoints.gainPoints.increaseScoreMultiplier(200);
				dead = true;
			} else if(interaction.tagCollisions.below == "Fireball" || interaction.tagCollisions.right == "Fireball") {
				moveSpeed = 0;
				anim.Play("DieFireLeft");
				GainPoints.gainPoints.PointPrefabSpawn = this.transform.position;
				GainPoints.gainPoints.increaseScoreMultiplier(200);
				dead = true;
			}
		}
	}

	IEnumerator waitABit() {
		yield return new WaitForSeconds(0.02f);
		gameObject.tag = "KoopaTrooperShellMoving";
		thisChild.tag = "KoopaTrooperShellMoving";
		gameObject.layer = LayerMask.NameToLayer("KoopaTrooperShellMoving");
		thisChild.layer = LayerMask.NameToLayer("KoopaTrooperShellMoving");
		GainPoints.gainPoints.PointPrefabSpawn = this.transform.position;
		GainPoints.gainPoints.increaseScoreFixed(400);
	}
}