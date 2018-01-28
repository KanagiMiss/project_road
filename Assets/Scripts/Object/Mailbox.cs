using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAT{

	public class Mailbox : MonoBehaviour {

		private static bool player1Arrived = false, player2Arrived = false;
		private static GameObject go_player1, go_player2;

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

				//此时交换player1和player2头上的信件
				if (go_player1 != null && go_player2 != null) {
					print("Exchange the envelops!");
					Transform tf_envelop_1 = go_player1.transform.GetChild (0);
					Transform tf_envelop_2 = go_player2.transform.GetChild (0);
					go_player1.transform.DetachChildren ();
					go_player2.transform.DetachChildren ();
					tf_envelop_1.parent = go_player2.transform;
					tf_envelop_2.parent = go_player1.transform;
					//
					tf_envelop_1.position = new Vector3(go_player2.transform.position.x,go_player2.transform.position.y+0.8f,0);
					tf_envelop_2.position = new Vector3(go_player1.transform.position.x,go_player1.transform.position.y+0.8f,0);
				} 
			}
		}

		void OnTriggerEnter2D(Collider2D coll){
			if (coll.gameObject.tag == "Player") {
				if(coll.gameObject.GetComponent<Player>().playerId == 1){
					go_player1 = coll.gameObject;
					player1Arrived = true;
				}
				else if(coll.gameObject.GetComponent<Player>().playerId == 2){
					go_player2 = coll.gameObject;
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
