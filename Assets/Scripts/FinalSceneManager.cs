using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalSceneManager : MonoBehaviour
{
    public static FinalSceneManager Instance { get; private set; }

    int viewsTotal;
    int emailsSentTotal;
    int clientsMatchedTotal;
    int decorsBoughtTotal;
    int comboMax;
    int moneyRemaining;
    [SerializeField] GameObject viewsTotalUI;
    [SerializeField] GameObject emailsSentTotalUI;
    [SerializeField] GameObject clientsMatchedTotalUI;
    [SerializeField] GameObject decorsBoughtTotalUI;
    [SerializeField] GameObject comboMaxUI;
    [SerializeField] GameObject moneyUI;

    public void SetFinalStats(int views,int emails,int clients,int decors,int combo,int money)
	{
        viewsTotal = views;
        emailsSentTotal = emails;
        clientsMatchedTotal = clients;
        decorsBoughtTotal = decors;
        comboMax = combo;
        moneyRemaining = money;
	}

	private void Awake()
	{

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }


	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(viewsTotal + "/" + emailsSentTotal + "/" + clientsMatchedTotal + "/" + decorsBoughtTotal + "/" + comboMax + "/" + moneyRemaining);
    }
}
