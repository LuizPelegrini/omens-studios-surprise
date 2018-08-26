using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	public Button playButton;
	public Button instructionsButton;
	public Button closePanelButton;

	public GameObject instructionsPanel;

	private const string _sceneLevelName = "Level";

	// Use this for initialization
	void Start () 
	{
		playButton.onClick.AddListener(Play);
		
		instructionsButton.onClick.AddListener(() => {
			instructionsPanel.SetActive(true);
		});	
		
		closePanelButton.onClick.AddListener(() => {
			instructionsPanel.SetActive(false);
		});
	}

	void Play()
	{
		SceneManager.LoadScene(_sceneLevelName);
	}
}
