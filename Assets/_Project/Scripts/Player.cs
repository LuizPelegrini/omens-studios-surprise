﻿using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class Player : MonoBehaviour {

	[HideInInspector] public Vector2 velocity;
	[HideInInspector] public MovementController movementController;
	public float minPoxX, maxPosX;
	private Transform spriteTransform;
	private float _changingDirectionGrounded = .2f;
	private float _changingDirectionOnAir = .1f;
	private bool _isFacingRight = true;
	[HideInInspector] public Number[] numbersCollected;
	[HideInInspector] public int numbersCollectedIndex = 0;
	private float deadlyPositionY = -3.8f;
	private bool _isFootstepPlaying = false;
	private bool _isDoorPlaying = false;

	[SerializeField] private AudioSource _audioSource;
	[SerializeField] private AudioSource _footStepsAudioSource;
	[SerializeField] private float _horizontalSpeed = 10f;
	[SerializeField] private float _gravity = -20f;
	[SerializeField] private float _jumpForce;
	
	[Space]
	[Header("SFXs")]
	[SerializeField] private AudioClip _sfxNumberCollected;
	[SerializeField] private AudioClip _sfxMathDoorOpening;
	[SerializeField] private AudioClip _sfxJump;
	[SerializeField] private AudioClip _footStepsSFX;
	[SerializeField] private AudioClip _levelCompleteSFX;
	[SerializeField] private AudioClip _errorSoundSFX;

	void Start()
	{
		movementController = GetComponent<MovementController>();
		numbersCollected = new Number[2];

		spriteTransform = transform.GetChild(0);
	}

	void Update()
	{
		if(GameManager.Instance.gamePaused)
			StopPlayingFootstepsSFX();

		if(GameManager.Instance.gamePaused && _isDoorPlaying)
			_audioSource.Pause();

		if(!GameManager.Instance.gamePaused && _isDoorPlaying)
			_audioSource.UnPause();


		if(GameManager.Instance.gameOver || GameManager.Instance.gameCompleted || GameManager.Instance.gamePaused)
			return;

		// prevent accumulating gravity across multiple frames, if the player is colliding vertically
		if(movementController.collisionInfo.above || movementController.collisionInfo.below)
			velocity.y = 0f;


		// If the player presses the Jump button and there is a collision below it, so he can jump
		if(Input.GetKeyDown(KeyCode.Space) && movementController.collisionInfo.below && !CameraFollow.shaking)
		{
			velocity.y = _jumpForce;
			_audioSource.PlayOneShot(_sfxJump, .2f);
		}

		// Apply gravity per-frame portion
		velocity.y += _gravity * Time.deltaTime;

		// Target horizontal input
		float targetVeloctityX = Input.GetAxisRaw("Horizontal") * _horizontalSpeed;

		float changingDirection = movementController.collisionInfo.below ? _changingDirectionGrounded : _changingDirectionOnAir;
		velocity.x = Mathf.Lerp(velocity.x, targetVeloctityX, changingDirection);

		if(Mathf.Abs(velocity.x) > .2f && movementController.collisionInfo.below && !CameraFollow.shaking)
			PlayFootstepsSFX();
		else
			StopPlayingFootstepsSFX();

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

	public Vector2 CollectNumber(Number number)
	{
		Vector2 operandPos = Vector2.zero;

		if(numbersCollected != null)
		{
			_audioSource.PlayOneShot(_sfxNumberCollected, .3f);

			if(numbersCollectedIndex == 0)
				operandPos = UIController.Instance.operandA.position;
			else
				operandPos = UIController.Instance.operandB.position;
			
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
		}

		return operandPos;
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

	public void PlayDestroyingDoor()
	{
		_isDoorPlaying = true;
		_audioSource.PlayOneShot(_sfxMathDoorOpening, 1f);
	}

	private void PlayFootstepsSFX()
	{
		if(!_isFootstepPlaying)
		{
			_footStepsAudioSource.clip = _footStepsSFX;
			_footStepsAudioSource.Play();
			_isFootstepPlaying = true;
		}
	}

	private void StopPlayingFootstepsSFX()
	{
		if(_isFootstepPlaying)
		{
			_footStepsAudioSource.Stop();
			_isFootstepPlaying = false;
		}
	}

	public void PlayLevelComplete()
	{
		_audioSource.PlayOneShot(_levelCompleteSFX, .2f);
	}

	public void PlayErrorSound()
	{
		_audioSource.PlayOneShot(_errorSoundSFX, .8f);
	}

}
