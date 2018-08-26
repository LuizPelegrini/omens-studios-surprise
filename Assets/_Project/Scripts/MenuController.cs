using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
	private const string _sceneLevelName = "Level";

	public Button playButton;
	public Button instructionsButton;
	public Button closePanelButton;

	public GameObject instructionsPanel;
	private AudioSource _audioSource;
	[SerializeField] private AudioClip _clickSFX;
	

	// Use this for initialization
	void Start () 
	{
		_audioSource = GetComponent<AudioSource>();

		playButton.onClick.AddListener(Play);
		
		instructionsButton.onClick.AddListener(() => {
			_audioSource.PlayOneShot(_clickSFX);
			instructionsPanel.SetActive(true);
		});	
		
		closePanelButton.onClick.AddListener(() => {
			_audioSource.PlayOneShot(_clickSFX);
			instructionsPanel.SetActive(false);
		});
	}

	void Play()
	{
		_audioSource.PlayOneShot(_clickSFX);
		SceneManager.LoadScene(_sceneLevelName);
	}
}
