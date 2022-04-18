using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RhythmGameStarter;
using System;
using System.Linq;

public class ObjectiveManager : MonoBehaviour
{
	internal class Day : MonoBehaviour
	{
		[SerializeField] public int index;// just for your information, not in actual use
		[SerializeField] public int tasks; // at least 5
		[SerializeField] public int viewersNeeded; // task 0
		[SerializeField] public int clientMatchesNeeded; // task 1
		[SerializeField] public int emailsNeeded; // task 2
		[SerializeField] public int[] morningEmailIDs; // task 3
		[SerializeField] public int[] eveningEmailIDs; // task 4
		[NonSerialized] public bool[] taskComplete;

	}

	[ContextMenu("Add Day Component")]
	private void AddNested()
	{
		gameObject.AddComponent<Day>();
	}


	[SerializeField] Day[] days;
	Day today;
	int dateOfToday = 0;

	[SerializeField] TMPro.TextMeshProUGUI objList;
	[SerializeField] GameObject BOSU;

	[NonSerialized] public int hour; // from 9 to 17 [System.NonSerialized] 
	[NonSerialized] public int minute; // 0 - 59 [System.NonSerialized]
	[NonSerialized] public int day = 1; // 1-5 [System.NonSerialized]

	[SerializeField] TextMeshProUGUI timeText;
	[SerializeField] GameObject viewrshipManager;
	[SerializeField] GameObject clientManager;
	[SerializeField] GameObject currencyManager;
	[SerializeField] GameObject mailScreen;
	[SerializeField] GameObject fade;

	EmailLoader emailloader;
	ClientMatching clientmatch;
	Viewship viewership;
	CurrencySystem currensys;
	StatsSystem statsystem;



	[SerializeField] TMPro.TextMeshProUGUI maxComboUI;
	[SerializeField] TMPro.TextMeshProUGUI highestViewsUI;
	[SerializeField] TMPro.TextMeshProUGUI moneyEarnedUI;
	[SerializeField] TMPro.TextMeshProUGUI susReportUI;

	private int maxCombo;
	private float highestView;
	private int moneyEarned;


	[SerializeField] TMPro.TextMeshProUGUI UItime;
	[SerializeField] TMPro.TextMeshProUGUI UIviews;


	// the list of tasks
	[SerializeField] GameObject[] taskCircles;
	[SerializeField] TextMeshProUGUI[] taskTexts;
	/*
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

	public TMPro.TextMeshProUGUI task1;
	public TMPro.TextMeshProUGUI task2;
	public TMPro.TextMeshProUGUI task3;
	public TMPro.TextMeshProUGUI task4;
	public TMPro.TextMeshProUGUI task5;

	*/

	private float viewBenchmark = 0;
	private int emailNumBenchmark = 0;
	private int phoneNumBenchmark = 0;
	private int clientNumBenchmark = 0;
	private bool caught = false;

	private float views = 0;
	private float emailsSent = 0;
	private float clientsMatched = 0;
	float timer = 1f; //seconds

	List<Email> specialEmails;



	void Start()
	{
		// Objects initialize 
		viewership = viewrshipManager.GetComponent<Viewship>();
		emailloader = mailScreen.GetComponent<EmailLoader>();
		clientmatch = clientManager.GetComponent<ClientMatching>();
		currensys = currencyManager.GetComponent<CurrencySystem>();
		statsystem = BOSU.GetComponent<StatsSystem>();
		foreach (GameObject circle in taskCircles)
		{
			circle.GetComponent<Image>().color = new Color32(163, 163, 163, 100);
		}

		// time setup
		hour = 9;
		minute = 0;

		// day setup
		days = days.OrderBy(c => c.index).ToArray(); // sort the days, in case they are placed in the wrong order
		DayProgress();
		today.taskComplete = new bool[today.tasks];

		

		


		/*
		viewBenchmark = 1000;
		emailNumBenchmark = 5;
		clientNumBenchmark = 2;

		
		task1circle.GetComponent<Image>().color = new Color32(cc, cc, cc, 100);
		task2circle.GetComponent<Image>().color = new Color32(cc, cc, cc, 100);
		task3circle.GetComponent<Image>().color = new Color32(cc, cc, cc, 100);
		task4circle.GetComponent<Image>().color = new Color32(cc, cc, cc, 100);
		task5circle.GetComponent<Image>().color = new Color32(cc, cc, cc, 100);
		*/
	}

	void Update()
	{


		TimeProgress();
		UpdateText();

		views = viewership.viewers;
		UIviews.text = viewership.viewers + "";
		emailsSent = emailloader.emailIndex;
		clientsMatched = clientmatch.clientMatched;

		if (views > viewBenchmark)
		{
			//task4complete = true;

		}


		//END SCREEN STUFF 
		if (highestView < views)
		{
			highestView = views;
		}

		// END SCREEN STUFF STOPS HERE




		/*
		//TASK STUFF 
		if (clientMatched >= clientNumBenchmark)
		{
			task5complete = true;
		}

		if (task1complete == true && task2complete == true && task3complete == true && task4complete == true && task5complete == true)
		{
			dayEnd();
		}
		*/
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



	/*
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
	*/


	void TimeProgress()
	{
		// display
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


		// time logic
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
		today = days[dateOfToday];
		emailloader.ResetDay();


	}
	void UpdateText()
	{
		taskTexts[0].text = "Send" + today.emailsNeeded + " emails";
		taskTexts[1].text = "Use phone less than 3 times but make 2 calls";
		taskTexts[2].text = "Don't get Caught";
		taskTexts[3].text = "Reach " + today.viewersNeeded + " views. View num: " + views;
		taskTexts[4].text = "Match " + today.clientMatchesNeeded + " Client Tasks. " + "Clients matched: " + clientsMatched;
	}

	bool CheckTasksComplete()
	{
		// check each task
		today.taskComplete[0] = viewership.viewers >= today.viewersNeeded; // viewers 
		today.taskComplete[1] = clientmatch.clientMatched >= today.clientMatchesNeeded; // client matches
		today.taskComplete[2] = emailsSent >= today.emailsNeeded; // emails sent
		bool morningEmailsSent = true;
		foreach (int id in today.morningEmailIDs)
		{
			if (!emailloader.emails[id].isSent)
			{
				morningEmailsSent = false;
			}
		}
		today.taskComplete[3] = morningEmailsSent; // morning emails sent
		bool eveningEmailsSent = true;
		foreach (int id in today.eveningEmailIDs)
		{
			if (!emailloader.emails[id].isSent)
			{
				eveningEmailsSent = false;
			}
		}
		today.taskComplete[4] = eveningEmailsSent; // evening emails sent


		// Display
		for (int i = 0;i<today.tasks;i++)
		{
			if (today.taskComplete[i])
			{
				taskTexts[i].fontStyle = FontStyles.Strikethrough;
				taskCircles[i].GetComponent<Image>().color = new Color32(255, 195, 0, 100);
			}
		}


		// finalize checking
		bool allTasksComeplete = true;
		foreach(bool taskComplete in today.taskComplete)
		{
			if (!taskComplete)
			{
				allTasksComeplete = false;
			}
		}

		return allTasksComeplete;
	}
}
