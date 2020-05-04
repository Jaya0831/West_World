using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookStew : State<Elsa>
{
    private enum Stages {WalkToStove, Cooking, FinishCooking};
    private Stages stage;
    public override StateName stateName
    {
        get
        {
            return StateName.CookStew;
        }
    }
    public override void Enter(Elsa elsa)
    {
        elsa.GoTo(Node.Location_Type.Stove);
        stage = Stages.WalkToStove;
    }
    public override void Execute(Elsa elsa)
    {
        if (elsa.path.Count == 0) 
        {
            if (stage == Stages.WalkToStove) 
            {
                stage = Stages.Cooking; 
                MessageDispatcher.DispatchMessage(2, 1, 1, (int)message_type.Msg_StewReady);                
            }
            if (stage == Stages.FinishCooking)
            {
                MessageDispatcher.DispatchMessage(0, 1, 0, (int)message_type.Msg_StewReady);
                elsa.e_StateMachine.ChangeState(new CleanTheHouse());
            }
        }
    }
    public override void Exit(Elsa miner)
    {
        Debug.Log("Exit CookStew!");
    }
    public override bool OnMessage(Elsa entity, Telegram msg)
    {
        if (msg.msg == (int)message_type.Msg_StewReady) 
        {
            stage = Stages.FinishCooking;
            entity.GoTo(Node.Location_Type.Table);
            return true;
        }
        return false;
    }
}
