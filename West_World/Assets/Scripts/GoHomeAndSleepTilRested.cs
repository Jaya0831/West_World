using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoHomeAndSleepTilRested : State
{
    int timer;
    public override StateName stateName
    {
        get
        {
            return StateName.GoHomeAndSleepTilRested;
        }
    }
    public override void Enter(Miner miner)
    {
        miner.GoTo(Node.Location_Type.Home);
        timer = 1;
    }
    public override void Execute(Miner miner)
    {
        if (miner.path.Count == 0) 
        {
            if (miner.m_Fatigue == 0) 
            {
                miner.m_StateMachine.RevertToPrevious();
            }
            else
            {
                timer++;
                if (timer % 40 == 0) 
                {
                    miner.m_Fatigue--;
                }
            }
        }
    }
    public override void Exit(Miner miner)
    {
        base.Exit(miner);
    }
}
