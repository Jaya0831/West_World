using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalState_Elsa : State<Elsa>
{
    public override void Execute(Elsa elsa)
    {
        if (elsa.e_StateMachine.e_CurrentState.stateName != State<Elsa>.StateName.GoBathroom && elsa.e_StateMachine.e_CurrentState.stateName != State<Elsa>.StateName.CookStew)
        {
            if (Random.Range(0, 500) == 250)
            {
                elsa.stateMachine_Elsa.GetComponent<StateMachine_Elsa>().ChangeState(new GoBathroom());
            }
        }      
    }
    public override bool OnMessage(Elsa entity, Telegram msg)
    {
        Debug.Log("OnMessage GlobalState_Elsa");
        if (msg.msg == (int)message_type.Msg_HiHoneyImHome)
        {
            // TODO: 如果在上厕所就上完在做饭
            entity.e_StateMachine.ChangeState(new CookStew());
            return true;
        }
        return false;
    }
}
