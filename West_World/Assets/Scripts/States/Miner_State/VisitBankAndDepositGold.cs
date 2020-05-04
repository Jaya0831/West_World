using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitBankAndDepositGold : State<Miner>
{
    public override StateName stateName
    {
        get
        {
            return StateName.VisitBankAndDepositGold;
        }
    }
    public override void Enter(Miner miner)
    {
        miner.GoTo(Node.Location_Type.Bank); 
    }
    public override void Execute(Miner miner)
    {
        if (miner.path.Count == 0) 
        {
            miner.m_MoneyInBank += miner.m_GoldCarried * 100;
            miner.m_GoldCarried = 0;
            miner.m_StateMachine.RevertToPrevious();
        }
    }
    public override void Exit(Miner miner)
    {
        base.Exit(miner);
    }
}
