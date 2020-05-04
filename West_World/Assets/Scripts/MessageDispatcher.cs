using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum message_type
{
    Msg_HiHoneyImHome,
    Msg_StewReady
}
public struct Telegram
{
    /// <summary>
    /// 发送这个telegram的实体
    /// </summary>
    public int sender;
    /// <summary>
    /// 接受这个telegram的实体
    /// </summary>
    public int receiver;
    /// <summary>
    /// 信息本身
    /// </summary>
    public int msg;
    /// <summary>
    /// 消息延时
    /// </summary>
    public double dispatchTime;
    public void WriteTelegram(int sender, int receiver, int msg, double dispatchTime)
    {
        this.sender = sender;
        this.receiver = receiver;
        this.msg = msg;
        this.dispatchTime = dispatchTime;
    }
}
public class MessageDispatcher : MonoBehaviour
{
    /// <summary>
    /// 存放延迟消息的容器（按延迟时间排序，无重复元素）
    /// </summary>
    private static SortedList<double, Telegram> priorityQ = new SortedList<double, Telegram>();

    private void Update()
    {
        DispatchDelayMessages();
    }

    /// <summary>
    /// 调用接受实体的消息处理函数
    /// </summary>
    /// <param name="pReceiver"></param>
    /// <param name="telegram"></param>
    private static void DisCharge(BaseGameEntity pReceiver,Telegram telegram)
    {
        Debug.Log("DisCharge:" + telegram.msg);
        pReceiver.HandleMessage(telegram);
    }
    /// <summary>
    /// 处理消息（即时消息发送，延时消息加入队列）
    /// </summary>
    /// <param name="delay"></param>
    /// <param name="sender"></param>
    /// <param name="receiver"></param>
    /// <param name="msg"></param>
    public static void DispatchMessage(double delay, int sender, int receiver, int msg)
    {
        Debug.Log("DispatchMessage:" + msg);
        BaseGameEntity pReceiver = EntityManager.GetEntityFromID(receiver);
        Telegram telegram = new Telegram();
        telegram.WriteTelegram(sender, receiver, msg, delay);
        if (delay <= 0.0) 
        {
            Debug.Log("delay <= 0.0");
            DisCharge(pReceiver, telegram);
        }
        else
        {
            Debug.Log("delay > 0.0");
            double currentTime = Time.time;
            telegram.dispatchTime = currentTime + delay;
            priorityQ.Add(telegram.dispatchTime, telegram);
        }
    }
    /// <summary>
    /// 发送延时消息（每帧调用一次）
    /// </summary>
    public static void DispatchDelayMessages()
    {
        double currentTime = Time.time;
        while (priorityQ.Count != 0 && (priorityQ.Keys[0] < currentTime) && priorityQ.Keys[0] > 0) 
        {
            Telegram telegram = priorityQ[priorityQ.Keys[0]];
            BaseGameEntity receiver = EntityManager.GetEntityFromID(telegram.receiver);
            DisCharge(receiver, telegram);
            priorityQ.RemoveAt(0);
        }
    }
}
