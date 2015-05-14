using UnityEngine;
using System.Collections;

// The object must have a BoxCollider2D to work,so it forces the object to have it
[RequireComponent(typeof(BoxCollider2D))]
public class InteractionController: MonoBehaviour {

	// Which layers the object collides with
	public LayerMask interactionMask;

	// To avoid the object from being stuck
	const float skinWidth = .016f;

	// How many horizontal and vertical lines are to be drawn (DrawRay)
	public int horizontalRayCount = 4;
	public int verticalRayCount = 4;
	// The distance between the DrawRay lines
	float horizontalRaySpacing;
	float verticalRaySpacing;

	// For creating bounds for the raycasting
	BoxCollider2D boxCollider;
	// Where the rays originate
	RaycastOrigins raycastOrigins;
	// Returns true or false based on where the collision is
	public collisionInfo collisions;
	// Returns the tag of the object we collided with
	public collisionTagInfo tagCollisions;

	void Start() {
		// Get stuff
		boxCollider = GetComponent<BoxCollider2D>();
		// Only needed once
		CalculateRaySpacing();
	}

	// For movement
	public void detect(Vector3 detectVector) {
		// Updates origin of raycast based on the objects velocity
		UpdateRaycastOrigins();
		// Sets all collisions to default
		collisions.reset();
		tagCollisions.reset();

		// If the velocity != 0 in one direction, the correct method is feeded a reference of the velocity parameter
		// "ref" allows the method to directly change the value of the parameter instead of changing the duplicated parameter
		if(detectVector.x != 0) {
			horizontalCollisions(ref detectVector);
		}
		if(detectVector.y != 0) {
			verticalCollisions(ref detectVector);
		}
	}

	// Checks if the object collides in the x-axis, and stops the object if it collides
	void horizontalCollisions(ref Vector3 velocity) {
		// Length of the ray
		float rayLength = Mathf.Abs(velocity.x) + skinWidth;
		// Loop to draw the horizontal lines
		for(int i = 0; i < horizontalRayCount; i++) {
			// Checks if it starts bottom left or bottom right
			Vector2 rayOriginLeft = raycastOrigins.bottomLeft;
			Vector2 rayOriginRight = raycastOrigins.bottomRight;
			// The y-position of the line
			rayOriginLeft += Vector2.up * (horizontalRaySpacing * i);
			rayOriginRight += Vector2.up * (horizontalRaySpacing * i);
			// Checks if it hits something on the interactionMask
			RaycastHit2D hitLeft = Physics2D.Raycast(rayOriginLeft, Vector2.right * -1, rayLength, interactionMask);
			RaycastHit2D hitRight = Physics2D.Raycast(rayOriginRight, Vector2.right * 1, rayLength, interactionMask);

			// Draws the rays. Ignored by camera, but visible in scene windows
			Debug.DrawRay(rayOriginLeft, Vector2.right * -1 * rayLength, Color.blue);
			Debug.DrawRay(rayOriginRight, Vector2.right * 1 * rayLength, Color.blue);

			if(hitLeft) {
				// Makes the object unable to move (velocity = 0)
				velocity.x = (hitLeft.distance - skinWidth) * -1;
				// changes the length of the rays
				rayLength = hitLeft.distance;
				// Returns true to left or right collision
				collisions.left = true;
				tagCollisions.left = hitLeft.collider.tag;
			}
			if(hitRight) {
				// Makes the object unable to move (velocity = 0)
				velocity.x = (hitLeft.distance - skinWidth) * 1;
				// changes the length of the rays
				rayLength = hitLeft.distance;
				// Returns true to left or right collision
				collisions.right = true;
				tagCollisions.right = hitRight.collider.tag;
			}

		}
	}

	// Same as over, but vertical
	void verticalCollisions(ref Vector3 velocity) {
		float rayLength = Mathf.Abs(velocity.y) + skinWidth;
		for(int i = 0; i < verticalRayCount; i++) {
			Vector2 rayOriginBelow = raycastOrigins.bottomLeft;
			Vector2 rayOriginAbove = raycastOrigins.topLeft;
			rayOriginBelow += Vector2.right * (verticalRaySpacing * i + velocity.x);
			rayOriginAbove += Vector2.right * (verticalRaySpacing * i + velocity.x);
			RaycastHit2D hitBelow = Physics2D.Raycast(rayOriginBelow, Vector2.up * -1, rayLength, interactionMask);
			RaycastHit2D hitAbove = Physics2D.Raycast(rayOriginAbove, Vector2.up * 1, rayLength, interactionMask);

			Debug.DrawRay(rayOriginBelow, Vector2.up * -1 * rayLength, Color.blue);
			Debug.DrawRay(rayOriginAbove, Vector2.up * 1 * rayLength, Color.blue);

			if(hitBelow) {
				velocity.y = (hitBelow.distance - skinWidth) * -1;
				rayLength = hitBelow.distance;

				collisions.below = true;
				tagCollisions.below = hitBelow.collider.tag;
			}
			if(hitAbove) {
				velocity.y = (hitAbove.distance - skinWidth) * 1;
				rayLength = hitAbove.distance;

				collisions.above = true;
				tagCollisions.above = hitAbove.collider.tag;
			}
		}
	}

	// Calculates origins of the raycast because the bounds are moving with the object
	void UpdateRaycastOrigins() {
		// Makes bounds for the origins
		Bounds bounds = boxCollider.bounds;
		// Makes the bounds slightly smaller
		bounds.Expand(skinWidth * -2);

		// Calculates origins of the raycast
		raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
	}

	// Spacing
	void CalculateRaySpacing() {
		// Same as above
		Bounds bounds = boxCollider.bounds;
		bounds.Expand(skinWidth * -2);

		// Limits the amount of rays between 2 and whatever 
		horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

		// Divides the rays evenly. RayCount - 1 because we want 3 spaces with 4 rays (| 1 | 2 | 3 |) not (| 1 | 2 | 3 | 4)
		horizontalRaySpacing = (bounds.size.y) / (horizontalRayCount - 1);
		verticalRaySpacing = (bounds.size.x) / (verticalRayCount - 1);
	}

	// Gives each corner a vector 2 to calculate the origins of the rays
	struct RaycastOrigins {
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}

	// Allows us to see where the object has collided
	public struct collisionInfo {
		public bool above, below;
		public bool left, right;

		public void reset() {
			above = below = false;
			left = right = false;
		}
	}

	// Allows us to see which tag collided with the ray
	public struct collisionTagInfo {
		public string above, below;
		public string left, right;

		public void reset() {
			above = below = null;
			left = right = null;
		}
	}
}
