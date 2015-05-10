using UnityEngine;
using System.Collections;

public class MovingItem: MonoBehaviour {

	Item item;
	void Start() {
		item = GetComponentInParent<Item>();
	}

	public void getSpeed() {
		item.moveSpeed = 3f;
		item.gravity = -1f;
	}
}
