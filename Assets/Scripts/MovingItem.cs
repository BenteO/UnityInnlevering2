using UnityEngine;
using System.Collections;

public class MovingItem: MonoBehaviour {

	Item item;

	void Start() {
		item = GetComponentInParent<Item>();
	}

	// Mushroom
	public void setMushroomSpeed() {
		item.moveSpeed = 3f;
		item.gravity = -1f;
	}
	// Fire Flower
	public void setFireFlowerSpeed() {
		item.moveSpeed = 0f;
		item.gravity = -1f;
	}

	// Star
	public void setStarSpeed() {
		item.moveSpeed = 4f;
		item.gravity = -1f;
	}
}
