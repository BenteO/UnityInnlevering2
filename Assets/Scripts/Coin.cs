﻿using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {

	Item item;
	void Start() {
		item = GetComponentInParent<Item>();
	}

	public void destroyThis() {
		item.destroyItem();
	}
}