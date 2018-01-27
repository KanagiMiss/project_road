using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MissileType {
	none,
	NORMAL,
	SERIOUS
}

public enum MissileDirection {
	UP,
	DOWN,
	LEFT,
	RIGHT
}


//所有的Missile都用这个脚本，控制爆炸逻辑
public class Missile : MonoBehaviour {

	public MissileType missile_type = MissileType.NORMAL;
	public float rest_distance;  //炮弹还要飞多远
	private MissileDirection fly_direction = MissileDirection.UP;  //炮弹飞行的方向
	private bool reached = false;  //炮弹已经到达爆炸点
	private bool explosed = false;

	[SerializeField] private float explosion_delay = 1f;  //扔下以后多久爆炸
	[SerializeField] private float explosion_duration = 1f;  //爆炸持续多久
	[SerializeField] private float fly_speed = 1f;   //炮弹飞行的速度

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
		if (!reached) {
			Fly ();
		}
	}

	//飞机只需要调用Throw()以后就什么都不用管了
	/*public void Throw (Vector2 pos){
		//设置导弹扔下的初始位置
		transform.position = pos;
		this.source = pos;
		this.destination = pos;
		//过explosion_delay的时间以后爆炸
		Invoke("Explosion", explosion_delay);
	}*/


	//把导弹从source反射到destination，然后爆炸
	public void Throw(Vector2 source, MissileDirection direction, float distance) {
		print ("Cannon Throw Missile!");
		transform.position = source;
		this.fly_direction = direction;
		this.rest_distance = distance;
	}

	//更新炮弹的坐标，只能上下左右飞
	void Fly(){
		Vector2 temp = transform.position;
		switch (this.fly_direction) {
		case MissileDirection.UP:
			temp.y += Time.deltaTime * fly_speed;
			break;
		case MissileDirection.DOWN:
			temp.y -= Time.deltaTime * fly_speed;
			break;
		case MissileDirection.LEFT:
			temp.x -= Time.deltaTime * fly_speed;
			break;
		case MissileDirection.RIGHT:
			temp.x += Time.deltaTime * fly_speed;
			break;
		}
		transform.position = temp;

		rest_distance -= Time.deltaTime * fly_speed;
		if (rest_distance <= 0) {
			reached = true;
			Invoke("Explosion", explosion_delay);
		}

	}

	//爆炸函数
	void Explosion(){
		//切换到Missile的爆炸动画

		print ("Explosion!");
		//把collider打开
		explosion_circle.enabled = true;   
		StartCoroutine (DestroyMissile());
	}

	//延迟销毁导弹
	IEnumerator DestroyMissile() {
		yield return new WaitForSeconds (explosion_duration);
		print ("DestroyMissile!");
		Destroy (this.gameObject); //炸弹已经成了废渣
	}


	//爆炸了！
	/*void OnCollisionEnter2D(Collision2D coll){
		print ("Missile : OnCollisionEnter2D().");
		GameObject other = coll.gameObject;
		switch (other.tag) {
		case "Hero":  //碰到了Hero
			other.GetComponent<player>().DoDamage(0);  //Hero伤血逻辑
			break;
		}
	}*/


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
