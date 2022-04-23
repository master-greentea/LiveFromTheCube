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
		[NonSerialized] public int[] morningEmailIDs; // task 3
		[NonSerialized] public int[] eveningEmailIDs; // task 4
		[NonSerialized] public bool[] tasksComplete;

	}

	[ContextMenu("Add Day Component")]
	private void AddNested()
	{
		gameObject.AddComponent<Day>();
	}


	[SerializeField]  Day[] days;
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

	EmailLoader emailLoader;
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


	private float viewBenchmark = 0;
	private int emailNumBenchmark = 0;
	private int phoneNumBenchmark = 0;
	private int clientNumBenchmark = 0;
	private bool caught = false;

	private float views = 0;
	private float emailsSent = 0;
	private float clientsMatched = 0;
	float timer = 1f; //seconds


	void Start()
	{
		// Objects initialize 
		viewership = viewrshipManager.GetComponent<Viewship>();
		emailLoader = mailScreen.GetComponent<EmailLoader>();
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
		today.tasksComplete = new bool[today.tasks];

		taskCircles[1].GetComponent<Image>().color = new Color32(255, 195, 0, 100);
		taskCircles[2].GetComponent<Image>().color = new Color32(255, 195, 0, 100);

	}

	void Update()
	{


		TimeProgress();
		UpdateText();
	}

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
			ThingsToDoByMinute();
			views = viewership.viewers;
			UIviews.text = viewership.viewers + "";
			clientsMatched = clientmatch.clientMatched;
			CheckTasksComplete();
			
			if (minute >= 60)
			{

				hour++;
				
				minute = 0;
			}
			timer = 1f;
		}

	}

	private void ThingsToDoByMinute()
	{
		if (hour == 15 && minute == 0)
		{
			emailLoader.InsertStoryEmails(day, 15); // add evening emails at 15:00
		}
		if (hour == 9 && minute == 1)
		{
			emailLoader.InsertStoryEmails(day, 9);
			Debug.Log("morning emails inserted");
		}
		if (hour >= 17 && minute == 0) // day ends at 17:00
		{
			hour = 9;
			DayProgress();
		}
	}

	void DayProgress()
	{
		today = days[dateOfToday];
		today.tasksComplete = new bool[today.tasks];
		emailsSent = 0;
		clientsMatched = 0;


	}
	public void EmailSent()
	{
		emailsSent++;
	}
	void UpdateText()
	{
		taskTexts[0].text = "Send" + today.emailsNeeded + " emails. Emails sent: "+emailsSent;
		taskTexts[1].text = "Treat yourself with a new decor!";
		taskTexts[2].text = "Reply to your morning and evening emails!";
		taskTexts[3].text = "Reach " + today.viewersNeeded + " views. View num: " + views;
		taskTexts[4].text = "Match " + today.clientMatchesNeeded + " Client Tasks. " + "Clients matched: " + clientsMatched;
	}

	bool CheckTasksComplete()
	{
		// check each task
		today.tasksComplete[3] = viewership.viewers >= today.viewersNeeded; // viewers 
		today.tasksComplete[4] = clientmatch.clientMatched >= today.clientMatchesNeeded; // client matches
		today.tasksComplete[0] = emailsSent >= today.emailsNeeded; // emails sent
		today.tasksComplete[2] = emailLoader.TodayStoryEmailsReplied(day);


		// Display
		for (int i = 0;i<today.tasks;i++)
		{
			if (today.tasksComplete[i])
			{
				taskTexts[i].fontStyle = FontStyles.Strikethrough;
				taskCircles[i].GetComponent<Image>().color = new Color32(255, 195, 0, 100);
			}
		}


		// finalize checking
		bool allTasksComeplete = true;
		foreach(bool taskComplete in today.tasksComplete)
		{
			if (!taskComplete)
			{
				allTasksComeplete = false;
			}
		}
		return allTasksComeplete;
		
	}
}
