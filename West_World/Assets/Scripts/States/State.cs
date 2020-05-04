using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class State <T>: MonoBehaviour
{
    /// <summary>
    /// 状态类名称
    /// </summary>
    public enum StateName {NULL, VisitBankAndDepositGold, GoHomeAndSleepTilRested, QuenchThirst, EnterMineAndDigForNugget, GoBathroom, CleanTheHouse, EatStew, CookStew };
    /// <summary>
    /// 状态
    /// </summary>
    public virtual StateName stateName
    {
        get
        {
            return StateName.NULL;
        }
    }
    /// <summary>
    /// 当状态进入时执行
    /// </summary>
    /// <param name="miner"></param>
    public virtual void Enter(T miner) { }
    /// <summary>
    /// 每一步都用此步骤被矿工更新函数调用
    /// </summary>
    /// <param name="miner"></param>
    public virtual void Execute(T miner) { }
    /// <summary>
    /// 当状态退出时执行
    /// </summary>
    /// <param name="miner"></param>
    public virtual void Exit(T miner) { }
    public virtual bool OnMessage(T entity, Telegram msg)
    {
        Debug.Log("OnMessage State");
        return false;           
    }
}
