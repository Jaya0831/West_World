using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Text[] texts;
    public GameObject miner;
    public enum textType { Thirst, Fatigue, GoldCarried,MoneyInBank};
    private void Update()
    {
        texts[0].text = "Tirst:" + miner.GetComponent<Miner>().m_Thirst;
        if (miner.GetComponent<Miner>().m_Thirst > 7) 
        {
            texts[0].color = Color.red;
        }
        else
        {
            texts[0].color = Color.green;
        }
        texts[1].text = "Fatigue:" + miner.GetComponent<Miner>().m_Fatigue;
        if (miner.GetComponent<Miner>().m_Fatigue > 7)
        {
            texts[1].color = Color.red;
        }
        else
        {
            texts[1].color = Color.green;
        }
        texts[2].text = "GoldCarried:" + miner.GetComponent<Miner>().m_GoldCarried;
        if (miner.GetComponent<Miner>().m_GoldCarried == 10)
        {
            texts[2].color = Color.yellow;
        }
        else
        {
            texts[2].color = Color.green;
        }
        texts[3].text = "MoneyInBank:" + miner.GetComponent<Miner>().m_MoneyInBank;
    }
}
