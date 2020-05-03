using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalState : State<Miner>
{
    public override void Execute(Miner miner)
    {
        if (miner.m_Thirst > 7 || miner.m_Fatigue > 7 || miner.m_GoldCarried == 10) 
        {
            if (miner.m_StateMachine.m_CurrentState.stateName != StateName.VisitBankAndDepositGold)
            {
                if (miner.m_GoldCarried != 0)
                {
                    miner.m_StateMachine.ChangeState(new VisitBankAndDepositGold());
                }
                else if (miner.m_Thirst > 7 && miner.m_StateMachine.m_CurrentState.stateName != StateName.GoHomeAndSleepTilRested && miner.m_StateMachine.m_CurrentState.stateName != StateName.QuenchThirst)
                {
                    miner.m_StateMachine.ChangeState(new QuenchThirst());
                }
                else if (miner.m_Fatigue > 7 && miner.m_StateMachine.m_CurrentState.stateName != StateName.QuenchThirst && miner.m_StateMachine.m_CurrentState.stateName != StateName.GoHomeAndSleepTilRested) 
                {
                    miner.m_StateMachine.ChangeState(new GoHomeAndSleepTilRested());
                }
            }  
        }
    }
}
