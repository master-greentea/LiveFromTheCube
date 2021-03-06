using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ShopManager : MonoBehaviour
{

    public GameObject MoneyManager;
    CurrencySystem mmanager;

    [SerializeField] public SodaCanEffect sce;

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
    public Button mugButton;
    public GameObject mugSoldOut;
    public Button redButton;
    public GameObject redSoldOut;
    public Button frameButton;
    public GameObject frameSoldOut;
    public Button earButton;
    public GameObject earSoldOut;
    public Button dressButton;
    public GameObject dressSoldOut;


    // juice
    [SerializeField]
    GameObject juiceObj;
    [SerializeField] int juiceprice = 15;
    [SerializeField] TMPro.TextMeshProUGUI juicePriceText;
    bool boughtJuice;

    // kirby
    [SerializeField] GameObject kirbyObj;
    [SerializeField] int kirbyprice = 250;
    [SerializeField] TMPro.TextMeshProUGUI kirbyPriceText;
    bool boughtKirby;

    // mirror
    [SerializeField]
    GameObject mirrorObj;
    [SerializeField] int mirrorprice = 10;
    [SerializeField] TMPro.TextMeshProUGUI mirrorPriceText;
    bool boughtMirror;

    // plant
    [SerializeField] GameObject plantObj;
    [SerializeField] int plantprice = 170;
    [SerializeField] TMPro.TextMeshProUGUI plantPriceText;
    bool boughtPlant;

    // mug
    [SerializeField] GameObject mugObj;
    [SerializeField] int mugprice = 20000;
    [SerializeField] TMPro.TextMeshProUGUI mugPriceText;
    bool boughtMug;

    // red
    [SerializeField] GameObject redObj;
    [SerializeField] int redprice = 20000;
    [SerializeField] TMPro.TextMeshProUGUI redPriceText;
    bool boughtRed;

    // frame
    [SerializeField] GameObject frameObj;
    [SerializeField] int frameprice = 20000;
    [SerializeField] TMPro.TextMeshProUGUI framePriceText;
    bool boughtFrame;

    // ear
    [SerializeField] int earprice = 20000;
    [SerializeField] TMPro.TextMeshProUGUI earPriceText;
    bool boughtEar;
    [SerializeField] Sprite earSprite;

    // dress
    [SerializeField] int dressprice = 20000;
    [SerializeField] TMPro.TextMeshProUGUI dressPriceText;
    bool boughtDress;
    [SerializeField] Sprite dressSprite;

    // PLYAER APPERANCE
    [SerializeField] SpriteRenderer player;
    [SerializeField] Sprite bothSprite;



    // objective manager
    [SerializeField] GameObject objectiveManager;
    
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

        Button mug = mugButton.GetComponent<Button>();
        if (!boughtMug) mug.onClick.AddListener(MugOnClick);

        Button red = redButton.GetComponent<Button>();
        if (!boughtRed) red.onClick.AddListener(RedOnClick);

        Button frame = frameButton.GetComponent<Button>();
        if (!boughtFrame) frame.onClick.AddListener(FrameOnClick);

        Button ear = earButton.GetComponent<Button>();
        if (!boughtEar) ear.onClick.AddListener(EarOnClick);

        Button dress = dressButton.GetComponent<Button>();
        if (!boughtDress) dress.onClick.AddListener(DressOnClick);


        // set price
        juicePriceText.text = "$"+juiceprice;
        kirbyPriceText.text = "$"+kirbyprice;
        mirrorPriceText.text = "$"+mirrorprice;
        plantPriceText.text = "$"+plantprice;
        mugPriceText.text = "$"+mugprice;
        redPriceText.text = "$"+redprice;
        framePriceText.text = "$"+frameprice;
        earPriceText.text = "$"+earprice;
        dressPriceText.text = "$"+dressprice;

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

    public void NewRandomTagline(){
        insult.text = RandomTagline();
    }

    string RandomTagline() {
        string[] insultArray = {"Our employees' wages have never been lower!", 
        "Work hard, no fun, become history!", 
        "All hail our great leader Beff Jezos!", 
        "Bank account too full? Don't worry! We will drain it dry for you!",
        "Everything from B to Z!",
        "Let's destroy brick and mortar together!",
        "Thanks for all your data!",
        "Bamazing!",
        "We know you so well, we finish each other's purchases!",
        "Get one month of Bamazon Brime on us!",
        "Our employees don't need bathroom breaks; we don't let them hydrate!"};
        int randIndex = UnityEngine.Random.Range(0, insultArray.Length);
        return  insultArray[randIndex];
    }
 
    void JuiceOnClick() {
        juiceButton.gameObject.transform.parent.transform.Find("detes_s").gameObject.SetActive(true);
        juiceObj.SetActive(false); // set plant
    }

    public void JuiceBuys() {
        if (CheckMoney(juiceprice)) {
            // boughtKirby = true;
            mmanager.money -= juiceprice; // deduct price
            // kirbySoldOut.SetActive(true); // say sold out
            // juiceButton.GetComponent<Button>().enabled = false; // no click
            juiceObj.SetActive(true); // set plant
            objectiveManager.GetComponent<ObjectiveManager>().DecorBought();
            sce.sodaTimer = 0;
        }
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
            objectiveManager.GetComponent<ObjectiveManager>().DecorBought();
        }
    }

    void PlantOnClick() {
        plantButton.gameObject.transform.parent.transform.Find("detes_p").gameObject.SetActive(true);
    }

    public void PlantBuys() {
        if (CheckMoney(plantprice)) {
            boughtPlant = true;
            mmanager.money -= plantprice; // deduct price
            plantSoldOut.SetActive(true); // say sold out
            plantButton.GetComponent<Button>().enabled = false; // no click
            plantObj.SetActive(true); // set plant
            objectiveManager.GetComponent<ObjectiveManager>().DecorBought();
        }
    }

    void MirrorOnClick() {
        mirrorButton.gameObject.transform.parent.transform.Find("detes_m").gameObject.SetActive(true);
    }

    public void MirrorBuys() {
        if (CheckMoney(mirrorprice)) {
            boughtMirror = true;
            mmanager.money -= mirrorprice; // deduct price
            mirrorSoldOut.SetActive(true); // say sold out
            mirrorButton.GetComponent<Button>().enabled = false; // no click
            mirrorObj.SetActive(true); // set plant
            objectiveManager.GetComponent<ObjectiveManager>().DecorBought();
        }
    }

    void MugOnClick() {
        mugButton.gameObject.transform.parent.transform.Find("detes_mug").gameObject.SetActive(true);
    }

    public void MugBuys() {
        if (CheckMoney(mugprice)) {
            boughtMug = true;
            mmanager.money -= mugprice; // deduct price
            mugSoldOut.SetActive(true); // say sold out
            mugButton.GetComponent<Button>().enabled = false; // no click
            mugObj.SetActive(true); // set plant
            objectiveManager.GetComponent<ObjectiveManager>().DecorBought();
        }
    }

    void RedOnClick() {
        redButton.gameObject.transform.parent.transform.Find("detes_rb").gameObject.SetActive(true);
    }

    public void RedBuys() {
        if (CheckMoney(redprice)) {
            boughtRed = true;
            mmanager.money -= redprice;
            redSoldOut.SetActive(true);
            redButton.GetComponent<Button>().enabled = false;
            redObj.SetActive(true);
            objectiveManager.GetComponent<ObjectiveManager>().DecorBought();
        }
    }

    void FrameOnClick() {
        frameButton.gameObject.transform.parent.transform.Find("detes_f").gameObject.SetActive(true);
    }

    public void FrameBuys() {
        if (CheckMoney(frameprice)) {
            boughtFrame = true;
            mmanager.money -= frameprice;
            frameSoldOut.SetActive(true);
            frameButton.GetComponent<Button>().enabled = false;
            frameObj.SetActive(true);
            objectiveManager.GetComponent<ObjectiveManager>().DecorBought();
        }
    }

    void EarOnClick() {
        earButton.gameObject.transform.parent.transform.Find("detes_e").gameObject.SetActive(true);
    }

    public void EarBuys() {
        if (CheckMoney(earprice)) {
            boughtEar = true;
            mmanager.money -= earprice;
            earSoldOut.SetActive(true);
            earButton.GetComponent<Button>().enabled = false;
            PlayerApperanceChange();
            objectiveManager.GetComponent<ObjectiveManager>().DecorBought();
        }
    }

    void DressOnClick() {
        dressButton.gameObject.transform.parent.transform.Find("detes_d").gameObject.SetActive(true);
    }

    public void DressBuys() {
        if (CheckMoney(dressprice)) {
            boughtDress = true;
            mmanager.money -= dressprice;
            dressSoldOut.SetActive(true);
            dressButton.GetComponent<Button>().enabled = false;
            PlayerApperanceChange();
            objectiveManager.GetComponent<ObjectiveManager>().DecorBought();
        }
    }

    void PlayerApperanceChange()
    {
        if (boughtDress && !boughtEar) player.sprite = dressSprite;
        else if (!boughtDress && boughtEar) player.sprite = earSprite;
        else if (boughtDress && boughtEar) player.sprite = bothSprite;
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
