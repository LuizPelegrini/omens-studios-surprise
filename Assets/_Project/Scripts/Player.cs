using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class Player : MonoBehaviour {

	[HideInInspector] public Vector2 velocity;
	[HideInInspector] public MovementController movementController;
	public float minPoxX, maxPosX;
	private Transform spriteTransform;
	private float _changingDirectionGrounded = .2f;
	private float _changingDirectionOnAir = .1f;
	private bool _isFacingRight = true;
	private AudioSource _audioSource;
	[HideInInspector] public Number[] numbersCollected;
	[HideInInspector] public int numbersCollectedIndex = 0;
	private float deadlyPositionY = -3.8f;

	[SerializeField] private float _horizontalSpeed = 10f;
	[SerializeField] private float _gravity = -20f;
	[SerializeField] private float _jumpForce;
	[SerializeField] private AudioClip _sfxNumberCollected;

	void Start()
	{
		movementController = GetComponent<MovementController>();
		_audioSource = GetComponent<AudioSource>();
		numbersCollected = new Number[2];

		spriteTransform = transform.GetChild(0);
	}

	void Update()
	{
		if(GameManager.Instance.gameOver)
			return;

		// prevent accumulating gravity across multiple frames, if the player is colliding vertically
		if(movementController.collisionInfo.above || movementController.collisionInfo.below)
			velocity.y = 0f;


		// If the player presses the Jump button and there is a collision below it, so he can jump
		if(Input.GetKeyDown(KeyCode.Space) && movementController.collisionInfo.below && !CameraFollow.shaking)
			velocity.y = _jumpForce;

		// Apply gravity per-frame portion
		velocity.y += _gravity * Time.deltaTime;

		// Target horizontal input
		float targetVeloctityX = Input.GetAxisRaw("Horizontal") * _horizontalSpeed;

		float changingDirection = movementController.collisionInfo.below ? _changingDirectionGrounded : _changingDirectionOnAir;
		velocity.x = Mathf.Lerp(velocity.x, targetVeloctityX, changingDirection);

		if(!CameraFollow.shaking)
			CheckDirection();

		// prevent from "climbing" walls, if the player is colliding horizontally
		if(movementController.collisionInfo.left || movementController.collisionInfo.right || CameraFollow.shaking)
			velocity.x = 0f;

		
		// Move the player by certain amount (based on velocity)
		movementController.Move(velocity * Time.deltaTime);

		CheckPosition();
	}

	void CheckDirection()
	{
		bool movingRight = Mathf.Sign(velocity.x) > 0 ? true : false;

		if(_isFacingRight && !movingRight)
		{
			spriteTransform.localScale = new Vector2(spriteTransform.localScale.x * -1, spriteTransform.localScale.y);
			_isFacingRight = !_isFacingRight;
		}

		if(!_isFacingRight && movingRight)
		{
			spriteTransform.localScale = new Vector2(spriteTransform.localScale.x * -1, spriteTransform.localScale.y);
			_isFacingRight = !_isFacingRight;
		}

	}

	public void CollectNumber(Number number)
	{
		if(numbersCollected != null)
		{
			_audioSource.PlayOneShot(_sfxNumberCollected);

			// Inventory full
			if(numbersCollectedIndex == 2)
			{
				// Drop the number of the first position
				DropNumber(numbersCollected[0]);
				numbersCollected[0] = numbersCollected[1];
				numbersCollected[1] = number;
			}
			else
			{
				numbersCollected[numbersCollectedIndex++] = number;
			}

			UIController.Instance.ChangeInventory(this);
		}
	}

	public void EmptyInventory()
	{
		// Change index of the inventory
		numbersCollectedIndex = 0;
		// Clear UI
		UIController.Instance.Clear();

		for (int i = 0; i < numbersCollected.Length; i++)
		{
			if(numbersCollected[i] != null)
			{
				DropNumber(numbersCollected[i]);
				numbersCollected[i] = null;
			}
		}
	}

	private void DropNumber(Number numberToDrop)
	{
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
