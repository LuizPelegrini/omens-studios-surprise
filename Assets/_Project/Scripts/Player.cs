using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class Player : MonoBehaviour {

	private Vector2 _velocity;
	private MovementController _movementController;
	private float changingDirectionGrounded = .2f;
	private float changingDirectionOnAir = .1f;

	[SerializeField] private float _horizontalSpeed = 10f;
	[SerializeField] private float _gravity = -20f;
	[SerializeField] private float _jumpForce;

	void Start()
	{
		_movementController = GetComponent<MovementController>();
	}

	void Update()
	{
		// prevent accumulating gravity across multiple frames, if the player is colliding vertically
		if(_movementController.collisionInfo.above || _movementController.collisionInfo.below)
			_velocity.y = 0f;

		// If the player presses the Jump button and there is a collision below it, so he can jump
		if(Input.GetKeyDown(KeyCode.Space) && _movementController.collisionInfo.below)
			_velocity.y = _jumpForce;

		// Apply gravity per-frame portion
		_velocity.y += _gravity * Time.deltaTime;

		// Target horizontal input
		float targetVeloctityX = Input.GetAxisRaw("Horizontal") * _horizontalSpeed;

		float changingDirection = _movementController.collisionInfo.below ? changingDirectionGrounded : changingDirectionOnAir;
		_velocity.x = Mathf.Lerp(_velocity.x, targetVeloctityX, changingDirection);

		// Move the player by certain amount (based on velocity)
		_movementController.Move(_velocity * Time.deltaTime);
	}
}
