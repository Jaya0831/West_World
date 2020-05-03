using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine_Elsa : MonoBehaviour
{
    /// <summary>
    /// 表示当前状态实例的指针
    /// </summary>
    public State<Elsa> e_CurrentState;
    /// <summary>
    /// 表示先前状态
    /// </summary>
    private State<Elsa> e_PreviousState;
    /// <summary>
    /// 表示全局状态
    /// </summary>
    private State<Elsa> e_GlobalState;
    /// <summary>
    /// 拥有它的智能体
    /// </summary>
    public GameObject owner;


    private void Start()
    {
        e_CurrentState = new CleanTheHouse();
        e_GlobalState = new GlobalState_Elsa();
        e_CurrentState.Enter(owner.GetComponent<Elsa>());
    }
    private void Update()
    {
        e_GlobalState.Execute(owner.GetComponent<Elsa>());
        e_CurrentState.Execute(owner.GetComponent<Elsa>());
    }
    /// <summary>
    /// 转换状态
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(State<Elsa> newState)
    {
        e_PreviousState = e_CurrentState;
        e_CurrentState.Exit(owner.GetComponent<Elsa>());
        e_CurrentState = newState;
        e_CurrentState.Enter(owner.GetComponent<Elsa>());
    }
    /// <summary>
    /// 回到先前状态
    /// </summary>
    /// <param name="destinaton"></param>
    public void RevertToPrevious()
    {
        ChangeState(e_PreviousState);
    }
}
