using UnityEngine;
using UnityEngine.UI;

public class UIController : Singleton<UIController> {
	
	public Image left;
	public Image right;

	public Transform operandA, operandB;

	protected override void Awake()
	{
		Clear();

		operandA.position = new Vector2(CameraFollow.cameraPosition.x - CameraUtil.halfScreenWidthInWorldUnits + 1f, 
										CameraFollow.cameraPosition.y + CameraUtil.halfScreenHeightInWorldUnits - 1f);
		
		
		operandB.position = new Vector2(CameraFollow.cameraPosition.x - CameraUtil.halfScreenWidthInWorldUnits + 1f, 
										CameraFollow.cameraPosition.y + CameraUtil.halfScreenHeightInWorldUnits - 2.3f);

		this.IsPersistentBetweenScenes = false;
		base.Awake();
	}

	void Update()
	{
		// operandA.position = new Vector2(CameraFollow.cameraPosition.x - CameraUtil.halfScreenWidthInWorldUnits + 1f, 
		// 								CameraFollow.cameraPosition.y + CameraUtil.halfScreenHeightInWorldUnits - .5f);
		
		
		// operandB.position = new Vector2(CameraFollow.cameraPosition.x - CameraUtil.halfScreenWidthInWorldUnits + 1f, 
		// 								CameraFollow.cameraPosition.y + CameraUtil.halfScreenHeightInWorldUnits - 2.3f);
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
