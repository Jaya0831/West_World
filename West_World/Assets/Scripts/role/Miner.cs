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
    public bool hadAteStew = false;
    public override void Awake()
    {
        base.Awake();
        m_StateMachine = stateMachine.GetComponent<StateMachine>();
    }
    private void Start()
    {
        transform.SetPositionAndRotation(grid.GetComponent<Grid>().objectInf[(int)Node.Location_Type.Home][0], Quaternion.identity);

    }
    public override bool HandleMessage(Telegram msg)
    {
        Debug.Log("Miner HandleMessage");
        return m_StateMachine.HandleMessage(msg);
    }
}
