using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAT{

	public class Messenger : MonoBehaviour {

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		void OnTriggerEnter2D(Collider2D coll){
			//print (coll.gameObject.name);
			if (coll.gameObject.tag == "Player") {
				if(coll.gameObject.GetComponent<Player>().playerId == 1){
					EventManager.TriggerEvent ("Player1ArriveMessenger");
				}
				else if(coll.gameObject.GetComponent<Player>().playerId == 2){
					EventManager.TriggerEvent ("Player2ArriveMessenger");
				}
				Destroy (this.gameObject);
			}
		}
	}

}
