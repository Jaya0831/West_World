using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatStew : State<Miner>
{
    int timer;
    public override StateName stateName
    {
        get
        {
            return StateName.EatStew;
        }
    }
    public override void Enter(Miner miner)
    {
        timer = 1;
        miner.GoTo(Node.Location_Type.Table);
    }
    public override void Execute(Miner miner)
    {
        if (miner.path.Count == 0)
        {
            timer++;
            if (timer % 200 == 0) 
            {
                miner.m_StateMachine.ChangeState(new GoHomeAndSleepTilRested());
            }
        }
    }
    public override void Exit(Miner miner)
    {
        Debug.Log("Exit EatStew!");
    }
}
