using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAT{

	//坦克类型的敌人
	public class Enemy_Tank : Enemy {

		// Use this for initialization
		void Start () {
			if (this.direction == EnemyDirection.LEFT_TO_RIGHT) {
				this.GetComponent<Animator> ().SetBool ("left_to_right", true);
			} else {
				this.GetComponent<Animator> ().SetBool ("left_to_right", false);
			}
		}

		void Update() {
			Move ();  //不同的敌人有不同的Move()函数
		}


		/*
		 * Tank Move:
		 * 从屏幕的一端移动到另一边，匀速地运动.
		 * 
		 */
		public override void Move(){
			//print ("Enemy_Tank Move().");
			Vector2 tempPos = transform.position;
			//对tempPos进行更新操作
			if (this.direction == EnemyDirection.LEFT_TO_RIGHT) {
				tempPos.x += Time.deltaTime * this.speed;
			} else {
				tempPos.x -= Time.deltaTime * this.speed;
			}

			transform.position = tempPos;
			base.Move ();
		}
	}
}