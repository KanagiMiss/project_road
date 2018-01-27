using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFTrigger : MonoBehaviour {

	public Dialogue dialogue;

	public void TriggerDialogue ()
	{
		FindObjectOfType<BFManager>().StartDialogue(dialogue);
	}

}
