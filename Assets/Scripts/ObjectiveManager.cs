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
		[SerializeField] public int decorsNeeded; // t
		[NonSerialized] public int[] morningEmailIDs; // task 3
		[NonSerialized] public int[] eveningEmailIDs; // task 4
		[NonSerialized] public bool[] tasksComplete;
		[NonSerialized] public int views;
		[NonSerialized] public int emailsSent;
		[NonSerialized] public int clientsMatched;
		[NonSerialized] public int decorsBought;

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
			today.views = viewership.viewers;
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


	}
	public void EmailSent()
	{
		today.emailsSent++;
	}
	public void DecorBought()
	{
		today.decorsBought++;
	}
	void UpdateText()
	{
		taskTexts[0].text = "Send" + today.emailsNeeded + " emails. Emails sent: "+today.emailsSent;
		taskTexts[1].text = "Reply to your morning and evening emails!";
		taskTexts[2].text = "Treat yourself with " +  "new decor!";
		taskTexts[3].text = "Reach " + today.viewersNeeded + " views. View num: " + today.views;
		taskTexts[4].text = "Match " + today.clientMatchesNeeded + " Client Tasks. " + "Clients matched: " + clientsMatched;
	}

	bool CheckTasksComplete()
	{
		// check each task
		today.tasksComplete[0] = emailsSent >= today.emailsNeeded; // emails sent
		today.tasksComplete[1] = emailLoader.TodayStoryEmailsReplied(day);
		today.tasksComplete[2] = today.decorsBought >= today.decorsNeeded;
		today.tasksComplete[3] = viewership.viewers >= today.viewersNeeded; // viewers 
		today.tasksComplete[4] = clientmatch.clientMatched >= today.clientMatchesNeeded; // client matches
		



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
