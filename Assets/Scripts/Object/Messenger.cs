using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAT{
	//邮差的逻辑
	public class Messenger : MonoBehaviour {
		private Vector3 mailing_destination = Vector3.zero;  //送信的目的地
		public GameObject prefabEnvelop1, prefabEnvelop2;  //inspector
		public bool success_to_send_mail = false; //已经成功地将信交到了player的手中
		public bool success_to_reach_dest = false;  //已经到达了目的地
		public bool flipped = false;  //邮差已经被翻转过了
		public float move_speed = 20f;
		//1.  f+f ：还在去的路上（left_to_right）
		//2.  f+t ：已经到达了目的地，在等待玩家（idle）
		//3.  t+t ：已经到达并且已经成功把信送达，说明此时已经在回去的路上了（right_to_left）


		//设置邮差的目的地
		public void SetMailingDestination(Vector3 des){
			this.mailing_destination = des;
		}



		//生成一个邮差的时候同时给他携带一个邮件
		public void SetMailType(int type) {
			GameObject go_envelop = this.transform.GetChild (0).gameObject;
			if (type == 1) {
				//获取信封gameObject
				go_envelop.GetComponent<SpriteRenderer> ().color = new Color (255,0,0,200);
			} else {
				go_envelop.GetComponent<SpriteRenderer> ().color = new Color (255,255,0,200);
			}

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
						this.GetComponent<Animator>().SetBool("running", false);
					}
				} else {
					//case 2. idle

				}
			} else {
				//case 3. right_to_left
				print("messenger move to left!");
				if (!flipped) {
					flipped = true;
					this.GetComponent<Animator>().SetBool("running", true);
					Vector3 temp_scale = transform.localScale;
					temp_scale.x *= -1;
					transform.localScale = temp_scale;
				}
				Vector3 temp_pos = transform.position;
				temp_pos.x -= move_speed * Time.deltaTime;
				transform.position = temp_pos;
				if (temp_pos.x <= 0) {
					print ("messenger could destroy.");
					if (this.transform.childCount > 0) {
						Destroy (this.transform.GetChild (0));
					}
					Destroy (this.gameObject);
				}
			}
		}


		void OnTriggerEnter2D(Collider2D coll){
			//print (coll.gameObject.name);
			if (coll.gameObject.tag == "Player") {
				if (success_to_send_mail == true)
					return;
				//设置messenger的animator
				success_to_send_mail = true;
				//把邮差头上的envelop gameobject的parent设置为player
				if (transform.childCount > 0) {
					Transform my_envelop = this.transform.GetChild (0);
					//player头上也有信封
					if (coll.transform.childCount > 0) {
						//先把邮差信封拿掉，在把player的信封给邮差
						this.transform.DetachChildren ();
						coll.transform.GetChild (0).parent = this.transform;
						my_envelop.parent = coll.transform;
					} else {
						//直接把自己头上的信封给player
						this.transform.GetChild (0).parent = coll.transform;
					}
				} else {
					//邮差头上没有信件
					//本游戏设定没有这种情况
				}
					

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
