using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAT
{
	

	public class Player_direct : MonoBehaviour {

		// player controls
		[Range(0.0f, 10.0f)] // create a slider in the editor and set limits on moveSpeed
		public float moveSpeed = 3f;
		public DirectionType facing = DirectionType.down;

		// player health
		public int playerHealth = 1;

		// which player
		public int playerId = 1;

		// player can move?
		// we want this public so other scripts can access it but we don't want to show in editor as it might confuse designer
		[HideInInspector]
		public bool playerCanMove = true;

		// SFXs
		public AudioClip coinSFX;
		public AudioClip deathSFX;
		public AudioClip jumpSFX;
		public AudioClip victorySFX;

		// particle
		public GameObject deathExplosion;

		// private variables below

		private float speedDecay = 15f;
		private float pullA = 4f;
		private float dragA = 2f;

		// store references to components on the gameObject
		private Transform _transform;
		private Rigidbody2D _rigidbody;
		private Animator _animator;
		private AudioSource _audio;

		// hold player motion in this timestep
		float xAxis, yAxis;
		//float _ay, _vy, last_vy, last_vy_time;

		// player tracking
		bool facingRight = true;
		bool isGrounded = false;
		bool isRunning = false;

		// store the layer the player is on (setup in Awake)
		int _playerLayer;

		void Awake () {
			// get a reference to the components we are going to be changing and store a reference for efficiency purposes
			_transform = GetComponent<Transform> ();

			_rigidbody = GetComponent<Rigidbody2D> ();
			if (_rigidbody==null) // if Rigidbody is missing
				Debug.LogError("Rigidbody2D component missing from this gameobject");
			/*
			_animator = GetComponent<Animator>();
			if (_animator==null) // if Animator is missing
				;//Debug.LogError("Animator component missing from this gameobject");

			_audio = GetComponent<AudioSource> ();
			if (_audio==null) { // if AudioSource is missing
				Debug.LogWarning("AudioSource component missing from this gameobject. Adding one.");
				// let's just add the AudioSource component dynamically
				_audio = gameObject.AddComponent<AudioSource>();
			}*/

			// determine the player's specified layer
			_playerLayer = this.gameObject.layer;
		}

		void Start()
		{
			//anim = GetComponent<Animator>();
		}

		// this is where most of the player controller magic happens each game event loop
		void Update()
		{

			if (playerId == 1) {
				// determine horizontal velocity change based on the horizontal input
				xAxis = Input.GetAxisRaw ("P1_Horizontal");
				yAxis = Input.GetAxisRaw ("P1_Vertical");
			} else if (playerId == 2) {
				xAxis = Input.GetAxisRaw ("P2_Horizontal");
				yAxis = Input.GetAxisRaw ("P2_Vertical");
			}
				
			Vector2 player_pos = transform.position;
			if (facing == DirectionType.left || facing == DirectionType.right) {
				//玩家上一轮是左右移动的，那么优先左右键
				if (xAxis != 0) {
					player_pos.x += xAxis * moveSpeed * Time.deltaTime;
					if (xAxis > 0) {
						facing = DirectionType.right;
					} else {
						facing = DirectionType.left;
					}
				} else if (yAxis != 0) {
					player_pos.y += yAxis * moveSpeed * Time.deltaTime;
					if (yAxis > 0) {
						facing = DirectionType.down;
					} else {
						facing = DirectionType.up;
					}
				}
			} else {
				//玩家上一轮是上下移动的，那么优先上下键
				if (yAxis != 0) {
					player_pos.y += yAxis * moveSpeed * Time.deltaTime;
					if (yAxis > 0) {
						facing = DirectionType.down;
					} else {
						facing = DirectionType.up;
					}
				} else if (xAxis != 0) {
					player_pos.x += xAxis * moveSpeed * Time.deltaTime;
					if (xAxis > 0) {
						facing = DirectionType.right;
					} else {
						facing = DirectionType.left;
					}
				}
			}
			transform.position = player_pos;
		}


		//OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
		void OnTriggerEnter2D (Collider2D other)
		{
			//Check if the tag of the trigger collided with is Exit.
			if(other.tag == "Exit")
			{

			}

			//Check if the tag of the trigger collided with is Food.
			else if(other.tag == "Food")
			{

			}

			//Check if the tag of the trigger collided with is Soda.
			else if(other.tag == "Soda")
			{

			}
		}

		private void CheckIfGameOver ()
		{
			//Check if food point total is less than or equal to zero.

		}

		// do what needs to be done to freeze the player
		void FreezeMotion() {
			playerCanMove = false;
			_rigidbody.isKinematic = true;
		}

		// do what needs to be done to unfreeze the player
		void UnFreezeMotion() {
			playerCanMove = true;
			_rigidbody.isKinematic = false;
		}

		// play sound through the audiosource on the gameobject
		void PlaySound(AudioClip clip)
		{
			_audio.PlayOneShot(clip);
		}

		// public function to apply damage to the player
		public void ApplyDamage (int damage) {
			if (playerCanMove) {
				playerHealth -= damage;

				if (playerHealth <= 0) { // player is now dead, so start dying
					//PlaySound(deathSFX);
					if (this.playerId == 1) {
						EventManager.TriggerEvent ("Player1Death");
					} else if (this.playerId == 2) {
						EventManager.TriggerEvent ("Player2Death");
					}
					//StartCoroutine (KillPlayer ());
					Destroy(this.gameObject);
					// create particle system
					GameObject obj = Instantiate(deathExplosion, this.transform.position, Quaternion.identity);
					StartCoroutine(destroyDeathExplosion(obj));
				}
			}
		}

		IEnumerator destroyDeathExplosion(GameObject obj)
		{
			yield return new WaitForSeconds (0.5f);
			Destroy (obj);
		}
	}

}