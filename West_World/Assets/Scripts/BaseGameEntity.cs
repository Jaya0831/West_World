using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGameEntity : MonoBehaviour
{
    /// <summary>
    /// 每个实体具有一个唯一的识别数字
    /// </summary>
    protected int m_ID;
    /// <summary>
    /// 下一个有效的ID，每次BaseGameEntity被实例化时更
    /// </summary>
    private static int m_NextValidID;
    /// <summary>
    /// 在构造函数中调用这个来确认ID被正确设置。（没懂）
    /// </summary>
    /// <param name="val"></param>
    private void SetID(int val) { }
    /// <summary>
    /// 返回ID值
    /// </summary>
    /// <returns></returns>
    public int ID()
    {
        return m_ID;
    }
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
}
