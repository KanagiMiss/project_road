using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//比较复杂的移动炮台
public class Enemy_Cannon : Enemy {

	public GameObject prefabMissile;

	private bool has_launched = false;  //一辆Cannon只能停下来开火一次
	private float launch_point_x;    //开火点
	//随机先选取一个点作为开火点，然后开到那里以后就停下来，随机选取一个方向进行开火！

	// Use this for initialization
	void Start () {
		//获取Animator组件并且设置left_to_right = true/false
		if (this.direction == EnemyDirection.LEFT_TO_RIGHT){
			this.GetComponent<Animator>().SetBool("left_to_right", true);
		} else {
			this.GetComponent<Animator>().SetBool("left_to_right", false);
		}

		//0~18是x轴的范围
		launch_point_x = 1.0f * Random.Range (4, 14);
	}

	void Update() {
		Move ();  //不同的敌人有不同的Move()函数
	}


	/*
	 * Cannon Move:
	 * 从一侧移动并且随机停下，停下后随机射出导弹。
	 */
	public override void Move(){
		//print ("Enemy_Cannon Move().");
		Vector2 tempPos = transform.position;
		//首先判断是否已经到达了开火点
		if ( !has_launched ) {
			if ((this.direction == EnemyDirection.LEFT_TO_RIGHT && 
				this.transform.position.x >= launch_point_x)  || 
				(this.direction == EnemyDirection.RIGHT_TO_LEFT &&
					this.transform.position.x <= launch_point_x)) {
				//这个时候已经到了开火点
				has_launched = true;
				//Fire!!!
				print("Fire!!!");
				Fire ();
			}
		} 


		//对tempPos进行更新操作
		if (this.direction == EnemyDirection.LEFT_TO_RIGHT) {
			tempPos.x += Time.deltaTime * this.speed;
		} else {
			tempPos.x -= Time.deltaTime * this.speed;
		}

		transform.position = tempPos;
		base.Move ();
	}


	//开火
	void Fire() {
		//随机选取一个射程进行开火，射程只有1~2之间
		float launch_distance = Random.Range (1.0f, 2.0f);

		//随机选取开火次数，有1次
		int launch_times = Random.Range (1, 2);

		//开火！
		for (int i = 0; i < launch_times; i++) {
			//随机选取一个方向进行开火
			int direction = Random.Range (0, 4);
			MissileDirection launch_direction = MissileDirection.UP;

			//生成导弹
			GameObject go = Instantiate (prefabMissile) as GameObject;
			switch (direction) {
			//旋转战车车身
			case 0:   //上
				launch_direction = MissileDirection.UP;
				break;
			case 1:   //下
				launch_direction = MissileDirection.DOWN;
				break;
			case 2:   //左
				launch_direction = MissileDirection.LEFT;
				break;
			case 3:   //右
				launch_direction = MissileDirection.RIGHT;
				break;
			}
			//真的发射！
			go.GetComponent<Missile> ().Throw (transform.position, launch_direction, launch_distance);
		}
	}



}
