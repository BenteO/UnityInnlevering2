using UnityEngine;
using System.Collections;

public class destroyScript: MonoBehaviour {

	public GameObject ObjectToDestroy;

	public void destroyThis() {
		Destroy(ObjectToDestroy);
	}

	public void disableBoxCollider() {
		ObjectToDestroy.GetComponent<BoxCollider2D>().enabled = !ObjectToDestroy.GetComponent<BoxCollider2D>().enabled;
	}
}
