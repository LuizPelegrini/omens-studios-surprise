using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform target;

	public float top, bottom, left, right;
	
	// Update is called once per frame
	void Update () 
	{
		if(GameManager.Instance.gameOver)
			return;

		transform.position = new Vector3(
			Mathf.Clamp(target.position.x, left, right),
			Mathf.Clamp(target.position.y, bottom, top),
			transform.position.z
		);
	}
}
