using UnityEngine;
using System.Collections;

// The object must have a BoxCollider2D to work,so it forces the object to have it
[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D: MonoBehaviour {

	// Which layers the object collides with
	public LayerMask collisionMask;
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
	BoxCollider2D marioCollider;
	// Where the rays originate
	RaycastOrigins raycastOrigins;
	// Returns true or false based on where the collision is
	public collisionInfo collisions;

	void Start() {
		// Get stuff
		marioCollider = GetComponent<BoxCollider2D>();
		// Only needed once
		CalculateRaySpacing();
	}

	// For movement
	public void move(Vector3 velocity) {
		// Updates origin of raycast based on the objects velocity
		UpdateRaycastOrigins();
		// Sets all collisions to default
		collisions.reset();

		// If the velocity != 0 in one direction, the correct method is feeded a reference of the velocity parameter
		// "ref" allows the method to directly change the value of the parameter instead of changing the duplicated parameter
		if(velocity.x != 0) {
			horizontalCollisions(ref velocity);
		}
		if(velocity.y != 0) {
			verticalCollisions(ref velocity);
		}

		// Changes the position
		transform.Translate(velocity);
	}

	// Checks if the object collides in the x-axis, and stops the object if it collides
	void horizontalCollisions(ref Vector3 velocity) {
		// Mathf.Sign returns -1 or 1 if the velocity.x is negative or positive/0 respectively
		float directionX = Mathf.Sign(velocity.x);
		// Length of the ray
		float rayLength = Mathf.Abs(velocity.x) + skinWidth;
		// Loop to draw the horizontal lines
		for(int i = 0; i < horizontalRayCount; i++) {
			// Checks if it starts bottom left or bottom right
			Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
			// The y-position of the line
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);
			// Checks if it hits something on the collisionMask
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
			RaycastHit2D hitInteraction = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, interactionMask);
			// Draws the rays. Ignored by camera, but visible in scene windows
			Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

			if(hit) {
				// Makes the object unable to move (velocity = 0)
				velocity.x = (hit.distance - skinWidth) * directionX;
				// changes the length of the rays
				rayLength = hit.distance;

				// Returns true to left or right collision
				collisions.left = directionX == -1;
				collisions.right = directionX == 1;
			}
			// Interaction with objects
			if(hitInteraction) {
				collisions.interaction = true;
			}
		}
	}

	// Same as over, but vertical
	void verticalCollisions(ref Vector3 velocity) {
		float directionY = Mathf.Sign(velocity.y);
		float rayLength = Mathf.Abs(velocity.y) + skinWidth;
		for(int i = 0; i < verticalRayCount; i++) {
			Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
			RaycastHit2D hitInteraction = Physics2D.Raycast(rayOrigin, Vector2.right * directionY, rayLength, interactionMask);

			Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

			if(hit) {
				velocity.y = (hit.distance - skinWidth) * directionY;
				rayLength = hit.distance;

				collisions.below = directionY == -1;
				collisions.above = directionY == 1;
			}

			if(hitInteraction) {
				collisions.interaction = true;
			}
		}
	}

	// Calculates origins of the raycast because the bounds are moving with the object
	void UpdateRaycastOrigins() {
		// Makes bounds for the origins
		Bounds bounds = marioCollider.bounds;
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
		Bounds bounds = marioCollider.bounds;
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
		public bool interaction;

		public void reset() {
			above = below = false;
			left = right = false;
			interaction = false;
		}
	}
}
