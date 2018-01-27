using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement; // include so we can load new scenes

public class GameLoseManager : MonoBehaviour {

	public Button button;
	// init the menu
	void Awake()
	{
		// setup the listener to loadlevel when clicked
		button.onClick.RemoveAllListeners();
		button.onClick.AddListener(() => loadLevel("MainMenu"));
	}

	public void loadLevel(string name)
	{
		Debug.Log ("OJ8K!");
		SceneManager.LoadScene(name);
	}
}
