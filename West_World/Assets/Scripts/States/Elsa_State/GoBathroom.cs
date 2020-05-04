using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBathroom : State<Elsa>
{
    int timer;
    public override StateName stateName
    {
        get
        {
            return State<Elsa>.StateName.GoBathroom;
        }
    }
    public override void Enter(Elsa elsa)
    {
        timer = 1;
        elsa.GoTo(Node.Location_Type.Bathroom);
    }
    public override void Execute(Elsa elsa)
    {
        if (elsa.path.Count == 0) 
        {
            timer++;
            if (timer % 200 == 0) 
            {
                elsa.stateMachine_Elsa.GetComponent<StateMachine_Elsa>().RevertToPrevious();
            }
        }
    }
    public override void Exit(Elsa miner)
    {
        Debug.Log("Exit GoBathroom!");
    }
}
