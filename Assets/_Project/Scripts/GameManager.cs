
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {

	private const string _menuSceneName = "Menu";

	public bool gameOver;
	public Player player;
	public Vector2 initialPlayerPosition;
	public bool gameCompleted;

	public Animator _panelAnimator;
	public AudioSource _audioSourceUI;
	public AudioClip _clickSFX;
	
	protected override void Awake()
	{
		this.IsPersistentBetweenScenes = false;
		base.Awake();
	}

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

	public void CompleteGame()
	{
		gameCompleted = true;
		_panelAnimator.SetBool("gameCompleted", gameCompleted);
	}

	public void Restart()
	{
		_audioSourceUI.PlayOneShot(_clickSFX);

		gameCompleted = false;
		gameOver = false;

		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void GoToMenu()
	{
		_audioSourceUI.PlayOneShot(_clickSFX);

		gameCompleted = false;
		gameOver = false;

		SceneManager.LoadScene(_menuSceneName);
	}
}
