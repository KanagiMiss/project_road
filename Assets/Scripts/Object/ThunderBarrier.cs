using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAT{


	//电击网: 
	//   一个用于间歇电击玩家的BoxCollider2D，初始为disabled
	//每隔5s让自己的更大的collider2D组件enabled，然后再disabled

	public class ThunderBarrier : MonoBehaviour {
		
	    BoxCollider2D electricCollider;  //电击网的碰撞体
		Vector2 transform_size;   //记录原本的电网的size
		public bool is_shining = false;  //正在放电吗


		void Awake() {
			electricCollider = this.GetComponent<BoxCollider2D> ();
		}

		// Use this for initialization
		void Start () {
			
			// 4s以后调用Shine函数
			Invoke ("Shine", 4);
		}


		// 隔离带放电了
		void Shine() {
			print ("Shine().");

			// 把碰撞体积范围扩大，粒子效果释放
			transform_size = transform.localScale;
			transform.localScale = new Vector2(transform.localScale.x, 5);
			is_shining = true;

			// 持续个2s的效果以后，收回粒子效果，缩小碰撞体积
			StartCoroutine ("ResumeShine");
		}

		// 放电结束，恢复正常的隔离带
		IEnumerator ResumeShine () {
			yield return new WaitForSeconds (2);
			is_shining = false;
			transform.localScale = transform_size;

			// 4秒以后调用Shine()，形成循环
			Invoke ("Shine", 4);
		}


		// Update is called once per frame
		void Update () {
			
		}

		// 有物体碰撞了电网
		void OnCollisionEnter2D(Collision2D coll){
			//碰到了中间的界限，此时可以触发一些呼喊的动画以及音效
			if (is_shining) {
				GameObject other = coll.gameObject;
				switch (other.tag) {
				case "Player":  //碰到了Hero
					//Hero跪了
					//other.GetComponent<player>().DoDamage(0);  //Hero伤血逻辑
					print("YOOO");
					other.GetComponent<Player>().ApplyDamage(5);
					break;
				}
			} else {
				print ("OnCollisionEnter2D() : Cannot pass through!");
			}

		}
	}

}