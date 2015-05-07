using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class Deathzone : MonoBehaviour {

	Vector3 movement = Vector3.zero;
	Controller2D controller;
	public GameObject mario;
	GameObject[] items;
	GameObject[] enemies;

	// Use this for initialization
	void Start () {
		controller = GetComponent<Controller2D>();
	}
	
	// Update is called once per frame
	void Update () {
		controller.move(movement);
		if(controller.collisions.above || controller.collisions.below || controller.collisions.left || controller.collisions.right) {
		
		}
	}
}
