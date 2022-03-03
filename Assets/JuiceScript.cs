using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JuiceScript : MonoBehaviour
{

    public GameObject MoneyManager;
    CurrencySystem mmanager;

    public GameObject insult;

    public Button juiceButton;
    public GameObject juiceSoldOut;
    public Button kirbyButton;
    public GameObject kirbySoldOut;
    public Button mirrorButton;
    public GameObject mirrorSoldOut;
    public Button plantButton;
    public GameObject plantSoldOut;


    // juice
    // [SerializeField]
    // GameObject juiceObj;
    [SerializeField]
    int juiceprice = 15;
    bool boughtJuice;

    // kirby
    [SerializeField]
    GameObject kirbyObj;
    [SerializeField]
    int kirbyprice = 250;
    bool boughtKirby;

    // mirror
    // [SerializeField]
    // GameObject mirrorObj;
    [SerializeField]
    int mirrorprice = 10;
    bool boughtMirror;

    // plant
    [SerializeField]
    GameObject plantObj;
    [SerializeField]
    int plantprice = 170;
    bool boughtPlant;

    void Start(){
        mmanager = MoneyManager.GetComponent<CurrencySystem>();

        Button juice = juiceButton.GetComponent<Button>();
        if (!boughtJuice && CheckMoney(juiceprice)) juice.onClick.AddListener(JuiceOnClick);

        Button kirby = kirbyButton.GetComponent<Button>();
        if (!boughtKirby) kirby.onClick.AddListener(KirbyOnClick);

        Button mirror = mirrorButton.GetComponent<Button>();
        if (!boughtMirror && CheckMoney(mirrorprice)) mirror.onClick.AddListener(MirrorOnClick);

        Button plant = plantButton.GetComponent<Button>();
        if (!boughtPlant) plant.onClick.AddListener(PlantOnClick);

    }

    void Update() {
    }
 
    void JuiceOnClick() {
        mmanager.money -= juiceprice;
        
    }

    void KirbyOnClick() {
        if (CheckMoney(kirbyprice)) {
            boughtKirby = true;
            mmanager.money -= kirbyprice; // deduct price
            kirbySoldOut.SetActive(true); // say sold out
            kirbyButton.GetComponent<Button>().enabled = false; // no click
            kirbyObj.SetActive(true); // set plant
        }
        
    }

    void PlantOnClick() {
        if (CheckMoney(plantprice)) {
            boughtPlant = true;
            mmanager.money -= plantprice; // deduct price
            plantSoldOut.SetActive(true); // say sold out
            plantButton.GetComponent<Button>().enabled = false; // no click
            plantObj.SetActive(true); // set plant
        }
    }

    void MirrorOnClick() {
        mmanager.money -= mirrorprice;
        
    }

    bool CheckMoney(int price) {
        if (price <= mmanager.money) {
            return true;
        }
        else
            Debug.Log("You Poor");
            insult.SetActive(true);
            return false;
    }
}
