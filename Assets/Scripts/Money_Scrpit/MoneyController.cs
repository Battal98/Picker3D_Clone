using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyController : MonoBehaviour
{
    public static MoneyController instance;
    public int TotalMoney;
    public int currMoney;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        TotalMoney = PlayerPrefs.GetInt("TotalMoney", TotalMoney);
        UIManager.instance.totalMoneyText.text = TotalMoney.ToString();
    }
    public void MoneyIncreaseProcess(GameObject _obj)
    {

    }

    public void MoneyDecreaseProcess(GameObject _obj)
    {

    }

    public void CalculateTotalMoney()
    {

    }
}
