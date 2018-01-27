using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//坦克类型的敌人
public class Enemy_Tank : Enemy {
	

	// Use this for initialization
	void Start () {
		//this.speed = 1f;
		//获取Animator组件并且设置left_to_right = true/false
		if (this.direction == EnemyDirection.LEFT_TO_RIGHT){
			print ("l2r!");
			this.GetComponent<Animator>().SetBool("left_to_right", true);
		} else {
			print ("r2l!");
			this.GetComponent<Animator>().SetBool("left_to_right", false);
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
