using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAT{

	public class MCTrigger : MonoBehaviour {

		public Dialogue dialogue;

		public void TriggerDialogue ()
		{
			FindObjectOfType<MCManager>().StartDialogue(dialogue);
		}

	}
}
