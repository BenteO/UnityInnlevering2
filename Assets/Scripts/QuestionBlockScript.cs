using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxController))]
public class QuestionBlockScript: MonoBehaviour {

	Animator anim;
	private int marioHealth;
	bool activate = false;
	BoxController controller;
	public GameObject itemInBlock;
	public bool used = false;
	GameObject item = null;
	Vector3 thisPosition;

	Vector3 detectorLength = new Vector3(0, -0.2f, 0);

	void Start() {
		anim = GetComponent<Animator>();
		controller = GetComponent<BoxController>();
		thisPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
	}

	void Update() {
		controller.detector(detectorLength);
		if(Mario.hitUp && controller.collisions.below) {
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
			Destroy(GetComponent<UsedAnimationEvent>());
		}
	}
}
