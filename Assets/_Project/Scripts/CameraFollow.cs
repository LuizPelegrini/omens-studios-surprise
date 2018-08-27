using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform target;

	public float top, bottom, left, right;
	public float followSpeed = 4f;
	public float offset = .05f;
	[HideInInspector] public static bool shaking = false;

	public static Vector2 cameraPosition;

	void OnEnable()
	{
		cameraPosition = transform.position;
	}

	void Update () 
	{
		if(GameManager.Instance.gameOver)
			return;

		// Follow the player with a smooth movement
		if(!shaking)
		{
			float tX = Mathf.Lerp(transform.position.x, target.position.x, followSpeed * Time.deltaTime);
			float tY = Mathf.Lerp(transform.position.y, target.position.y, followSpeed * Time.deltaTime);
		
			transform.position = new Vector3(
				Mathf.Clamp(tX, left, right),
				Mathf.Clamp(tY, bottom, top),
				transform.position.z
			);

			cameraPosition = transform.position;
		}
	}

	/// <summary>
	/// Set the camera position at a random point
	/// </summary>
	public void ShakeCamera()
	{
		Vector3 random = Random.insideUnitCircle;
		transform.position = transform.position + (random * offset);
	}
}
