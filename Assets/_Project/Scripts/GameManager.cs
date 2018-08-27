
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {

	private const string _menuSceneName = "Menu";

	public bool gameOver, gamePaused;
	public Player player;
	public Vector2 initialPlayerPosition;
	public bool gameCompleted;

	public Animator _panelAnimator;
	public Animator _pausePanelAnimator;
	public AudioSource _audioSourceUI;
	public AudioClip _clickSFX;

	public GameObject invisibleWallGO;

	public GameObject _pauseButtonGO;
	
	protected override void Awake()
	{
		invisibleWallGO.transform.position = new Vector3(-CameraUtil.halfScreenWidthInWorldUnits - .4f, invisibleWallGO.transform.position.y);
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
		Destroy(_pauseButtonGO);
		gameCompleted = true;
		_panelAnimator.SetBool("gameCompleted", gameCompleted);
	}

	public void Restart()
	{
		Time.timeScale = 1f;

		_audioSourceUI.PlayOneShot(_clickSFX);

		gameCompleted = false;
		gameOver = false;
		gamePaused = false;

		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void GoToMenu()
	{
		Time.timeScale = 1f;

		_audioSourceUI.PlayOneShot(_clickSFX);

		gameCompleted = false;
		gameOver = false;
		gamePaused = false;

		SceneManager.LoadScene(_menuSceneName);
	}

	public void Pause()
	{
		Time.timeScale = 0f;
		gamePaused = true;
		_pausePanelAnimator.SetBool("gameCompleted", true);
		_audioSourceUI.PlayOneShot(_clickSFX);
	}

	public void Resume()
	{
		Time.timeScale = 1f;
		gamePaused = false;
		_pausePanelAnimator.SetBool("gameCompleted", false);
		_audioSourceUI.PlayOneShot(_clickSFX);
	}

	public void Quit()
	{
		_audioSourceUI.PlayOneShot(_clickSFX);
		Application.Quit();
	}
}
