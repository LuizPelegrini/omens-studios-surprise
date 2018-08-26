using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour {

	private float _speed = 1f;
	public float offset;
	public float minSpeed, maxSpeed;
	public float minSize, maxSize;

	
	void OnEnable()
	{
		float size = Random.Range(minSize, maxSize);
		_speed = Random.Range(minSpeed, maxSpeed);
		transform.localScale = Vector2.one * size;
	}

	// Update is called once per frame
	void Update () 
	{
		// Move even if the game is paused
		transform.Translate(Vector2.left * _speed * Time.unscaledDeltaTime);

		if(IsOutOfScreen())
		{
			gameObject.SetActive(false);
		}
	}

	private bool IsOutOfScreen()
	{
		return transform.position.x < -(CameraUtil.halfScreenWidthInWorldUnits + offset);
	}
}
