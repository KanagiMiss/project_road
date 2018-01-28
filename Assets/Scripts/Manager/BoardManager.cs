using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic; 		//Allows us to use Lists.
using Random = UnityEngine.Random; 		//Tells Random to use the Unity Engine random number generator.

namespace GOAT

{

	public class BoardManager : MonoBehaviour
	{
		// Using Serializable allows us to embed a class with sub properties in the inspector.
		[Serializable]
		public class Count
		{
			public int minimum; 			//Minimum value for our Count class.
			public int maximum; 			//Maximum value for our Count class.


			//Assignment constructor.
			public Count (int min, int max)
			{
				minimum = min;
				maximum = max;
			}
		}
			
		public int columns = 8; 										//Number of columns in our game board.
		public int rows = 8;											//Number of rows in our game board.
		public Count wallCount = new Count (5, 9);						//Lower and upper limit for our random number of walls per level.
		public Count foodCount = new Count (1, 5);						//Lower and upper limit for our random number of food items per level.
		public GameObject exit;											//Prefab to spawn for exit.
		public GameObject[] floorTiles;									//Array of floor prefabs.
		public GameObject[] wallTiles;									//Array of wall prefabs.
		public GameObject[] foodTiles;									//Array of food prefabs.
		public GameObject[] enemyTiles;									//Array of enemy prefabs.
		public GameObject[] outerWallTiles;								//Array of outer tile prefabs.
		public GameObject splitLine;
		public GameObject visualGC;
		public GameObject blackScreen;
		public GameObject player1, player2;
		public GameObject mailbox1, mailbox2;
		public GameObject messenger1, messenger2;

		private Transform boardHolder;									//A variable to store a reference to the transform of our Board object.
		private List <Vector3> gridPositions = new List <Vector3> ();	//A list of possible locations to place tiles.
		private List <Vector3> gridEnemySpawnPositions = new List <Vector3> ();
		private List <Vector3> gridEnemySpawnPositionsOrig = new List <Vector3> ();
		private GameObject mb1, mb2, mg1, mg2;

		private int mailbox_x = 0, mailbox_x_usage = 2;

		void Awake()
		{
			mailbox_x_usage = 2;
			mailbox_x = Random.Range (1, columns - 1);
		}

		void OnEnable ()
		{
			EventManager.StartListening ("Player1ArriveMailbox", OnPlayer1ArriveMailbox);
			EventManager.StartListening ("Player2ArriveMailbox", OnPlayer2ArriveMailbox);
			EventManager.StartListening ("Player1ArriveMessenger", OnPlayer1ArriveMessenger);
			EventManager.StartListening ("Player2ArriveMessenger", OnPlayer2ArriveMessenger);
			EventManager.StartListening ("Player1Death", OnPlayer1Death);
			EventManager.StartListening ("Player2Death", OnPlayer2Death);
		}

		void OnDisable ()
		{
			EventManager.StopListening ("Player1ArriveMailbox", OnPlayer1ArriveMailbox);
			EventManager.StopListening ("Player2ArriveMailbox", OnPlayer2ArriveMailbox);
			EventManager.StopListening ("Player1ArriveMessenger", OnPlayer1ArriveMessenger);
			EventManager.StopListening ("Player2ArriveMessenger", OnPlayer2ArriveMessenger);
			EventManager.StopListening ("Player1Death", OnPlayer1Death);
			EventManager.StopListening ("Player2Death", OnPlayer2Death);
		}

		//Clears our list gridPositions and prepares it to generate a new board.
		void InitialiseList ()
		{
			//Clear our list gridPositions.
			gridPositions.Clear ();

			//Loop through x axis (columns).
			for(int x = 1; x < columns-1; x++)
			{
				//Within each column, loop through y axis (rows).
				for(int y = 1; y < rows-1; y++)
				{
					//At each index add a new Vector3 to our list with the x and y coordinates of that position.
					gridPositions.Add (new Vector3(x, y, 0f));
				}
			}
		}

		void OnPlayer1Death()
		{
			//top
			Instantiate (player1, new Vector3 (Random.Range(1, columns-1), rows-1, 0f), Quaternion.identity);
		}

		void OnPlayer2Death()
		{
			//bottom
			Instantiate (player2, new Vector3 (Random.Range(1, columns-1), 0, 0f), Quaternion.identity);
		}

		void OnPlayer1ArriveMailbox()
		{
			Destroy (mb1);
			//top
			mg1 = Instantiate (messenger1, new Vector3 (0, rows-1, 0f), Quaternion.identity);
			mg1.GetComponent<Messenger> ().SetMailingDestination (new Vector3 (Random.Range(1, columns-1), rows-1, 0f));
			mg1.GetComponent<Messenger> ().SetMailType (1);
		}

