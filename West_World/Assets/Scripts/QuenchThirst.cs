using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuenchThirst : State
{
    int timer = 0;
    public override void Enter(Miner miner)
    {
        miner.GoTo(Node.Location_Type.Bar);
        timer = 1;
    }
    public override void Execute(Miner miner)
    {
        if (miner.path.Count == 0) 
        {
            if (miner.m_Thirst == 0)
            {
                if (miner.m_Fatigue > 7)
                {
                    miner.ChangeState(new GoHomeAndSleepTilRested());
                }
                else
                {
                    miner.ChangeState(new EnterMineAndDigForNugget());
                }
            }
            else
            {
                timer++;
                if (timer % 25 == 0)
                {
                    miner.m_Thirst--;
                }
            }
        }
        
    }
    public override void Exit(Miner miner)
    {
        Debug.Log("exit drinking!");
    }
}
