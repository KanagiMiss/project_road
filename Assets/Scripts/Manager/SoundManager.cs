using UnityEngine;
using System.Collections;

namespace GOAT
{
	public class SoundManager : MonoBehaviour 
	{
		public AudioSource efxSource;					//Drag a reference to the audio source which will play the sound effects.
		public AudioSource musicSource;					//Drag a reference to the audio source which will play the music.
		public static SoundManager instance = null;		//Allows other scripts to call functions from SoundManager.				
		public float lowPitchRange = .95f;				//The lowest a sound effect will be randomly pitched.
		public float highPitchRange = 1.05f;			//The highest a sound effect will be randomly pitched.

		public AudioClip sfxPlayerArriveMailbox;
		public AudioClip sfxPlayerArriveMessenger;
		public AudioClip[] sfxPlayerDeath;
		public AudioClip[] sfxBoom;
		public AudioClip[] sfxDamage;
		public AudioClip sfxElectric;

		private float last_explosion_time = 0f;

		void Awake ()
		{
			//Check if there is already an instance of SoundManager
			if (instance == null)
				//if not, set it to this.
				instance = this;
			//If instance already exists:
			else if (instance != this)
				//Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
				Destroy (gameObject);
			//Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
			//DontDestroyOnLoad (gameObject);
			last_explosion_time = 0f;
		}

		void OnEnable()
		{
			EventManager.StartListening ("Player1ArriveMailbox", OnPlayerArriveMailbox);
			EventManager.StartListening ("Player2ArriveMailbox", OnPlayerArriveMailbox);
			EventManager.StartListening ("Player1ArriveMessenger", OnPlayerArriveMessenger);
			EventManager.StartListening ("Player2ArriveMessenger", OnPlayerArriveMessenger);
			EventManager.StartListening ("Player1Death", OnPlayerDeath);
			EventManager.StartListening ("Player2Death", OnPlayerDeath);
			EventManager.StartListening ("Boom", OnBoom);
			EventManager.StartListening ("Damage", OnDamage);
			EventManager.StartListening ("Electric", OnElectric);
		}

		void OnDisable()
		{
			EventManager.StopListening ("Player1ArriveMailbox", OnPlayerArriveMailbox);
			EventManager.StopListening ("Player2ArriveMailbox", OnPlayerArriveMailbox);
			EventManager.StopListening ("Player1ArriveMessenger", OnPlayerArriveMessenger);
			EventManager.StopListening ("Player2ArriveMessenger", OnPlayerArriveMessenger);
			EventManager.StopListening ("Player1Death", OnPlayerDeath);
			EventManager.StopListening ("Player2Death", OnPlayerDeath);
			EventManager.StopListening ("Boom", OnBoom);
			EventManager.StopListening ("Damage", OnDamage);
			EventManager.StopListening ("Electric", OnElectric);
		}

		void OnPlayerArriveMailbox()
		{
			this.PlaySingle (1.0f, this.sfxPlayerArriveMailbox);
		}

		void OnPlayerArriveMessenger()
		{
			this.PlaySingle (1.0f, this.sfxPlayerArriveMessenger);
		}

		void OnPlayerDeath()
		{
			this.RandomizeSfxNormalPitch (1.0f, this.sfxPlayerDeath);
		}

		void OnBoom()
		{
			if (last_explosion_time == 0f) {
				last_explosion_time = Time.time;
			} else if(Time.time - last_explosion_time < 0.2f){
				return;
			}
			this.RandomizeSfx (0.2f, this.sfxBoom);
		}

		void OnDamage()
		{
			this.RandomizeSfx (1.0f, this.sfxDamage);
		}

		void OnElectric()
		{
			this.PlaySingle (1.0f, this.sfxElectric);
		}
		
		//Used to play single sound clips.
		public void PlaySingle(float volume, AudioClip clip)
		{
			//Set the clip of our efxSource audio source to the clip passed in as a parameter.
			efxSource.clip = clip;

			efxSource.volume = volume;
			
			//Play the clip.
			efxSource.Play ();
		}

		public void RandomizeSfxNormalPitch(float volume, params AudioClip[] clips)
		{
			//Generate a random number between 0 and the length of our array of clips passed in.
			int randomIndex = Random.Range(0, clips.Length);

			//Set the clip to the clip at our randomly chosen index.
			efxSource.clip = clips[randomIndex];

			efxSource.volume = volume;

			//Play the clip.
			efxSource.Play();
		}
		
		//RandomizeSfx chooses randomly between various audio clips and slightly changes their pitch.
		public void RandomizeSfx (float volume, params AudioClip[] clips)
		{
			//Generate a random number between 0 and the length of our array of clips passed in.
			int randomIndex = Random.Range(0, clips.Length);
			
			//Choose a random pitch to play back our clip at between our high and low pitch ranges.
			float randomPitch = Random.Range(lowPitchRange, highPitchRange);
			
			//Set the pitch of the audio source to the randomly chosen pitch.
			efxSource.pitch = randomPitch;
			
			//Set the clip to the clip at our randomly chosen index.
			efxSource.clip = clips[randomIndex];

			efxSource.volume = volume;

			//Play the clip.
			efxSource.Play();
		}
	}
}
