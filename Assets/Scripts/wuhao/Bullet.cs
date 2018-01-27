using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BulletType {
	none,
	NORMAL,
	SERIOUS
}


//所有的Missile都用这个脚本，控制爆炸逻辑
public class Bullet : MonoBehaviour {

	public BulletType bullet_type = BulletType.NORMAL;

	//底下的不是用户可以在Inspector面板中选择的。
	CircleCollider2D explosion_circle;

	void Awake() {
		
	}

	// Use this for initialization
	void Start () {
		explosion_circle = this.GetComponent<CircleCollider2D> ();
		explosion_circle.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//飞机只需要调用Throw()以后就什么都不用管了
	public void Throw (Vector2 pos){
		//设置导弹扔下的初始位置
		transform.position = pos;
		//过explosion_delay的时间以后爆炸
	}


	//Hero被爆到了
	void OnTriggerEnter2D(Collider2D other) {
		print ("Missile : OnTriggerEnter2D().");
		GameObject go = other.gameObject;
		if (go != null && go.tag == "Hero") {
			//如果碰撞到的是玩家，那么干吧
			//other.GetComponent<player>().DoDamage(0);  //Hero伤血逻辑
		}
	}

}
