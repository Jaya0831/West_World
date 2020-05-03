using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalState_Elsa : State<Elsa>
{
    public override void Execute(Elsa elsa)
    {
        if (elsa.e_StateMachine.e_CurrentState.stateName != State<Elsa>.StateName.GoBathroom)
        {
            if (Random.Range(0, 500) == 250)
            {
                elsa.stateMachine_Elsa.GetComponent<StateMachine_Elsa>().ChangeState(new GoBathroom());
            }
        }      
    }
}
