using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {


	public bool gameOver;
	public Player player;
	public Vector2 initialPlayerPosition;
	
	// Update is called once per frame
	void Update () 
	{
		if(gameOver)
		{
			gameOver = false;
			player.ChangePosition(initialPlayerPosition);
			player.EmptyInventory();
		}
	}
}
