using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAT{

	public class Mailbox : MonoBehaviour {

		private static bool player1Arrived = false, player2Arrived = false;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			if (player1Arrived && player2Arrived) {
				EventManager.TriggerEvent ("Player1ArriveMailbox");
				EventManager.TriggerEvent ("Player2ArriveMailbox");
				player1Arrived = false;
				player2Arrived = false;
			}
		}

		void OnTriggerEnter2D(Collider2D coll){
			if (coll.gameObject.tag == "Player") {
				if(coll.gameObject.GetComponent<Player>().playerId == 1){
					player1Arrived = true;
				}
				else if(coll.gameObject.GetComponent<Player>().playerId == 2){
					player2Arrived = true;
				}
			}
		}

		void OnTriggerExit2D(Collider2D coll){
			if (coll.gameObject.tag == "Player") {
				if(coll.gameObject.GetComponent<Player>().playerId == 1){
					player1Arrived = false;
				}
				else if(coll.gameObject.GetComponent<Player>().playerId == 2){
					player2Arrived = false;
				}
			}
		}
	}
		
}
