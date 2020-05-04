using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    /// <summary>
    /// 表示当前状态实例的指针
    /// </summary>
    public State<Miner> m_CurrentState = new EnterMineAndDigForNugget();
    /// <summary>
    /// 表示先前状态
    /// </summary>
    public State<Miner> m_PreviousState;
    /// <summary>
    /// 表示全局状态
    /// </summary>
    private State<Miner> m_GlobalState = new GlobalState();
    /// <summary>
    /// 拥有它的智能体
    /// </summary>
    public GameObject owner;


    private void Start()
    {
        m_CurrentState = new EnterMineAndDigForNugget();
        m_GlobalState = new GlobalState();
        m_CurrentState.Enter(owner.GetComponent<Miner>());
    }
    private void Update()
    {
        //Debug.Log(m_CurrentState);
        m_GlobalState.Execute(owner.GetComponent<Miner>());
        m_CurrentState.Execute(owner.GetComponent<Miner>());
    }
    /// <summary>
    /// 转换状态
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(State<Miner> newState)
    {
        if (m_CurrentState.stateName != State<Miner>.StateName.VisitBankAndDepositGold && m_CurrentState.stateName != State<Miner>.StateName.GoHomeAndSleepTilRested && m_CurrentState.stateName != State<Miner>.StateName.EatStew) 
        {
            m_PreviousState = m_CurrentState;
        }
        // HACK：感觉这句放这里不太合适
        if (m_CurrentState.stateName != State<Miner>.StateName.GoHomeAndSleepTilRested && m_CurrentState.stateName != State<Miner>.StateName.EatStew) 
        {
            owner.GetComponent<Miner>().hadAteStew = false;
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
    public bool HandleMessage(Telegram msg)
    {
        Debug.Log("HandleMessage: Miner's StateMachine");
        if (m_CurrentState.OnMessage(owner.GetComponent<Miner>(), msg)) 
        {
            return true;
        }
        if (m_GlobalState.OnMessage(owner.GetComponent<Miner>(), msg)) 
        {
            return true;
        }
        return false;
    }
}
