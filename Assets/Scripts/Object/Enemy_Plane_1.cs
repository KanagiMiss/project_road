using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAT{

	//飞机类型的敌人
	public class Enemy_Plane_1 : Enemy {
		public GameObject prefabMissile;
		public float throwRate = 0.5f;  //飞机扔炸弹的频率
		//随机选取一小段值，每次扔了炸弹以后，就立刻计算下一次投弹的位置
		public float throwDistance = 8;
		private float rest_distance = 0;


		// Use this for initialization
		void Start () {
			//调整飞机头的朝向
			if (direction == EnemyDirection.LEFT_TO_RIGHT) {
				Vector3 old_scale = this.transform.localScale;
				old_scale.x = -1;
				this.transform.localScale = old_scale;
			}
			//InvokeRepeating ("ThrowMissile", 0f, 1.0f/throwRate);
		}

		void Update() {
			Move ();
			if (rest_distance <= 0) {
				ThrowMissile ();
				//此时需要随机选取一个0~throwDistance的值作为新的rest_distance
				rest_distance = Random.Range (throwDistance/2.0f, throwDistance) ;
			}
			rest_distance -= Time.deltaTime * this.speed;
		}


		/*
		 * Plane Move:
		 * 从屏幕的一端移动到另一边，匀速地运动.
		 * 投下一些导弹
		 */
		public override void Move(){
			//print ("Enemy_Plane Move().");
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




		//投弹
		public void ThrowMissile() {
			//生成导弹
			GameObject go = Instantiate (prefabMissile) as GameObject;
			//投弹
			go.GetComponent<Missile> ().Throw (transform.position);
		}
	}
}
