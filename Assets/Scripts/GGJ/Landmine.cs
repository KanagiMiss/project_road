﻿using System.Collections;
using UnityEngine;

namespace GOAT{

	public class Landmine : MonoBehaviour {
	    [SerializeField] private float explosion_delay = 1f;  //扔下以后多久爆炸
	    [SerializeField] private float explosion_duration = 1f;  //爆炸持续多久

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
	        Invoke("Explosion", explosion_delay);
	    }


	    //爆炸函数
	    void Explosion(){
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
}