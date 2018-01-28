using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement; // include so we can load new scenes

public class GameLoseManager : MonoBehaviour {

	public Button button;
	public Text score;
	public Text highScore;
	// init the menu
	void Awake()
	{
		// setup the listener to loadlevel when clicked
		button.onClick.RemoveAllListeners();
		button.onClick.AddListener(() => loadLevel("MainMenu"));

		score.text = "Score: " + PlayerPrefManager.GetScore ();
		highScore.text = "High score: " + PlayerPrefManager.GetHighscore();
	}
		
	public void loadLevel(string name)
	{
		SceneManager.LoadScene(name);
	}
}
