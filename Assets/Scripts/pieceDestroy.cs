using UnityEngine;
using System.Collections;

public class pieceDestroy: MonoBehaviour {

	// Destroy brick pieces when it out of bounds
	void Update() {
		if(transform.position.y < -2) {
			Destroy(this.gameObject);
		}
	}
}
