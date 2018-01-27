using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Soldier_Shoot1 : Enemy {
	public GameObject prefabBullet;
	public float rateScale = 1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
	}



	//
	public override void Move(){
		Vector2 tempPos = transform.position;
		//对tempPos进行更新操作
		if (this.direction == EnemyDirection.LEFT_TO_RIGHT) {
			tempPos.x += Time.deltaTime * this.speed;
		} else {
			tempPos.x -= Time.deltaTime * this.speed;
		}
	}
		
}
