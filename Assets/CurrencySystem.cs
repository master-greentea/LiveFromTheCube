using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrencySystem : MonoBehaviour
{
    public TextMeshProUGUI moneyDisplay;  
    public Viewship viewship;
    public int money;
    
    // viewership tiers
    int viewerMod = 2;
    int viewerTier = 100;
    int nextViewerTier = 200;

    float getMoneyTimer;
    int getMoneyRange;

    int tierIndex = 1;

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
        if (money < 0) {money = 0;}
        if (viewship.rhythmGame.activeInHierarchy) {
            //etMoneyBasedOnViewers();
        }

        moneyDisplay.text = "" + money;
    }

    public void GainMoney(int income)
	{
        money += income;
	}

    /*
    void GetMoneyBasedOnViewers() {
        if (viewship.viewers >= viewerTier && viewship.viewers < nextViewerTier) {
            GetMoneyOvertime();
        }
        // reframing viewership tier
        else if (viewship.viewers < viewerTier) {
            viewerTier /= viewerMod;
            nextViewerTier /= nextViewerTier;
            tierIndex --;
        }
        else {
            viewerTier *= viewerMod;
            nextViewerTier *= viewerMod;
            tierIndex ++;
        }

        if (viewerTier < 100) {viewerTier = 100; nextViewerTier = 200; tierIndex = 1;} //clamp
    }

    void GetMoneyOvertime() {
        getMoneyRange = Random.Range(6, 9);
        
        if (getMoneyTimer > .75f) {
            money += getMoneyRange * tierIndex;
            getMoneyTimer = 0;
        }
        getMoneyTimer += Time.deltaTime;
    }
    */
}
