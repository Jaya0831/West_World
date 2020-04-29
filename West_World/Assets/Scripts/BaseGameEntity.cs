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
}