		void OnPlayer2ArriveMailbox()
		{
			Destroy (mb2);
			//bottom
			mg2 = Instantiate (messenger2, new Vector3 (0, 0, 0f), Quaternion.identity);
			mg2.GetComponent<Messenger> ().SetMailingDestination (new Vector3 (Random.Range(1, columns-1), 0, 0f));
			mg2.GetComponent<Messenger> ().SetMailType (2);
		}

		void OnPlayer1ArriveMessenger()
		{
			//Destroy (mg1);
			if(mailbox_x_usage <= 0){
				mailbox_x_usage = 2;
				mailbox_x = Random.Range (1, columns - 1);
			}
			mailbox_x_usage--;
			mb1 = Instantiate (mailbox1, new Vector3 (mailbox_x, (rows/2)+1f, 0f), Quaternion.identity);
		}

		void OnPlayer2ArriveMessenger()
		{
			//Destroy (mg2);
			if(mailbox_x_usage <= 0){
				mailbox_x_usage = 2;
				mailbox_x = Random.Range (1, columns - 1);
			}
			mailbox_x_usage--;
			mb2 = Instantiate (mailbox2, new Vector3 (mailbox_x, (rows/2)-0.5f, 0f), Quaternion.identity);
		}

		void InitialiseEnemySpawnPoints()
		{
			gridEnemySpawnPositions.Clear ();
			int x = 0;
			for (int y = 1; y < rows - 1; ++y) {
				int random_num = Random.Range (0, 100);
				if (random_num > 50) {
					x = 0;
					//x = -1;
				} else if (random_num < 50) {
					x = columns - 1;
					//x = columns;
				}
				if (y != rows / 2) {
					gridEnemySpawnPositions.Add (new Vector3 (x, y, 0f));
					gridEnemySpawnPositionsOrig.Add (new Vector3 (x, y, 0f));
				}
			}
		}

		void ResetEnemySpawnPoints()
		{
			gridEnemySpawnPositions.Clear();
			foreach (var pos in gridEnemySpawnPositionsOrig.ToArray()){
				gridEnemySpawnPositions.Add(pos);
			}
		}

		//Sets up the outer walls and floor (background) of the game board.
		void BoardSetup ()
		{
			//Instantiate Board and set boardHolder to its transform.
			boardHolder = new GameObject ("Board").transform;

			//Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
			for(int x = -1; x < columns + 1; x++)
			{
				//Loop along y axis, starting from -1 to place floor or outerwall tiles.
				for(int y = -1; y < rows + 1; y++)
				{
					//Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
					GameObject toInstantiate = floorTiles[Random.Range (0,floorTiles.Length)];

					//Check if we current position is at board edge, if so choose a random outer wall prefab from our array of outer wall tiles.
					if(x == -1 || x == columns || y == -1 || y == rows)
						toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];

					//Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
					GameObject instance =
						Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;

					//Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
					instance.transform.SetParent (boardHolder);
				}
			}
		}

		//RandomPosition returns a random position from our list gridPositions.
		Vector3 RandomPosition ()
		{
			//Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
			int randomIndex = Random.Range (0, gridPositions.Count);

			//Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
			Vector3 randomPosition = gridPositions[randomIndex];

			//Remove the entry at randomIndex from the list so that it can't be re-used.
			gridPositions.RemoveAt (randomIndex);

			//Return the randomly selected Vector3 position.
			return randomPosition;
		}

		//LayoutObjectAtRandom accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.
		void LayoutObjectAtRandom (GameObject[] tileArray, int minimum, int maximum)
		{
			//Choose a random number of objects to instantiate within the minimum and maximum limits
			int objectCount = Random.Range (minimum, maximum+1);

			//Instantiate objects until the randomly chosen limit objectCount is reached
			for(int i = 0; i < objectCount; i++)
			{
				//Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
				Vector3 randomPosition = RandomPosition();

				//Choose a random tile from tileArray and assign it to tileChoice
				GameObject tileChoice = tileArray[Random.Range (0, tileArray.Length)];

				//Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
				Instantiate(tileChoice, randomPosition, Quaternion.identity);
			}
		}

		IEnumerator SpawnEnemy(GameObject[] tileArray, int minimum, int maximum)
		{
			while (true) {
				//Choose a random number of objects to instantiate within the minimum and maximum limits
				int objectCount = Random.Range (minimum, maximum+1);

				//Instantiate objects until the randomly chosen limit objectCount is reached
				for(int i = 0; i < objectCount; i++)
				{
					int randomIndex = Random.Range (0, gridEnemySpawnPositions.Count);
					Vector3 randomPosition = gridEnemySpawnPositions[randomIndex];
					gridEnemySpawnPositions.RemoveAt (randomIndex);

					//Choose a random tile from tileArray and assign it to tileChoice
					GameObject tileChoice = tileArray[Random.Range (0, tileArray.Length)];

					//Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
					Instantiate(tileChoice, randomPosition, Quaternion.identity);
				}
				ResetEnemySpawnPoints ();
				yield return new WaitForSeconds(2f);
			}
		}

