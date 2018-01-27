using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace GOAT
{
	using System.Collections.Generic;		//Allows us to use Lists. 
	using UnityEngine.UI;					//Allows us to use UI.

	public class GameManager : MonoBehaviour
	{
		public float levelStartDelay = 2f;						//Time to wait before starting level, in seconds.
		public float turnDelay = 0.1f;							//Delay between each Player turn.
		public int playerFoodPoints = 100;						//Starting value for Player food points.
		public static GameManager instance = null;				//Static instance of GameManager which allows it to be accessed by any other script.

		private Text levelText;									//Text to display current level number.
		private GameObject levelImage;							//Image to block out level as levels are being set up, background for levelText.
		private BoardManager boardScript;						//Store a reference to our BoardManager which will set up the level.
		private int level = 1;									//Current level number, expressed in game as "Day 1".
		private List<Enemy> enemies;							//List of all Enemy units, used to issue them move commands.
		private bool enemiesMoving;								//Boolean to check if enemies are moving.
		private bool doingSetup = true;							//Boolean to check if we're setting up board, prevent Player from moving during setup.


		//code added for UI begin 
		public int score = 0;
		public int highScore = 0;
		public int startLives = 10;
		public int lives = 10;
		public float reminingTime = 120;

		public Text UIScore;
		public Text UIHighScore;
		public Text UIReminingTime;
		public GameObject[] UIExtraLives;

		public GameObject[] Players;
		//code addeb for UI end


		//Awake is always called before any Start functions
		void Awake()
		{
			//Check if instance already exists
			if (instance == null)

				//if not, set instance to this
				instance = this;

			//If instance already exists and it's not this:
			else if (instance != this)

				//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
				Destroy(gameObject);	

			//Sets this to not be destroyed when reloading scene
			DontDestroyOnLoad(gameObject);

			//Assign enemies to a new List of Enemy objects.
			enemies = new List<Enemy>();

			//Get a component reference to the attached BoardManager script
			boardScript = GetComponent<BoardManager>();

			//Call the InitGame function to initialize the first level 
			InitGame();
		}

		//this is called only once, and the paramter tell it to be called only after the scene was loaded
		//(otherwise, our Scene Load callback would be called the very first load, and we don't want that)
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		static public void CallbackInitialization()
		{
			//register the callback to be called everytime the scene is loaded
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		//This is called each time a scene is loaded.
		static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
		{
			//instance.level++;
			instance.InitGame();
		}


		//Initializes the game for each level.
		void InitGame()
		{
			/*
			//While doingSetup is true the player can't move, prevent player from moving while title card is up.
			doingSetup = true;

			//Get a reference to our image LevelImage by finding it by name.
			levelImage = GameObject.Find("LevelImage");

			//Get a reference to our text LevelText's text component by finding it by name and calling GetComponent.
			levelText = GameObject.Find("LevelText").GetComponent<Text>();

			//Set the text of levelText to the string "Day" and append the current level number.
			levelText.text = "Day " + level;

			//Set levelImage to active blocking player's view of the game board during setup.
			levelImage.SetActive(true);

			//Call the HideLevelImage function with a delay in seconds of levelStartDelay.
			Invoke("HideLevelImage", levelStartDelay);

			//Clear any Enemy objects in our List to prepare for next level.
			enemies.Clear();
			*/
			//Call the SetupScene function of the BoardManager script, pass it current level number.
			boardScript.SetupScene(level);
			if (Players == null) {
				Players = GameObject.FindGameObjectsWithTag ("Hero");
			}
			if (Players == null) {
				Debug.LogError ("Player not found in Game Manager!");
			}

			if (UIScore == null) {
				Debug.LogError ("need to set UIScore on Game Manager!");
			}

			if (UIHighScore == null) {
				Debug.LogError ("need to set UIHighScore on Game Manager!");
			}

			//get stored player prefs
			refreshPlayerState();

			//get the UI ready for the game
			refreshGUI();
		}


		//Hides black image used between levels
		void HideLevelImage()
		{
			//Disable the levelImage gameObject.
			levelImage.SetActive(false);

			//Set doingSetup to false allowing player to move again.
			doingSetup = false;
		}

		//Update is called every frame.
		void Update()
		{
//			//Check that playersTurn or enemiesMoving or doingSetup are not currently true.
//			if(enemiesMoving || doingSetup)
//
//				//If any of these are true, return and do not start MoveEnemies.
//				return;

			//Start moving enemies.
			//StartCoroutine (MoveEnemies ());
			reminingTime -= Time.deltaTime;
			if (reminingTime <= 0 || lives <= 0) {
				SceneManager.LoadScene("GameLose");
				return;

			}

			int minute = (int)(reminingTime / 60);
			int second = (int)(Mathf.Floor(reminingTime % 60));
			int millionSecond = (int)((reminingTime * 100) % 100);

			UIReminingTime.text = number2Text(minute)+ ":" + number2Text(second) + ":" +number2Text(millionSecond);

		}

		private string number2Text(int number){
			if (number < 10) {
				return "0" + number.ToString ();
			} else {
				return number.ToString ();
			}
		}

		//Call this to add the passed in Enemy to the List of Enemy objects.
		public void AddEnemyToList(Enemy script)
		{
			//Add Enemy to List enemies.
			enemies.Add(script);
		}


		//GameOver is called when the player reaches 0 food points
		public void GameOver()
		{
			//Set levelText to display number of levels passed and game over message
			levelText.text = "After " + level + " days, you starved.";

			//Enable black background image gameObject.
			levelImage.SetActive(true);

			//Disable this GameManager.
			enabled = false;
		}

		//get stored player prefs
		void refreshPlayerState(){
			//lives update
			//score update
			//highScore update
			if(score > highScore){
				highScore = score;
			}
		}

		//get the UI ready for the game
		void refreshGUI(){
			UIScore.text = "Score: " + score.ToString ();
			UIHighScore.text = "HighScore: " + highScore.ToString ();
			UIReminingTime.text = reminingTime.ToString ();
			for (int i = 0; i < UIExtraLives.Length; i++) {
				if (i < (lives)) {
					UIExtraLives[i].SetActive (true);
				} else {
					UIExtraLives[i].SetActive (false);
				}
			} 
		}

		public bool playerDead(){
			if (lives > 0) {
				lives--;
				for (int i = 0; i < UIExtraLives.Length; i++) {
					if (i < lives) {
						UIExtraLives[i].SetActive (true);
					} else {
						UIExtraLives [i].SetActive (false);
					}
				}
				return true;
			} else {
				return false;
			}
		}

		public void playerGetScore(int s){
			score += s; 
			UIScore.text = "Score: " + score.ToString ();
			if (score > highScore) {
				highScore = score;
				UIHighScore.text = "HighScore: " + highScore.ToString ();
			}
		}



	}
}

