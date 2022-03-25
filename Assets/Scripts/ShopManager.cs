using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{

    public GameObject MoneyManager;
    CurrencySystem mmanager;

    public TMPro.TextMeshProUGUI insult;
    float insultTimer;
    bool insulting;

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
    [SerializeField]
    TMPro.TextMeshProUGUI juicePriceText;
    bool boughtJuice;

    // kirby
    [SerializeField]
    GameObject kirbyObj;
    [SerializeField]
    int kirbyprice = 250;
    [SerializeField]
    TMPro.TextMeshProUGUI kirbyPriceText;
    bool boughtKirby;

    // mirror
    // [SerializeField]
    // GameObject mirrorObj;
    [SerializeField]
    int mirrorprice = 10;
    [SerializeField]
    TMPro.TextMeshProUGUI mirrorPriceText;
    bool boughtMirror;

    // plant
    [SerializeField]
    GameObject plantObj;
    [SerializeField]
    int plantprice = 170;
    [SerializeField]
    TMPro.TextMeshProUGUI plantPriceText;
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


        // set price
        kirbyPriceText.text = "$"+kirbyprice;
        mirrorPriceText.text = "$"+mirrorprice;
        plantPriceText.text = "$"+plantprice;

        insult.text = RandomTagline();
        insulting = false;
    }

    void Update() {
        if (insultTimer > 3f) {
            insult.text = RandomTagline();
            insultTimer = 0f;
            insulting = false;
        }
        if (insulting) {
            insultTimer += Time.deltaTime;
        }
    }

    string RandomTagline() {
        string[] insultArray = {"Our employees' wages have never been lower!", 
        "Work hard, no fun, become history!", 
        "All hail our great leader Beff Jezos!", 
        "Bank account too full? Don't worry! We will drain it dry for you!"};
        int randIndex = Random.Range(0, insultArray.Length);
        return  insultArray[randIndex];
    }
 
    void JuiceOnClick() {
        mmanager.money -= juiceprice;
        
    }

    void KirbyOnClick() {
        kirbyButton.gameObject.transform.parent.transform.Find("detes_k").gameObject.SetActive(true);
        
    }

    public void KirbyBuys() {
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
            insult.text = "Item purchased. Bamazon Prime instant delievery activated!";
            insultTimer = 0f;
            insulting = true;
            return true;
        }
        else
            insult.text = "You poor bastard get yourself some money first before you use our site.\n-Beff Jezos";
            insultTimer = 0f;
            insulting = true;
            return false;
    }
}