		void LayoutEnemyAtSpawnPoint(GameObject[] tileArray, int minimum, int maximum)
		{
			/*//Choose a random number of objects to instantiate within the minimum and maximum limits
			int objectCount = Random.Range (minimum, maximum+1);

			//Instantiate objects until the randomly chosen limit objectCount is reached
			for(int i = 0; i < objectCount; i++)
			{
				int randomIndex = Random.Range (0, gridEnemySpawnPositions.Count);
				Vector3 randomPosition = gridEnemySpawnPositions[randomIndex];
				gridEnemySpawnPositions.RemoveAt (randomIndex);

				//Choose a random tile from tileArray and assign it to tileChoice
				GameObject tileChoice = tileArray[Random.Range (0, tileArray.Length)];

				//Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
				Instantiate(tileChoice, randomPosition, Quaternion.identity);
			}*/
			StartCoroutine(SpawnEnemy(tileArray, minimum, maximum));
		}

		void LayoutSplitLine()
		{
			Instantiate (splitLine, new Vector3 (columns / 2, rows / 2, 0f), Quaternion.identity);
		}

		void LayoutVisualGC()
		{
			Instantiate (visualGC, new Vector3 (-2, rows / 2, 0f), Quaternion.identity);
			Instantiate (visualGC, new Vector3 (columns + 2, rows / 2, 0f), Quaternion.identity);
		}

		void LayoutBlackScreen()
		{
			Instantiate (blackScreen, new Vector3 (-8, rows / 2, 0f), Quaternion.identity);
			Instantiate (blackScreen, new Vector3 (columns+8, rows / 2, 0f), Quaternion.identity);
		}

		void LayoutPlayer()
		{
			//top
			Instantiate (player1, new Vector3 (Random.Range(1, columns-1), rows-1, 0f), Quaternion.identity);
			//bottom
			Instantiate (player2, new Vector3 (Random.Range(1, columns-1), 0, 0f), Quaternion.identity);
		}

		void LayoutMailbox()
		{
			int x = Random.Range (1, columns - 1);
			//up
			mb1 = Instantiate (mailbox1, new Vector3 (x, (rows/2)+1f, 0f), Quaternion.identity);
			//down
			mb2 = Instantiate (mailbox2, new Vector3 (x, (rows/2)-0.5f, 0f), Quaternion.identity);
		}


		//邮差的生成函数
		void LayoutMessenger()
		{
			//top
			mg1 = Instantiate (messenger1, new Vector3 (0, rows-1, 0f), Quaternion.identity);
			mg1.GetComponent<Messenger> ().SetMailingDestination (new Vector3 (Random.Range(1, columns-1), rows-1, 0f));
			mg1.GetComponent<Messenger> ().SetMailType (1);
			//bottom
			mg2 = Instantiate (messenger2, new Vector3 (0, 0, 0f), Quaternion.identity);
			mg2.GetComponent<Messenger> ().SetMailingDestination (new Vector3 (Random.Range(1, columns-1), 0, 0f));
			mg2.GetComponent<Messenger> ().SetMailType (2);
		}

		//SetupScene initializes our level and calls the previous functions to lay out the game board
		public void SetupScene (int level)
		{
			//Creates the outer walls and floor.
			BoardSetup ();

			//Reset our list of gridpositions.
			InitialiseList ();
			InitialiseEnemySpawnPoints ();

			//Instantiate a random number of wall tiles based on minimum and maximum, at randomized positions.
			//LayoutObjectAtRandom (wallTiles, wallCount.minimum, wallCount.maximum);

			//Instantiate a random number of food tiles based on minimum and maximum, at randomized positions.
			LayoutObjectAtRandom (foodTiles, foodCount.minimum, foodCount.maximum);

			LayoutSplitLine ();
			LayoutVisualGC ();
			LayoutBlackScreen ();
			LayoutMessenger ();
			//LayoutMailbox ();
			LayoutPlayer ();

			//Determine number of enemies based on current level number, based on a logarithmic progression
			//int enemyCount = (int)Mathf.Log(level, 2f);
			int enemyCount = (int)Mathf.Log(100, 2f);

			//Instantiate a random number of enemies based on minimum and maximum, at randomized positions.
			//LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);
			LayoutEnemyAtSpawnPoint (enemyTiles, 10, 10);

			//Instantiate the exit tile in the upper right hand corner of our game board
			//Instantiate (exit, new Vector3 (columns - 1, rows - 1, 0f), Quaternion.identity);
		}
	}
}
