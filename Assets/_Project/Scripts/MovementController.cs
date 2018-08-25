using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class MovementController : MonoBehaviour {

	// Util struct to place the origin of the rays
	public struct RaycastOrigins
	{
		public Vector2 bottomLeft, bottomRight;
		public Vector2 topLeft, topRight;
	}

	private float skinWidth = .015f;				// Width to shrink the bounds
	private BoxCollider2D _boxCollider2D;			// Reference to the boxcollider
	public RaycastOrigins raycastOrigins;
	private float horizontalSpacing;						// The space between the horizontal rays
	private float verticalSpacing;							// The space between the vertical rays
	private RaycastHit2D[] _hitResults;

	[SerializeField] private int horizontalRayCount = 2;	// How many rays cast, horizontally?
	[SerializeField] private int verticalRayCount = 2;		// How many rays cast, vertically?
	[SerializeField] private LayerMask _collisionLayerMask;

	void Start()
	{
		_hitResults = new RaycastHit2D[1];

		_boxCollider2D = GetComponent<BoxCollider2D>();

		// Calculate the space between rays
		CalculateRaycastSpacing();
	}

	/// <summary>
	/// Updates the origin of each ray, based on the bounds of the collider
	/// </summary>
	void UpdateRaycastOrigins()
	{
		// Shrink the bounds
		Bounds bounds = _boxCollider2D.bounds;
		bounds.Expand(-2 * skinWidth);

		raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);	
	}

	/// <summary>
	/// Calculate the spacing between rays in both directions (horizontal and vertical)
	/// <summary>
	void CalculateRaycastSpacing()
	{
		// Shrink the bounds
		Bounds bounds = _boxCollider2D.bounds;
		bounds.Expand(-2 * skinWidth);

		horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

		horizontalSpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalSpacing = bounds.size.x / (verticalRayCount - 1);
	}


	/// <summary>
	/// Checks for vertical collisions within the deltaMovement
	/// </summary>
	void VerticalCollisions(ref Vector2 deltaMovement)
	{
		// Sign of the direction of the movement in the y-axis
		float directionY = Mathf.Sign(deltaMovement.y);

		// the length of the movement
		float deltaMovementLength = Mathf.Abs(deltaMovement.y) + skinWidth;

		// For every ray...
		for(int i = 0; i < verticalRayCount; i++)
		{
			// If the player is moving downwards, the ray origin must start at the bottom left
			// Otherwise, if the player is moving upwards, the ray origin must start at the top left
			Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
			rayOrigin += Vector2.right * verticalSpacing * i;

			// Cast a ray from the origin of deltaMovementLength
			int results = Physics2D.RaycastNonAlloc(rayOrigin, directionY * Vector2.up, _hitResults, deltaMovementLength, _collisionLayerMask);
			Debug.DrawRay(rayOrigin, Vector2.up * deltaMovementLength * directionY, Color.green);

			// If the player collides with anything in the _collisionLayerMask
			if(results > 0)
			{
				// Take the distance of the collision
				float collisionDistance = _hitResults[0].distance;
				
				// Set as the new deltaMovement in the y-axis
				deltaMovement.y = (collisionDistance - skinWidth) * directionY;

				// To prevent the remaining rays from being cast as their length will be shortened by the distance of the collision
				deltaMovementLength = collisionDistance;
			}

		}
	}

	/// <summary>
	/// Checks for vertical collisions within the deltaMovement
	/// </summary>
	void HorizontalCollisions(ref Vector2 deltaMovement)
	{
		// Sign of the direction of the movement in the y-axis
		float directionX = Mathf.Sign(deltaMovement.x);

		// the length of the movement
		float deltaMovementLength = Mathf.Abs(deltaMovement.x) + skinWidth;

		// For every ray...
		for(int i = 0; i < horizontalRayCount; i++)
		{
			// If the player is moving downwards, the ray origin must start at the bottom left
			// Otherwise, if the player is moving upwards, the ray origin must start at the top left
			Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
			rayOrigin += Vector2.up * horizontalSpacing * i;

			// Cast a ray from the origin of deltaMovementLength
			int results = Physics2D.RaycastNonAlloc(rayOrigin, directionX * Vector2.right, _hitResults, deltaMovementLength, _collisionLayerMask);
			Debug.DrawRay(rayOrigin, Vector2.right * deltaMovementLength * directionX, Color.green);

			// If the player collides with anything in the _collisionLayerMask
			if(results > 0)
			{
				// Take the distance of the collision
				float collisionDistance = _hitResults[0].distance;
				
				// Set as the new deltaMovement in the y-axis
				deltaMovement.x = (collisionDistance - skinWidth) * directionX;

				// To prevent the remaining rays from being cast as their length will be shortened by the distance of the collision
				deltaMovementLength = collisionDistance;
			}

		}
	}

	/// <summary>
	/// Move the object by deltaMovement
	/// </summary>
	public void Move(Vector2 deltaMovement)
	{
		// Update the origin of the ray based on the bounds position
		UpdateRaycastOrigins();

		// Only check vertical collisions if the player is moving vertically
		if(deltaMovement.y != 0)
			VerticalCollisions(ref deltaMovement);

		// Only check horizontal collisions if the player is moving horizontally
		if(deltaMovement.x != 0)
			HorizontalCollisions(ref deltaMovement);

		// Move it
		transform.Translate(deltaMovement);
	}
}
