using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//枚举一些武器
public enum DirectionType {
	none,
	left,
	right,
	up,
	down
}

public class player : MonoBehaviour {
	public float speed = 3; //玩家的移动速度
	public DirectionType facing = DirectionType.down;

	//private DirectionType last_direction = facing;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//获取玩家的方向键
		float xAxis = Input.GetAxis ("Horizontal");
		float yAxis = Input.GetAxis ("Vertical");

		Vector2 player_pos = transform.position;
		if (facing == DirectionType.left || facing == DirectionType.right) {
			//玩家上一轮是左右移动的，那么优先左右键
			if (xAxis != 0) {
				player_pos.x += xAxis * speed * Time.deltaTime;
				if (xAxis > 0) {
					facing = DirectionType.right;
				} else {
					facing = DirectionType.left;
				}
			} else if (yAxis != 0) {
				player_pos.y += yAxis * speed * Time.deltaTime;
				if (yAxis > 0) {
					facing = DirectionType.down;
				} else {
					facing = DirectionType.up;
				}
			}
		} else {
			//玩家上一轮是上下移动的，那么优先上下键
			if (yAxis != 0) {
				player_pos.y += yAxis * speed * Time.deltaTime;
				if (yAxis > 0) {
					facing = DirectionType.down;
				} else {
					facing = DirectionType.up;
				}
			} else if (xAxis != 0) {
				player_pos.x += xAxis * speed * Time.deltaTime;
				if (xAxis > 0) {
					facing = DirectionType.right;
				} else {
					facing = DirectionType.left;
				}
			}
		}


		transform.position = player_pos;
	}

	//伤害玩家
	public void DoDamage(float value){
		print ("DoManage().");
	}

	// 碰撞了电网
	void OnCollisionEnter2D(Collision2D coll){
		//print ("player OnCollisionEnter()");
	}
}
