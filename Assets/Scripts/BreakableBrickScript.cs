using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxController))]
public class BreakableBrickScript: MonoBehaviour {

	Animator anim;
	Mario mario;
	private int marioHealth;
	bool activate = false;
	BoxController controller;
	public GameObject itemInBlock;
	public bool used = false;
	public int itemAmount = 1;
	GameObject item = null;
	Vector3 thisPosition;

	Vector3 detectorLength = new Vector3(0, -0.2f, 0);

	void Start() {
		anim = GetComponentInChildren<Animator>();
		mario = GameObject.Find("Mario Parent").GetComponentInChildren<Mario>();
		controller = GetComponent<BoxController>();
		thisPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
	}

	void Update() {
		marioHealth = mario.health;
		anim.SetInteger("Mario Health", marioHealth);
		anim.SetInteger("Coins", itemAmount);

		controller.detector(detectorLength);
		if(mario.hitUp && controller.collisions.below) {
			activate = true;
		} else {
			activate = false;
		}
		anim.SetBool("MarioJumpUnder", activate);
		if(used) {
			if(itemInBlock != null) {
				item = (GameObject) Instantiate(itemInBlock, thisPosition, Quaternion.identity);
			}
			used = false;
			itemAmount--;
			if(itemAmount <= 0) {
				Destroy(GetComponentInChildren<UsedAnimationEvent>());
			}
		}
	}
}
