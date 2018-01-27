using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAT{

	public enum EnemyDirection {
		none,
		LEFT_TO_RIGHT,
		RIGHT_TO_LEFT
	}

	//敌人基类
	public class Enemy : MonoBehaviour {
		public float speed = 3f;  //运动速度，m/s
		public int dmg = 5;
		public EnemyDirection direction = EnemyDirection.LEFT_TO_RIGHT;  //坦克从左到右移动？
		public bool __________________;


		public Color[] originalColors;
		public Bounds bounds;  //本对象及其子对象的边界框
		public Vector3 boundsCenterOffset;   //边界中心

		void Awake(){
			
			if (this.transform.position.x < 8) {
				direction = EnemyDirection.LEFT_TO_RIGHT;
			} else {
				direction = EnemyDirection.RIGHT_TO_LEFT;
			}
			//0f后开始，没2f调用一次CheckOffScreen
			//InvokeRepeating ("CheckOffScreen", 0f , 2f);
		}

		// Use this for initialization
		void Start () {

		}

		// Update is called once per frame
		void Update () {
		}


		//设置敌人的从左向右还是从右向左
		public void SetEnemyMovingDirection(EnemyDirection enemy_direction){
			this.direction = enemy_direction;
		}


		//敌机要逐渐向屏幕底部移动，只处理y轴的移动
		public virtual void Move(){
			//print ("Enemy Move().");
		}

		public Vector2 pos {
			get{ 
				return this.transform.position;
			}
			set{
				this.transform.position = value;
			}
		}

		//检查敌人超出屏幕范围，超出则销毁它
		void CheckOffScreen(){
			if (bounds.size == Vector3.zero) {
				//bounds还未初始化，初始化为敌机GameObject的所有组件边框之并集
				bounds = Utils.CombineBoundsOfChildren(this.gameObject);
				boundsCenterOffset = bounds.center - transform.position;
			}
			//每次根据当前位置更新边界框
			bounds.center = transform.position + boundsCenterOffset;
			//检查边界是否完全位于屏幕之外
			Vector3 off = Utils.ScreenBoundsCheck(bounds, BoundsTest.offScreen);
			if (off != Vector3.zero) {
				//如果敌人已经已经超出了屏幕的范围，销毁这个敌人
				Destroy (this.gameObject);
			}
		}

		//敌人和coll进行了碰撞
		void OnCollisionEnter2D (Collision2D coll) {
			GameObject other = coll.gameObject;
			switch (other.tag) {
			case "Player":  //碰到了Hero
				//Hero跪了
				//other.GetComponent<player>().DoDamage(0);  //Hero伤血逻辑
				print("YOOO");
				other.GetComponent<Player>().ApplyDamage(dmg);
				break;
			}
		}
	}

}
