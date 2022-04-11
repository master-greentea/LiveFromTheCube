using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RhythmGameStarter;
using System;

public class ObjectiveManager : MonoBehaviour
{
	public TMPro.TextMeshProUGUI objList;
	public GameObject rhythmGame;

	[NonSerialized] public int hour; // from 9 to 17 [System.NonSerialized] 
	[NonSerialized] public int minute; // 0 - 59 [System.NonSerialized]
	[NonSerialized] public int day = 1; // 1-5 [System.NonSerialized]

	[SerializeField] TextMeshProUGUI timeText;
	public GameObject viewrshipManager;
	public GameObject clientSusManager;
	public GameObject currencyManager;
	public GameObject mailScreen;
	public GameObject fade;

	EmailLoader emailloader;
	ClientMatching clientmatch;
	Viewship viewership;
	CurrencySystem currensys;



	StatsSystem statsystem;

	public TMPro.TextMeshProUGUI task1;
	public TMPro.TextMeshProUGUI task2;
	public TMPro.TextMeshProUGUI task3;
	public TMPro.TextMeshProUGUI task4;
	public TMPro.TextMeshProUGUI task5;

	public TMPro.TextMeshProUGUI maxComboUI;
	public TMPro.TextMeshProUGUI highestViewsUI;
	public TMPro.TextMeshProUGUI moneyEarnedUI;
	public TMPro.TextMeshProUGUI susReportUI;

	private int maxCombo;
	private float highestView;
	private int moneyEarned;


	public TMPro.TextMeshProUGUI UItime;
	public TMPro.TextMeshProUGUI UIviews;

	private bool task1complete = false;
	private bool task2complete = false;
	private bool task3complete = false;
	private bool task4complete = false;
	private bool task5complete = false;

	public GameObject task1circle;
	public GameObject task2circle;
	public GameObject task3circle;
	public GameObject task4circle;
	public GameObject task5circle;


	//in theory i would import the days from the day manager 
	private bool day1 = false;
	private bool day2 = false;
	private bool day3 = false;
	private bool day4 = false;
	private bool day5 = false;

	private float viewBenchmark = 0;
	private int emailNumBenchmark = 0;
	private int phoneNumBenchmark = 0;
	private int clientNumBenchmark = 0;
	private bool caught = false;

	private float views = 0;
	private float sentmails = 0;
	private float clientMatched = 0;
	float timer = 1f; //seconds

	List<Email> specialEmails;



	void Start()
	{
		hour = 9;
		minute = 0;
		viewership = viewrshipManager.GetComponent<Viewship>();
		emailloader = mailScreen.GetComponent<EmailLoader>();
		clientmatch = clientSusManager.GetComponent<ClientMatching>();
		currensys = currencyManager.GetComponent<CurrencySystem>();
		statsystem = rhythmGame.GetComponent<StatsSystem>();

		sentmails = emailloader.emailIndex;

		objList.text = "vibe. no objectives yet";
		day1 = true;

		viewBenchmark = 1000;
		emailNumBenchmark = 5;
		task2complete = true;
		task3complete = true;

		clientNumBenchmark = 2;

		DAY1();

		byte cc = 163;
		task1circle.GetComponent<Image>().color = new Color32(cc, cc, cc, 100);
		task2circle.GetComponent<Image>().color = new Color32(cc, cc, cc, 100);
		task3circle.GetComponent<Image>().color = new Color32(cc, cc, cc, 100);
		task4circle.GetComponent<Image>().color = new Color32(cc, cc, cc, 100);
		task5circle.GetComponent<Image>().color = new Color32(cc, cc, cc, 100);

	}

