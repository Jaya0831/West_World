using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoHomeAndSleepTilRested : State<Miner>
{
    int timer;
    private bool hadSendMessage;
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
        hadSendMessage = false;
    }
    public override void Execute(Miner miner)
    {
        if (miner.path.Count == 0) 
        {
            if ((!hadSendMessage) && miner.hadAteStew == false) 
            {
                // HACK:通过角色类的实例获得id
                MessageDispatcher.DispatchMessage(0, 0, 1, (int)message_type.Msg_HiHoneyImHome);
                hadSendMessage = true;
                miner.hadAteStew = true;
            }
            if (miner.m_Fatigue == 0) 
            {
                miner.m_StateMachine.RevertToPrevious();
            }
            else
            {
                timer++;
                if (timer % 60 == 0) 
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
    public override bool OnMessage(Miner entity, Telegram msg)
    {
        Debug.Log("OnMessage GoHomeAndSleepTilRested");
        if (msg.msg == (int)message_type.Msg_StewReady) 
        {
            entity.m_StateMachine.ChangeState(new EatStew());
            return true;
        }
        return false;
    }
}
