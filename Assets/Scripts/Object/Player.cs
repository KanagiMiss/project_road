using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAT
{
	
	public class Player : MonoBehaviour {

		// player controls
		[Range(0.0f, 10.0f)] // create a slider in the editor and set limits on moveSpeed
		public float moveSpeed = 3f;

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
		float _ax, _vx, last_vx, last_vx_time;
		float _ay, _vy, last_vy, last_vy_time;

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

			_vx = _vy = 0;
		}

		void Start()
		{
			//anim = GetComponent<Animator>();
		}

		// this is where most of the player controller magic happens each game event loop
		void Update()
		{
			// exit update if player cannot move or game is paused
			if (!playerCanMove || (Time.timeScale == 0f))
				return;

			if (playerId == 1) {
				// determine horizontal velocity change based on the horizontal input
				_ax = Input.GetAxisRaw ("P1_Horizontal");
				_ay = Input.GetAxisRaw ("P1_Vertical");
			} else if (playerId == 2) {
				_ax = Input.GetAxisRaw ("P2_Horizontal");
				_ay = Input.GetAxisRaw ("P2_Vertical");
			}

			_vx = _vx + pullA * _ax * Time.deltaTime;
			_vy = _vy + pullA * _ay * Time.deltaTime;

			_vx = _vx + Mathf.Sign (_vx) * (-1) * dragA * Time.deltaTime;
			_vy = _vy + Mathf.Sign (_vy) * (-1) * dragA * Time.deltaTime;

			_vx = Mathf.Sign (_vx) * Mathf.Clamp (Mathf.Abs (_vx), 0f, 1f);
			_vy = Mathf.Sign (_vy) * Mathf.Clamp (Mathf.Abs (_vy), 0f, 0.8f);

			if (Mathf.Abs (_vx) < 0.05f) {
				_vx = 0f;
			}

			if (Mathf.Abs (_vy) < 0.05f) {
				_vy = 0f;
			}

			// Determine if running based on the horizontal movement
			if (_vx != 0 || _vy != 0) 
			{
				isRunning = true;
			} else {
				isRunning = false;
			}

			/*/ lerp
			if (_vx == 0) {
				float t = Mathf.Pow (Time.time - last_vx_time, 2);
				float sign = Mathf.Sign (last_vx);
				last_vx = sign * Mathf.Clamp (Mathf.Abs(last_vx) - t * t * speedDecay, 0f, 99f);
			} else {
				last_vx = _vx;
				last_vx_time = Time.time;
			}

			if (_vy == 0) {
				float t = Mathf.Pow (Time.time - last_vy_time, 2);
				float sign = Mathf.Sign (last_vy);
				last_vy = sign * Mathf.Clamp (Mathf.Abs(last_vy) - t * t * speedDecay, 0f, 99f);
			} else {
				last_vy = _vy;
				last_vy_time = Time.time;
			}*/

			// set the running animation state
			//_animator.SetBool("Running", isRunning);
		
			// Change the actual velocity on the rigidbody
			_rigidbody.velocity = new Vector2(_vx * moveSpeed, _vy * moveSpeed);

			// if moving up then don't collide with platform layer
			// this allows the player to jump up through things on the platform layer
			// NOTE: requires the platforms to be on a layer named "Platform"
			//Physics2D.IgnoreLayerCollision(_playerLayer, _platformLayer, (_vy > 0.0f)); 
		}

		// Checking to see if the sprite should be flipped
		// this is done in LateUpdate since the Animator may override the localScale
		// this code will flip the player even if the animator is controlling scale
		void LateUpdate()
		{
			// get the current scale
			Vector3 localScale = _transform.localScale;

			if (_vx > 0) // moving right so face right
			{
				facingRight = true;
			} else if (_vx < 0) { // moving left so face left
				facingRight = false;
			}

			// check to see if scale x is right for the player
			// if not, multiple by -1 which is an easy way to flip a sprite
			if (((facingRight) && (localScale.x<0)) || ((!facingRight) && (localScale.x>0))) {
				localScale.x *= -1;
			}

			// update the scale
			_transform.localScale = localScale;
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