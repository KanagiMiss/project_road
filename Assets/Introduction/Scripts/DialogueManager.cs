using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace GOAT{
	public class DialogueManager : MonoBehaviour {

		public Text titleText;
		public Text dialogueText;

		public Animator animator;

		private string[] titles;
		private Queue<string> sentences;

		// Use this for initialization
		void Awake () {
			titles = new string[2];
			sentences = new Queue<string>();

//			animator.SetBool("IsMC", true);
			//Debug.Log (animator.GetBool("IsMC"));

			titles[0] = "司令部";
			titles[1] = "前线";

			sentences.Clear();

			sentences.Enqueue ("Hi!");
			sentences.Enqueue ("Hello!");
			sentences.Enqueue ("How are you?");
			sentences.Enqueue ("I am fine, thank you, and you?");
			sentences.Enqueue ("I am very well!");
		}

		public void Start ()
		{
			DisplayNextSentence();
		}

		void Update(){
			
			if(Input.GetKeyDown(KeyCode.Space)){
//				Debug.Log (transform.position.x);
//				Debug.Log (transform.position.y);
//				if (transform.position.x != 302) {
//					transform.position = new Vector3(302, 160, 0);
//				} else {
//					transform.position = new Vector3(21, 291, 0);
//				}
				titleText.text = "";
				dialogueText.text = "";
				if (animator.GetBool ("IsMC")) {
					animator.SetBool("IsMC", false);
				} else {
					animator.SetBool("IsMC", true);
				}

				DisplayNextSentence ();
			}

			if (Input.GetKeyDown (KeyCode.Escape)) {
				Debug.Log ("escape");
				EndDialogue ();
			}
		}

		public void DisplayNextSentence ()
		{
			if (sentences.Count == 0)
			{
				EndDialogue();
				return;
			}
			//titleText.text = titles[(5-sentences.Count)%2];
			string sentence = sentences.Dequeue();

			StopAllCoroutines();
			StartCoroutine(TypeSentence(titles[(5-sentences.Count)%2], sentence));
//			animator.SetBool("IsMC", false);
		}

		IEnumerator TypeSentence (string title, string sentence)
		{
			//lsm
			yield return new WaitForSeconds(0.1f);
			titleText.text = title;
			dialogueText.text = "";
			foreach (char letter in sentence.ToCharArray())
			{
				dialogueText.text += letter;
				yield return null;
			}
		}

		void EndDialogue()
		{
			SceneManager.LoadScene ("Level 1");
		}

	}

}