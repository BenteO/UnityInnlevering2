using UnityEngine;
using System.Collections;

public class destroyScript: MonoBehaviour {

	// Components
	public GameObject ObjectToDestroy;
	public string layerName;

	// Variables
	bool setLayer = false;

	// Method to destroy the object
	public void destroyThis() {
		Destroy(ObjectToDestroy);
	}

	// Method to disable BoxCollider2D
	public void disableBoxCollider() {
		ObjectToDestroy.GetComponent<BoxCollider2D>().enabled = !ObjectToDestroy.GetComponent<BoxCollider2D>().enabled;
	}

	// Method to change layer
	public void changeLayer() {
		ObjectToDestroy.layer = 11; // Ignore-layer
		ObjectToDestroy.GetComponent<Controller2D>().collisionMask = LayerMask.GetMask("Default", "Ground");
	}

	// Method to change tag
	public void changeTag(string tagName) {
		ObjectToDestroy.tag = tagName;
	}

	// Method to change layer when the object is visible by the camera (including viewport)
	void OnWillRenderObject() {
		if(!setLayer) {
			this.gameObject.layer = LayerMask.NameToLayer(layerName);
			ObjectToDestroy.layer = LayerMask.NameToLayer(layerName);
			setLayer = true;
		}
	}
}
