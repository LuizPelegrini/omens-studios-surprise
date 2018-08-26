﻿using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class Player : MonoBehaviour {

	[HideInInspector] public Vector2 velocity;
	[HideInInspector] public MovementController movementController;
	private float changingDirectionGrounded = .2f;
	private float changingDirectionOnAir = .1f;
	private bool _isFacingRight = true;
	private Number[] _numbersCollected;
	private int _numbersCollectedIndex = 0;

	[SerializeField] private float _horizontalSpeed = 10f;
	[SerializeField] private float _gravity = -20f;
	[SerializeField] private float _jumpForce;

	void Start()
	{
		movementController = GetComponent<MovementController>();
		_numbersCollected = new Number[2];
	}

	void Update()
	{
		// prevent accumulating gravity across multiple frames, if the player is colliding vertically
		if(movementController.collisionInfo.above || movementController.collisionInfo.below)
			velocity.y = 0f;

		// If the player presses the Jump button and there is a collision below it, so he can jump
		if(Input.GetKeyDown(KeyCode.Space) && movementController.collisionInfo.below)
			velocity.y = _jumpForce;

		// Apply gravity per-frame portion
		velocity.y += _gravity * Time.deltaTime;

		// Target horizontal input
		float targetVeloctityX = Input.GetAxisRaw("Horizontal") * _horizontalSpeed;

		float changingDirection = movementController.collisionInfo.below ? changingDirectionGrounded : changingDirectionOnAir;
		velocity.x = Mathf.Lerp(velocity.x, targetVeloctityX, changingDirection);

		CheckDirection();

		// Move the player by certain amount (based on velocity)
		movementController.Move(velocity * Time.deltaTime);
	}

	void CheckDirection()
	{
		bool movingRight = Mathf.Sign(velocity.x) >= 1 ? true : false;

		if(_isFacingRight && !movingRight)
		{
			transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
			_isFacingRight = !_isFacingRight;
		}

		if(!_isFacingRight && movingRight)
		{
			transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
			_isFacingRight = !_isFacingRight;
		}

	}

	public void CollectNumber(Number number)
	{
		if(_numbersCollected != null)
		{
			// Inventory full
			if(_numbersCollectedIndex == 2)
			{
				// Drop the number of the first position
				DropNumber(_numbersCollected[0]);
				_numbersCollected[0] = _numbersCollected[1];
				_numbersCollected[1] = number;
			}
			else
			{
				_numbersCollected[_numbersCollectedIndex++] = number;
			}

			print("Number: " + number.numberValue + " collected");
		}
	}

	public void EmptyInventory()
	{
		// Change index of the inventory
		_numbersCollectedIndex = 0;

		// Clear UI
	}

	private void DropNumber(Number numberToDrop)
	{
		print("Number " + numberToDrop.numberValue + " dropped");
		numberToDrop.Drop();
	}
}
