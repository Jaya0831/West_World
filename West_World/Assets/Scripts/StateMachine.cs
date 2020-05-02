using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    /// <summary>
    /// 表示当前状态实例的指针
    /// </summary>
    public State m_CurrentState;
    /// <summary>
    /// 表示先前状态
    /// </summary>
    private State m_PreviousState;
    /// <summary>
    /// 表示全局状态
    /// </summary>
    private State m_GlobalState;
    /// <summary>
    /// 拥有它的智能体
    /// </summary>
    public GameObject owner;
    private void Awake()
    {
        m_GlobalState = new GlobalState();
        Debug.Log(m_GlobalState);
        m_CurrentState = new EnterMineAndDigForNugget();
        Debug.Log(m_CurrentState);
    }
    private void Start()
    {
        m_CurrentState.Enter(owner.GetComponent<Miner>());
    }
    private void Update()
    {
        Debug.Log(m_CurrentState);
        if (m_GlobalState)
        {
            m_GlobalState.Execute(owner.GetComponent<Miner>());
        }
        if (m_CurrentState)
        {
            m_CurrentState.Execute(owner.GetComponent<Miner>());
        }
    }
    /// <summary>
    /// 转换状态
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(State newState)
    {
        if (m_CurrentState.stateName != State.StateName.VisitBankAndDepositGold) 
        {
            m_PreviousState = m_CurrentState;
        }
        m_CurrentState.Exit(owner.GetComponent<Miner>());
        m_CurrentState = newState;
        m_CurrentState.Enter(owner.GetComponent<Miner>());
    }
    /// <summary>
    /// 回到先前状态
    /// </summary>
    /// <param name="destinaton"></param>
    public void RevertToPrevious()
    {
        ChangeState(m_PreviousState);
    }
}
