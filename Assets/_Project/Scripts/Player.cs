using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class Player : MonoBehaviour {

	private float _gravity = -20f;
	private Vector2 _velocity;
	private MovementController _movementController;
	private float _speed = 10f;


	void Start()
	{
		_movementController = GetComponent<MovementController>();
	}

	void Update()
	{
		// Apply gravity per-frame portion
		_velocity.y += _gravity * Time.deltaTime;

		// Apply input
		_velocity.x = Input.GetAxisRaw("Horizontal") * _speed;

		// Move the player by certain amount (based on velocity)
		_movementController.Move(_velocity * Time.deltaTime);
	}
}
