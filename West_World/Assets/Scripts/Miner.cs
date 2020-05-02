using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miner : BaseGameEntity
{
    public GameObject stateMachine;
    /// <summary>
    /// Mine的StateMachine实例
    /// </summary>
    public StateMachine m_StateMachine;
    /// <summary>
    /// 位置信息
    /// </summary>
    public Node.Location_Type m_Location;
    /// <summary>
    /// 矿工包中装的金块(0~10)
    /// </summary>
    public int m_GoldCarried = 0;
    /// <summary>
    /// 矿工在银行存的钱
    /// </summary>
    public int m_MoneyInBank = 0;
    /// <summary>
    /// 矿工的口渴值(0~10)
    /// </summary>
    public int m_Thirst = 0;
    /// <summary>
    /// 矿工的疲劳值(0~10)
    /// </summary>
    public int m_Fatigue = 0;
    /// <summary>
    /// 格子
    /// </summary>
    public GameObject grid;
    /// <summary>
    /// 每帧运动距离
    /// </summary>
    public float speed = 0.2f;
    /// <summary>
    /// 路径
    /// </summary>
    public List<Vector3> path = new List<Vector3>();
    private void Awake()
    {
        m_StateMachine = stateMachine.GetComponent<StateMachine>();
    }
    private void Start()
    {
        m_ID = 0;
        transform.SetPositionAndRotation(grid.GetComponent<Grid>().objectInf[(int)Node.Location_Type.Home][0], Quaternion.identity) ;
    }
    private void Update()
    {
        if (path.Count != 0)
        {
            if (Mathf.Abs(transform.position.x - path[path.Count - 1].x) > 0.001 || Mathf.Abs(transform.position.y - path[path.Count - 1].y) > 0.001)
            {
                Vector2 temp = Vector2.MoveTowards(transform.position, path[path.Count - 1], speed);
                GetComponent<Rigidbody2D>().MovePosition(temp);
            }
            else
            {
                path.RemoveAt(path.Count - 1);
            }
        }
        GetComponent<Animator>().SetInteger("path", path.Count);
    }
    public void GoTo(Node.Location_Type destinaton)
    {
        Vector3 vector3 = new Vector3() ;
        float distance = 1000;
        for (int i = 0; i < grid.GetComponent<Grid>().objectInf[(int)destinaton].Count; i++)
        {
            if (Vector3.Distance(transform.position, grid.GetComponent<Grid>().objectInf[(int)destinaton][i]) < distance) 
            {
                vector3 = grid.GetComponent<Grid>().objectInf[(int)destinaton][i];
                distance = Vector3.Distance(transform.position, grid.GetComponent<Grid>().objectInf[(int)destinaton][i]);
            }
        }
        path = grid.GetComponent<Grid>().FindWay(transform.position, vector3);
    }
}
