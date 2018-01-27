using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//生成敌人
public class Main : MonoBehaviour {
	static public Main S;
	//public GameObject[] prefabEnemies;  //所有的敌人类型
	public float enemySpawnPerSecond = 0.3f;
	public float enemySpawnRate = 1.5f;
	public float xMin = 0;
	public float xMax = 0;
	public float yMin = 5f;
	public float yMax = 10f;
	public int[] ys = {1,2,3,4};

	void Awake(){
		S = this;
		enemySpawnRate = 1f / enemySpawnPerSecond;
		//Invoke ("SpawnEnemy", enemySpawnRate);  //2s以后调用SpawnEnemy()

	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//生成敌人，这里控制着各种敌人的生成，每一种生成需要指定是从左往右还是从右往左
	/*public void SpawnEnemy () {
		int ndx = Random.Range (0, prefabEnemies.Length);
		GameObject go = Instantiate (prefabEnemies [ndx]) as GameObject;
		go.GetComponent<Enemy> ().SetEnemyMovingDirection (EnemyDirection.LEFT_TO_RIGHT);
		//敌机从屏幕的最上方出现，x的位置随机
		Vector2 pos = Vector2.zero;

		pos.x = -10;
		pos.y = ys[Random.Range (0, 3)];
		go.transform.position = pos;
		//这么做的目的是为了游戏进行过程中通过简单调整enemySpawnRate，动态地调整Invoke的时间差
		Invoke ("SpawnEnemy", enemySpawnRate);  //2s以后调用SpawnEnemy()
	}*/
}
