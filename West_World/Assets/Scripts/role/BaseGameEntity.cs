using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGameEntity : MonoBehaviour
{
    /// <summary>
    /// 每个实体具有一个唯一的识别数字
    /// </summary>
    public int m_ID;
    /// <summary>
    /// 下一个有效的ID，每次BaseGameEntity被实例化时更
    /// </summary>
    private static int m_NextValidID = 0;
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
    public virtual void Awake()
    {
        m_ID = m_NextValidID;
        m_NextValidID++;
        EntityManager.RegisterEntity(this);
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
        Vector3 vector3 = new Vector3();
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
    public void GoTo(Vector3 destination)
    {
        path = grid.GetComponent<Grid>().FindWay(transform.position, destination);
    }
    public virtual bool HandleMessage(Telegram msg)
    {
        return false;
    }
}
