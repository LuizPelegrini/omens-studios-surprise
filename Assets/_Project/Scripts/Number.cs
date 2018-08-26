using UnityEngine;

public class Number : MonoBehaviour {

	public Player _player;
	public int numberValue;

	[HideInInspector] public Sprite sprite;

	void Awake()
	{
		sprite = GetComponentInChildren<SpriteRenderer>().sprite;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.CompareTag("Player"))
		{
			_player.CollectNumber(this);
			this.gameObject.SetActive(false);
		}
	}

	public void Drop()
	{
		this.gameObject.SetActive(true);
	}

}
