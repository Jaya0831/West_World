using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elsa : BaseGameEntity
{
    public GameObject stateMachine_Elsa;
    public StateMachine_Elsa e_StateMachine;
    private void Awake()
    {
        e_StateMachine = stateMachine_Elsa.GetComponent<StateMachine_Elsa>();
        speed = 0.1f;
    }
    private void Start()
    {
        m_ID = 1;
        transform.SetPositionAndRotation(grid.GetComponent<Grid>().objectInf[(int)Node.Location_Type.Floor][0], Quaternion.identity);
    }
}
