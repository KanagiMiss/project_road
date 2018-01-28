using System.Collections;
using System.Collections.Generic;
using GOAT;
using UnityEngine;


//所有的Missile都用这个脚本，控制爆炸逻辑
public class Missile_Cannon : MonoBehaviour {

	public MissileType missile_type = MissileType.NORMAL; // 炮弹类型
	public float rest_distance;  //炮弹还要飞多远
	private MissileDirection fly_direction = MissileDirection.DOWN;  //炮弹默认的飞行方向
	private bool reached = false;  //炮弹已经到达爆炸点
	private bool explosed = false;
	private int dmg = 5;

	[SerializeField] private float explosion_delay = 1f;  //扔下以后多久爆炸
	[SerializeField] private float explosion_duration = 1f;  //爆炸持续多久
	[SerializeField] private float fly_speed = 5.0f;   //炮弹飞行的速度

	//底下的不是用户可以在Inspector面板中选择的。
	CircleCollider2D explosion_circle;

	void Awake() {
		
	}

	// Use this for initialization
	void Start () {
		explosion_circle = this.GetComponent<CircleCollider2D> ();
		explosion_circle.enabled = false; // 飞的过程是不会碰撞的，到达后设为true
	}
	
	// Update is called once per frame
	void Update () {
		if (!reached) {
			Fly ();
		}
	}


	//把导弹从source反射到destination，然后爆炸
	public void Throw(Vector2 source, MissileDirection direction, float distance) {
		print ("Cannon Throw Missile!");
		ChangeDirection(direction);
		transform.position = source;
		this.fly_direction = direction;
		this.rest_distance = distance;
		
	}

	//旋转炮弹方向
	private void ChangeDirection(MissileDirection direction)
	{
		//Quaternion temp = transform.rotation.SetEulerAngles(0,0,180);
		switch (direction) {
			case MissileDirection.UP:
				transform.rotation = Quaternion.Euler(0,0,180);
				break;
			case MissileDirection.DOWN:
				break;
			case MissileDirection.LEFT:
				transform.rotation = Quaternion.Euler(0,0,270);
				break;
			case MissileDirection.RIGHT:
				transform.rotation = Quaternion.Euler(0,0,90);
				break;
		}

		print("Missile change direction!");
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
			print("Missile reached!");
			//Invoke("Explosion", explosion_delay);
			Explosion();
		}

	}

	//爆炸函数
	void Explosion(){
		transform.rotation = Quaternion.Euler(0,0,0);
		//切换到Missile的爆炸动画
		this.GetComponent<Animator>().SetBool("reached", true);
		print ("Explosion!");
		EventManager.TriggerEvent ("Boom");
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
	void OnCollisionEnter2D(Collision2D coll){
		GameObject other = coll.gameObject;
		switch (other.tag) {
		case "Player":  //碰到了Hero
			//Hero跪了
			//other.GetComponent<player>().DoDamage(0);  //Hero伤血逻辑
			other.GetComponent<Player>().ApplyDamage(dmg);
			break;
		}
	}

}
