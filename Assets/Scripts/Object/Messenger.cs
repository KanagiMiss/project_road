using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAT{
	//邮差的逻辑
	public class Messenger : MonoBehaviour {
		private Vector3 mailing_destination = Vector3.zero;  //送信的目的地
		public bool success_to_send_mail = false; //已经成功地将信交到了player的手中
		public bool success_to_reach_dest = false;  //已经到达了目的地
		public float move_speed = 20f;
		//1.  f+f ：还在去的路上（left_to_right）
		//2.  f+t ：已经到达了目的地，在等待玩家（idle）
		//3.  t+t ：已经到达并且已经成功把信送达，说明此时已经在回去的路上了（right_to_left）


		//设置邮差的目的地
		public void SetMailingDestination(Vector3 des){
			this.mailing_destination = des;
		}

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			/*if (!success_to_send_mail && mailing_destination == Vector3.zero) {
				return;
			}*/
			//此时邮差已经有了目的地
			if (success_to_send_mail == false) {
				if (success_to_reach_dest == false) {
					//case 1. left_to_right
					Vector3 temp_pos = transform.position;
					temp_pos.x += move_speed * Time.deltaTime;
					transform.position = temp_pos;
					if (temp_pos.x >= mailing_destination.x) {
						print ("messenger reach the destination.");
						//已经到了idle状态
						success_to_reach_dest = true;
						//this.gameObject.GetComponent<Animator> ().SetBool ();
					}
				} else {
					//case 2. idle

				}
			} else {
				//case 3. right_to_left
				print("messenger move to left!");
				Vector3 temp_pos = transform.position;
				temp_pos.x -= move_speed * Time.deltaTime;
				transform.position = temp_pos;
				if (temp_pos.x <= 0) {
					print ("messenger could destroy.");
					Destroy (this.gameObject);
				}
			}
		}


		void OnTriggerEnter2D(Collider2D coll){
			//print (coll.gameObject.name);
			if (coll.gameObject.tag == "Player") {
				if (success_to_send_mail == true)
					return;
				success_to_send_mail = true;
				print ("the player has touched the messenger.");
				if(coll.gameObject.GetComponent<Player>().playerId == 1){
					EventManager.TriggerEvent ("Player1ArriveMessenger");
				}
				else if(coll.gameObject.GetComponent<Player>().playerId == 2){
					EventManager.TriggerEvent ("Player2ArriveMessenger");
				}
			}
		}
	}

}
