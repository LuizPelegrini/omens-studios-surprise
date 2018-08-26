using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathChallenge : MonoBehaviour {

	public enum Operation
	{
		ADDITION,
		SUBTRACTION
	}

	public Operation operation;
	public float operationResult;
	private BoxCollider2D _collider2D;
	public bool playerHasScored;
	public float destroySpeed = 1f;
	public float distance = 5f;
	public CameraFollow cameraFollow;
	public bool _isTheLastWall;

	// Use this for initialization
	void Start () 
	{
		_collider2D = GetComponent<BoxCollider2D>();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.CompareTag("Player") && !playerHasScored)
		{
			Player player = other.GetComponent<Player>();
			switch(operation)
			{
				case Operation.ADDITION:
					if(player.numbersCollectedIndex == player.numbersCollected.Length)
						playerHasScored = ((player.numbersCollected[0].numberValue + player.numbersCollected[1].numberValue) == operationResult);
					break;
				case Operation.SUBTRACTION:
					if(player.numbersCollectedIndex == player.numbersCollected.Length)
						playerHasScored = ((player.numbersCollected[0].numberValue - player.numbersCollected[1].numberValue) == operationResult);
					break;
			}

			if(playerHasScored)
			{
				player.EmptyInventory();
				GameManager.Instance.initialPlayerPosition = transform.position;
				player.PlayDestroyingDoor();
				StartCoroutine(Destroying(player));
			}
		}
	}

	IEnumerator Destroying(Player player)
	{
		CameraFollow.shaking = true;
		Vector3 ini = transform.position;
		while(Vector3.Distance(ini, transform.position) < distance)
		{
			transform.Translate(Vector2.down * destroySpeed * Time.deltaTime);
			cameraFollow.ShakeCamera();
			yield return null;
		}
		CameraFollow.shaking = false;

		if(_isTheLastWall)
		{
			player.PlayLevelComplete();
			GameManager.Instance.CompleteGame();
		}


		Destroy(gameObject);
	}
}
