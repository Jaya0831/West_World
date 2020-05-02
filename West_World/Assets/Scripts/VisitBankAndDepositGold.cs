using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitBankAndDepositGold : State
{
    public new StateName stateName = StateName.VisitBankAndDepositGold;
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
            //if (miner.m_Thirst > 7)
            //{
            //    miner.m_StateMachine.ChangeState(new QuenchThirst());
            //}
            //else if (miner.m_Fatigue > 7) 
            //{
            //    miner.m_StateMachine.ChangeState(new GoHomeAndSleepTilRested());
            //}
            //else 
            //{
                //miner.m_StateMachine.ChangeState(new EnterMineAndDigForNugget());
            //}
        }
    }
    public override void Exit(Miner miner)
    {
        base.Exit(miner);
    }
}
