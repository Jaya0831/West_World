using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterMineAndDigForNugget : State<Miner>
{
    public override StateName stateName
    {
        get
        {
            return StateName.EnterMineAndDigForNugget;
        }
    }
    public int timer;
    public override void Enter(Miner miner)
    {
        Debug.Log("Dig_Enter");
        miner.GoTo(Node.Location_Type.Mine);
        timer = 1;
    }
    public override void Execute(Miner miner)
    {
        if (miner.path.Count == 0)
        {
            timer++;
            if (timer % 100 == 0)
            {
                miner.m_Thirst++;
            }
            if (timer % 50 == 0)
            {
                miner.m_GoldCarried++;
            }
            if (timer % 150 == 0)
            {
                miner.m_Fatigue++;
            }
        }
    }
    public override void Exit(Miner miner)
    {
        Debug.Log("exit digging!");
    }
}
