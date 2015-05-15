using UnityEngine;
using System.Collections;

public class destroyScript: MonoBehaviour {

	public GameObject ObjectToDestroy;
	bool setLayer = false;
	public string layerName;

	public void destroyThis() {
		Destroy(ObjectToDestroy);
	}

	public void disableBoxCollider() {
		ObjectToDestroy.GetComponent<BoxCollider2D>().enabled = !ObjectToDestroy.GetComponent<BoxCollider2D>().enabled;
	}

	public void changeLayer() {
		ObjectToDestroy.layer = 11; // Ignore-layer
	}

	public void changeTag(string tagName) {
		ObjectToDestroy.tag = tagName;
	}

	void OnWillRenderObject() {
		if(!setLayer) {
			this.gameObject.layer = LayerMask.NameToLayer(layerName);
			ObjectToDestroy.layer = LayerMask.NameToLayer(layerName);
			setLayer = true;
		}
	}
}
