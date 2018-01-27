using UnityEngine;

public class Enemy_Sapper : Enemy
{
    public GameObject prefabLandmine;
    public float throwRate = 0.5f; // 工兵埋雷的频率

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        Move();
    }

    /**
     * Sapper Move:
     * 过程中埋雷
     */
    public override void Move()
    {
        base.Move();
    }
}