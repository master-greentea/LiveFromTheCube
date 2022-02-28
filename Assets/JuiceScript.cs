using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JuiceScript : MonoBehaviour
{

    public GameObject MoneyManager;
    CurrencySystem mmanager;

    public Button juiceButton;
    public Button kirbyButton;
    public Button mirrorButton;
    public Button plantButton;

    int juiceprice = 15;
    int kirbyprice = 250;
    int mirrorprice = 10;
    int plantprice = 170; 

    void Start(){
        mmanager = MoneyManager.GetComponent<CurrencySystem>();

        Button juice = juiceButton.GetComponent<Button>();
        juice.onClick.AddListener(JuiceOnClick);

        Button kirby = kirbyButton.GetComponent<Button>();
        kirby.onClick.AddListener(KirbyOnClick);

        Button mirror = mirrorButton.GetComponent<Button>();
        mirror.onClick.AddListener(MirrorOnClick);

        Button plant = plantButton.GetComponent<Button>();
        plant.onClick.AddListener(PlantOnClick);

    }
 
    void JuiceOnClick() {
        mmanager.money -= juiceprice;
        Debug.Log(mmanager.money);
    }

    void KirbyOnClick() {
        mmanager.money -= kirbyprice;
        Debug.Log(mmanager.money);
    }

    void PlantOnClick() {
        mmanager.money -= plantprice;
        Debug.Log(mmanager.money);
    }

    void MirrorOnClick() {
        mmanager.money -= mirrorprice;
        Debug.Log(mmanager.money);
    }
}
