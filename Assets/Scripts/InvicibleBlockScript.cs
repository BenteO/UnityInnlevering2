using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxController))]
public class InvicibleBlockScript: MonoBehaviour {

	Animator anim;
	Mario mario;
	private int marioHealth;
	bool activate = false;
	BoxController controller;
	public GameObject itemInBlock;
	public bool used = false;
	GameObject item = null;
	Vector3 thisPosition;
	SpriteRenderer spriteRenderer;

	Vector3 detectorLength = new Vector3(0, -0.2f, 0);

	void Start() {
		anim = GetComponentInChildren<Animator>();
		mario = GameObject.Find("Mario Parent").GetComponentInChildren<Mario>();
		controller = GetComponent<BoxController>();
		thisPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
	}

	void Update() {
		controller.detector(detectorLength);
		if(mario.interaction && controller.collisions.interaction) {
			activate = true;
			spriteRenderer.color = new Vector4(255, 255, 255, 255);
			controller.collisionMask = LayerMask.GetMask("Player", "Fireball", "Items");
			this.gameObject.layer = 0;
		} else {
			activate = false;
		}
		anim.SetBool("MarioJumpUnder", activate);
		if(used) {
			spriteRenderer.color = new Vector4(255, 255, 255, 255);
			controller.collisionMask = LayerMask.GetMask("Player", "Fireball", "Items");
			this.gameObject.layer = 0;
			item = (GameObject) Instantiate(itemInBlock, thisPosition, Quaternion.identity);
			used = false;
			Destroy(GetComponentInChildren<UsedAnimationEvent>());
		}
	}
}