	void Update()
	{

		DAY1();

		string hourStr;
		string minuteStr;

		if (hour < 10)
		{
			hourStr = "0" + hour.ToString();

		}
		else
		{
			hourStr = hour.ToString();
		}

		if (minute < 10)
		{
			minuteStr = "0" + minute.ToString();

		}
		else
		{
			minuteStr = minute.ToString();
		}

		timeText.text = hourStr + ":" + minuteStr;
		UItime.text = hourStr + ":" + minuteStr;

		TimeProgress();


		views = viewership.viewers;
		UIviews.text = viewership.viewers + "";
		sentmails = emailloader.emailIndex;
		clientMatched = clientmatch.clientMatched;

		if (views > viewBenchmark)
		{
			task4complete = true;

		}


		//END SCREEN STUFF 
		if (highestView < views)
		{
			highestView = views;
		}

		// END SCREEN STUFF STOPS HERE

		bool specialEmailsSent = true;

		if (specialEmails != null)
		{
			foreach (Email e in specialEmails)
			{
				if (!e.isSent)
				{
					specialEmailsSent = false;
					break;
				}
			}
			if (specialEmailsSent)
			{
				if (sentmails > emailNumBenchmark)
				{
					task1complete = true;
				}
			}
		}





		//TASK STUFF 
		if (clientMatched >= clientNumBenchmark)
		{
			task5complete = true;
		}

		if (task1complete == true && task2complete == true && task3complete == true && task4complete == true && task5complete == true)
		{
			dayEnd();
		}
	}

	//MORE END SCREEN STUFF
	void dayEnd()
	{
		fade.SetActive(true);

		maxComboUI.text = statsystem.maxCombo + "!";
		highestViewsUI.text = highestView + " views";
		moneyEarnedUI.text = "$" + currensys.money + "";
		susReportUI.text = "sussy baka";
	}




	void DAY1()
	{
		objList.text = "";
		task1.text = "Send" + emailNumBenchmark + " emails";
		task2.text = "Use phone less than 3 times but make 2 calls";
		task3.text = "Don't get Caught";
		task4.text = "Reach " + viewBenchmark + " views. View num: " + views;
		task5.text = "Match " + clientNumBenchmark + " Client Tasks";

		//TASK ONE 
		if (task1complete == true)
		{
			task1.fontStyle = FontStyles.Strikethrough;
			task1circle.GetComponent<Image>().color = new Color32(255, 195, 0, 100);

		}
		else if (task1complete == false)
		{
			task1.fontStyle = FontStyles.Bold;
		}

		//TASK TWO 

		if (task2complete == true)
		{
			task2.fontStyle = FontStyles.Strikethrough;
			task2circle.GetComponent<Image>().color = new Color32(255, 195, 0, 100);

		}
		else if (task2complete == false)
		{
			task2.fontStyle = FontStyles.Bold;
		}


		//TASK THREE 
		if (task3complete == true)
		{
			task3.fontStyle = FontStyles.Strikethrough;
			task3circle.GetComponent<Image>().color = new Color32(255, 195, 0, 100);

		}
		else if (task3complete == false)
		{
			task3.fontStyle = FontStyles.Bold;
		}

		//TASK FOUR 

		if (task4complete == true)
		{
			task4.fontStyle = FontStyles.Strikethrough;
			task4circle.GetComponent<Image>().color = new Color32(255, 195, 0, 100);

		}
		else if (task4complete == false)
		{
			task4.fontStyle = FontStyles.Bold;
		}

		//TASK FIVE
		if (task5complete == true)
		{
			task5.fontStyle = FontStyles.Strikethrough;
			task5circle.GetComponent<Image>().color = new Color32(255, 195, 0, 100);

		}
		else if (task5complete == false)
		{
			task4.fontStyle = FontStyles.Bold;
		}
	}


	void DAY2()
	{

	}

	void DAY3()
	{

	}

	void DAY4()
	{

	}

	void DAY5()
	{

	}

	void TimeProgress()
	{

		timer -= Time.deltaTime;
		if (timer <= 0)
		{

			minute++;
			if (minute >= 60)
			{

				hour++;
				if (hour >= 15)
				{
					hour = 9;
					DayProgress();
				}
				minute = 0;
			}
			timer = 1f;
		}

	}
	void DayProgress()
	{
		emailloader.ResetDay();
		specialEmails = new List<Email>();
		specialEmails.AddRange(emailloader.morningEmails[day]);
		specialEmails.AddRange(emailloader.eveningEmails[day]);

		switch (day)
		{
			case 1:
				DAY1();
				break;
			case 2:
				DAY2();
				break;
			case 3:
				DAY3();
				break;
			case 4:
				DAY4();
				break;
			case 5:
				DAY5();
				break;
			default:
				break;
		}
	}

}
