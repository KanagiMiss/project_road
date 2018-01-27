using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//飞机类型的敌人
public class Enemy_Plane_1 : Enemy {
	public GameObject prefabMissile;
	public float throwRate = 0.5f;  //飞机扔炸弹的频率


	// Use this for initialization
	void Start () {
		//调整飞机头的朝向
		if (direction == EnemyDirection.LEFT_TO_RIGHT) {
			Vector3 old_scale = this.transform.localScale;
			old_scale.x = -1;
			this.transform.localScale = old_scale;
		}
		InvokeRepeating ("ThrowMissile", 0f, throwRate);
	}

	void Update() {
		Move ();
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
