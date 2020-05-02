using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class State : MonoBehaviour
{
    /// <summary>
    /// 状态类名称
    /// </summary>
    public enum StateName {NULL, VisitBankAndDepositGold, GoHomeAndSleepTilRested, QuenchThirst, EnterMineAndDigForNugget };
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
    public virtual void Enter(Miner miner) { }
    /// <summary>
    /// 每一步都用此步骤被矿工更新函数调用
    /// </summary>
    /// <param name="miner"></param>
    public virtual void Execute(Miner miner) { }
    /// <summary>
    /// 当状态退出时执行
    /// </summary>
    /// <param name="miner"></param>
    public virtual void Exit(Miner miner) { }


}
