using UnityEngine;
using UnityEngine.UI;

public class UIController : Singleton<UIController> {
	
	public Image left;
	public Image right;

	protected override void Awake()
	{
		Clear();

		this.IsPersistentBetweenScenes = false;
		base.Awake();
	}

	public void ChangeInventory(Player player)
	{
		if(player.numbersCollected[0] != null)
		{
			left.color = Color.white;
			left.sprite = player.numbersCollected[0].sprite;
		}
		
		if(player.numbersCollected[1] != null)
		{
			right.color = Color.white;
			right.sprite = player.numbersCollected[1].sprite;
		}
	}

	public void Clear()
	{
		left.sprite = null;
		left.color = Color.clear;

		right.sprite = null;
		right.color = Color.clear;
	}

}
