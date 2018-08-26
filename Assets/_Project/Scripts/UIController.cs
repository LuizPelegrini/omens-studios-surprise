using UnityEngine;
using UnityEngine.UI;

public class UIController : Singleton<UIController> {
	
	public Image left;
	public Image right;

	protected override void Awake()
	{
		left.sprite = null;
		right.sprite = null;

		this.IsPersistentBetweenScenes = false;
		base.Awake();
	}

	public void ChangeInventory(Player player)
	{
		if(player.numbersCollected[0] != null)
			left.sprite = player.numbersCollected[0].sprite;

		if(player.numbersCollected[1] != null)
			right.sprite = player.numbersCollected[1].sprite;
	}

}
