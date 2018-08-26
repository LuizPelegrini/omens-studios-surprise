using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class Player : MonoBehaviour {

	[HideInInspector] public Vector2 velocity;
	[HideInInspector] public MovementController movementController;
	private float _changingDirectionGrounded = .2f;
	private float _changingDirectionOnAir = .1f;
	private bool _isFacingRight = true;
	[HideInInspector] public Number[] numbersCollected;
	private int _numbersCollectedIndex = 0;
	private float deadlyPositionY = -3.8f;

	[SerializeField] private float _horizontalSpeed = 10f;
	[SerializeField] private float _gravity = -20f;
	[SerializeField] private float _jumpForce;

	void Start()
	{
		movementController = GetComponent<MovementController>();
		numbersCollected = new Number[2];
	}

	void Update()
	{
		if(GameManager.Instance.gameOver)
			return;

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

		float changingDirection = movementController.collisionInfo.below ? _changingDirectionGrounded : _changingDirectionOnAir;
		velocity.x = Mathf.Lerp(velocity.x, targetVeloctityX, changingDirection);

		CheckDirection();

		// Move the player by certain amount (based on velocity)
		movementController.Move(velocity * Time.deltaTime);

		CheckPosition();
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
		if(numbersCollected != null)
		{
			// Inventory full
			if(_numbersCollectedIndex == 2)
			{
				// Drop the number of the first position
				DropNumber(numbersCollected[0]);
				numbersCollected[0] = numbersCollected[1];
				numbersCollected[1] = number;
			}
			else
			{
				numbersCollected[_numbersCollectedIndex++] = number;
			}


			UIController.Instance.ChangeInventory(this);

			print("Number: " + number.numberValue + " collected");
		}
	}

	public void EmptyInventory()
	{
		// Change index of the inventory
		_numbersCollectedIndex = 0;

		// Clear UI
		UIController.Instance.Clear();
	}

	private void DropNumber(Number numberToDrop)
	{
		print("Number " + numberToDrop.numberValue + " dropped");
		numberToDrop.Drop();
	}

	void CheckPosition()
	{
		if(transform.position.y < deadlyPositionY)
		{
			GameManager.Instance.gameOver = true;
		}
	}

	public void ChangePosition(Vector2 positionToSet)
	{
		transform.position = positionToSet;
	}
}
