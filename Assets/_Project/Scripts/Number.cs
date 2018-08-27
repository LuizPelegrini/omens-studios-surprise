using UnityEngine;

public class Number : MonoBehaviour {

	public Player _player;
	public int numberValue;

	[HideInInspector] public Sprite sprite;

	private float _speed;

	void OnEnable()
	{
		_speed = Random.Range(2f, 5f);
	}

	void Awake()
	{
		sprite = GetComponentInChildren<SpriteRenderer>().sprite;
	}

	void Update()
	{
		// sin movement
		float y = Mathf.Sin(Time.timeSinceLevelLoad * _speed);
		transform.Translate(Vector2.up * y * Time.deltaTime);
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
