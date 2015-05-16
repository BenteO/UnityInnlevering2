using UnityEngine;
using System.Collections;

public class CameraControl: MonoBehaviour {

	// Components
	public Transform Mario;
	public Vector2 Margin, Smoothing;
	public BoxCollider2D BoundingBox;
	private Vector3 min, max;
	Camera mainCamera = null;

	public bool IsFollowing {
		get;
		set;
	}

	public void Start() {
		mainCamera = GetComponent<Camera>();
		// Binds the camera
		min = BoundingBox.bounds.min;
		max = BoundingBox.bounds.max;
		IsFollowing = true;
	}

	public void Update() {
		float x = transform.position.x;
		float y = transform.position.y;

		if(IsFollowing) {
			// Updates the minimum value to prevent going back
			min = BoundingBox.bounds.min;
			// Follows Mario
			if(Mathf.Abs(x - Mario.position.x) > Margin.x)
				x = Mathf.Lerp(x, Mario.position.x + 0.5f, Smoothing.x * Time.deltaTime);

			float cameraHalfWidth = mainCamera.orthographicSize * ((float) Screen.width / Screen.height);

			// Binds the camera to the level
			x = Mathf.Clamp(x, min.x + cameraHalfWidth, max.x - cameraHalfWidth);
			y = Mathf.Clamp(y, min.y + mainCamera.orthographicSize, max.y - mainCamera.orthographicSize);

			transform.position = new Vector3(x, y, transform.position.z);
		}
	}
}
