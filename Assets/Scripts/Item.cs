using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D), typeof(InteractionController))]
public class Item: MonoBehaviour {

	Controller2D controller;
	InteractionController interaction;
	Animator anim;
	Mario mario;

	public float moveSpeed = 0f;
	public float gravity = -3f;
	Vector3 velocity;
	bool facingRight = true;

	// Use this for initialization
	void Start() {
		controller = GetComponent<Controller2D>();
		interaction = GetComponent<InteractionController>();
		anim = GetComponentInChildren<Animator>();
		mario = GameObject.Find("Mario Parent").GetComponent<Mario>();
		if(this.gameObject.tag == "TransformingItem") {
			anim.SetInteger("Health", GameController.gameController.health);
		}
	}

	// Update is called once per frame
	void Update() {
		if(controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;
		}

		if(controller.collisions.below && this.gameObject.tag == "Star") {
			velocity.y = 0.33f;
		}

		velocity.y += gravity * Time.deltaTime;
		if(controller.collisions.right) {
			facingRight = false;
		} else if(controller.collisions.left) {
			facingRight = true;
		}
		velocity.x = (facingRight ? moveSpeed : -moveSpeed) * Time.deltaTime;
		controller.move(velocity);

		interaction.detect(new Vector3(0.01f, 0.01f, 0));

		if(transform.position.y < -1 || (transform.position.x - Camera.main.transform.position.x > 15) || (transform.position.x - Camera.main.transform.position.x < -15)) {
			//destroyItem();
		}
		// If static coin
		if((interaction.collisions.above || interaction.collisions.below || interaction.collisions.left || interaction.collisions.right) && this.gameObject.tag == "Coin") {
			GainPoints.increaseScoreStatic(200);
			GameController.gameController.coins++;
			destroyItem();
		}
		// If transform item
		if((interaction.collisions.above || interaction.collisions.below || interaction.collisions.left || interaction.collisions.right) && this.gameObject.tag == "TransformingItem") {
			if(GameController.gameController.health < 3) {
				GameController.gameController.health++;
			}
			mario.StartCoroutine("transformCoroutine");
			GainPoints.increaseScoreStatic(1000);
			destroyItem();
		}
		// If oneUp mushroom
		if((interaction.collisions.above || interaction.collisions.below || interaction.collisions.left || interaction.collisions.right) && this.gameObject.tag == "OneUp") {
			GameController.gameController.lives++;
			destroyItem();
		}

		// If star
		if((interaction.collisions.above || interaction.collisions.below || interaction.collisions.left || interaction.collisions.right) && this.gameObject.tag == "Star") {
			GameController.gameController.star = true;
			GameController.gameController.StartCoroutine("marioInvincible");
			destroyItem();
		}
	}

	public void destroyItem() {
		Destroy(this.gameObject);
	}
}
