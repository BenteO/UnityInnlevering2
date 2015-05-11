using UnityEngine;
using System.Collections;

public class MovingItem: MonoBehaviour {

	Item item;
	void Start() {
		item = GetComponentInParent<Item>();
	}

	public void setMushroomSpeed() {
		item.moveSpeed = 3f;
		item.gravity = -1f;
	}

	public void setFireFlowerSpeed() {
		item.moveSpeed = 0f;
		item.gravity = -1f;
	}
}
