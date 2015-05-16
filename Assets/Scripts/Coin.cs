using UnityEngine;
using System.Collections;

public class Coin: MonoBehaviour {

	Item item;

	void Start() {
		item = GetComponentInParent<Item>();
	}
	// Method for animator
	public void destroyThis() {
		GameController.gameController.coins++;
		item.destroyItem();
	}
}
