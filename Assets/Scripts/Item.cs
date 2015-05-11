using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class Item: MonoBehaviour {

	Controller2D controller;
	Animator anim;
	Mario mario;

	public float moveSpeed = 0f;
	public float gravity = -3f;
	Vector3 velocity;
	bool facingRight = true;

	// Use this for initialization
	void Start() {
		controller = GetComponent<Controller2D>();
		anim = GetComponentInChildren<Animator>();
		mario = GameObject.Find("Mario Parent").GetComponent<Mario>();
		if(this.gameObject.tag == "TransformingItem") {
			anim.SetInteger("Health", mario.health);
		}
	}

	// Update is called once per frame
	void Update() {
		if(controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;
		}
		velocity.y += gravity * Time.deltaTime;
		if(controller.collisions.right) {
			facingRight = false;
		} else if(controller.collisions.left) {
			facingRight = true;
		}
		velocity.x = (facingRight ? moveSpeed : -moveSpeed) * Time.deltaTime;
		controller.move(velocity);

		if(transform.position.y < -1) {
			destroyItem();
		}

		if(controller.collisions.interaction && this.gameObject.tag == "TransformingItem") {
			print("item get");
			mario.StartCoroutine("transformCoroutine");
			destroyItem();
		}
	}

	public void destroyItem() {
		Destroy(this.gameObject);
	}
}
