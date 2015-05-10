using UnityEngine;
using System.Collections;

// Same as Controller2D, except the move method is changed to detector method and does not translate the position
// Extremely similar to Controller2D, but we dont want the rays to draw from the corners, but more towards the center so the collision feels more correct.
[RequireComponent(typeof(BoxCollider2D))]
public class BoxController: MonoBehaviour {

	public LayerMask collisionMask;
	public LayerMask interactionMask;

	const float skinWidth = .016f;
	public int horizontalRayCount = 4;
	public int verticalRayCount = 4;

	float horizontalRaySpacing;
	float verticalRaySpacing;

	BoxCollider2D blockCollider;
	RaycastOrigins raycastOrigins;
	public collisionInfo collisions;

	void Start() {
		blockCollider = GetComponent<BoxCollider2D>();
		CalculateRaySpacing();
	}

	public void detector(Vector3 testDistance) {
		UpdateRaycastOrigins();
		collisions.reset();

		if(testDistance.x != 0) {
			horizontalCollisions(ref testDistance);
		}
		if(testDistance.y != 0) {
			verticalCollisions(ref testDistance);
		}
	}

	void horizontalCollisions(ref Vector3 testDistance) {
		float directionX = Mathf.Sign(testDistance.x);
		float rayLength = Mathf.Abs(testDistance.x) + skinWidth;
		for(int i = 0; i < horizontalRayCount; i++) {
			Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
			RaycastHit2D hitInteraction = Physics2D.Raycast(rayOrigin, Vector2.up * directionX, rayLength, interactionMask);

			Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

			if(hit) {
				testDistance.x = (hit.distance - skinWidth) * directionX;
				rayLength = hit.distance;

				collisions.left = directionX == -1;
				collisions.right = directionX == 1;
			}
			if(hitInteraction) {
				collisions.interaction = true;
			}
		}
	}

	void verticalCollisions(ref Vector3 testDistance) {
		float directionY = Mathf.Sign(testDistance.y);
		float rayLength = Mathf.Abs(testDistance.y) + skinWidth;
		for(int i = 0; i < verticalRayCount; i++) {
			Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i + testDistance.x);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
			RaycastHit2D hitInteraction = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, interactionMask);

			Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

			if(hit) {
				testDistance.y = (hit.distance - skinWidth) * directionY;
				rayLength = hit.distance;

				collisions.below = directionY == -1;
				collisions.above = directionY == 1;
			}
			if(hitInteraction) {
				collisions.interaction = true;
			}
		}
	}


	void UpdateRaycastOrigins() {
		Bounds bounds = blockCollider.bounds;
		bounds.Expand(skinWidth * -2);

		raycastOrigins.bottomLeft = new Vector2(bounds.min.x + 0.2f, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2(bounds.max.x - 0.2f, bounds.min.y);
		raycastOrigins.topLeft = new Vector2(bounds.min.x + 0.2f, bounds.max.y);
		raycastOrigins.topRight = new Vector2(bounds.max.x - 0.2f, bounds.max.y);
	}

	void CalculateRaySpacing() {
		Bounds bounds = blockCollider.bounds;
		bounds.Expand(skinWidth * -2);

		horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

		horizontalRaySpacing = (bounds.size.y - 0.4f) / (horizontalRayCount - 1);
		verticalRaySpacing = (bounds.size.x - 0.4f) / (verticalRayCount - 1);
	}

	struct RaycastOrigins {
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}

	public struct collisionInfo {
		public bool above, below;
		public bool left, right;
		public bool interaction;

		public void reset() {
			above = below = false;
			left = right = false;
			interaction = false;
		}
	}
}
