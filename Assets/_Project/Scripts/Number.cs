using UnityEngine;
using System.Collections;

public class Number : MonoBehaviour {

	public Player _player;
	public int numberValue;

	[HideInInspector] public Sprite sprite;

	private float _speed;
	private Vector2 _initialPosition;
	private bool _isChangingPosition = false;
	[SerializeField] private float _moveSpeedToUI = 1f;
	[SerializeField] private float _moveSpeedToInitialPosition = 1f;

	void OnEnable()
	{
		_speed = Random.Range(2f, 5f);
	}

	void Awake()
	{
		_initialPosition = transform.position;
		sprite = GetComponentInChildren<SpriteRenderer>().sprite;
	}

	void Update()
	{
		if(!_isChangingPosition)
		{
			// sin movement
			float y = Mathf.Sin(Time.timeSinceLevelLoad * _speed);
			transform.Translate(Vector2.up * y * Time.deltaTime);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.CompareTag("Player") && !_isChangingPosition)
		{
			// this.gameObject.SetActive(false);
			Vector2 operandPosition = _player.CollectNumber(this);
			if(operandPosition != Vector2.zero)
				StartCoroutine(Move(operandPosition, true));
		}
	}

	public void Drop()
	{
		this.gameObject.SetActive(true);
		StartCoroutine(Move(UIController.Instance.operandA.position, false));
	}

	IEnumerator Move(Vector3 operandPos, bool isMovingToUI)
	{
		_isChangingPosition = true;

		Vector3 targetPosition;
		Vector3 direction;
		float speed;

		if(isMovingToUI)
		{
			targetPosition = operandPos;
			speed = _moveSpeedToUI;
		}
		else
		{
			targetPosition = _initialPosition;
			transform.position = operandPos;
			speed = _moveSpeedToInitialPosition;
		}

		direction = (targetPosition - transform.position).normalized;

		while(Vector2.Distance(transform.position, targetPosition) >= (direction * speed * Time.deltaTime).magnitude)
		{
			if(isMovingToUI)
				targetPosition = operandPos;
			
			direction = (targetPosition - transform.position).normalized;
			// transform.position = Vector3.Lerp(transform.position, targetPosition, _moveSpeed * Time.deltaTime);
			transform.position += (direction * speed * Time.deltaTime);
			yield return null;
		}

		// At this point, the number reached the targetPosition

		// If it is moving towards the inventory
		if(isMovingToUI)
		{
			UIController.Instance.ChangeInventory(_player);

			// Disable it
			gameObject.SetActive(false);
		}
		else
		{
			// Otherwise, starts the sin movement up and down
			_isChangingPosition = false;
		}
	}

}
