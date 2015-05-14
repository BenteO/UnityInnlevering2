using UnityEngine;
using System.Collections;

public class CameraControl: MonoBehaviour {

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
		min = BoundingBox.bounds.min;
		max = BoundingBox.bounds.max;
		IsFollowing = true;
	}

	public void Update() {
		var x = transform.position.x;
		var y = transform.position.y;

		if(IsFollowing) {
			min = BoundingBox.bounds.min;
			// Følger Mario
			if(Mathf.Abs(x - Mario.position.x) > Margin.x)
				x = Mathf.Lerp(x, Mario.position.x + 0.5f, Smoothing.x * Time.deltaTime);

			var cameraHalfWidth = mainCamera.orthographicSize * ((float) Screen.width / Screen.height);

			// Binder kamera fast til banen
			x = Mathf.Clamp(x, min.x + cameraHalfWidth, max.x - cameraHalfWidth);
			y = Mathf.Clamp(y, min.y + mainCamera.orthographicSize, max.y - mainCamera.orthographicSize);

			transform.position = new Vector3(x, y, transform.position.z);
		}
	}
}
