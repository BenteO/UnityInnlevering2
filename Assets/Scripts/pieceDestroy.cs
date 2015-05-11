using UnityEngine;
using System.Collections;

public class pieceDestroy: MonoBehaviour {
	// Update is called once per frame
	void Update() {
		if(transform.position.y < -2) {
			Destroy(this.gameObject);
		}
	}
}
