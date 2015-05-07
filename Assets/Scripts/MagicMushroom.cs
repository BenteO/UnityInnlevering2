using UnityEngine;
using System.Collections;

public class MagicMushroom: MonoBehaviour {

	Item item;
	void Start() {
		item = GetComponentInParent<Item>();
	}

	public void getSpeed() {
		item.moveSpeed = 3f;
	}
}
