using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrencySystem : MonoBehaviour
{
    public TextMeshProUGUI moneyDisplay;  
    public int money = 1000;


    // Start is called before the first frame update
    void Start()
    {
        moneyDisplay.text = ""+money; 
    }

    public int returnMoney() {
        return money; 
    }

    // Update is called once per frame
    void Update()
    {
        moneyDisplay.text = "" + money;
    }
}
