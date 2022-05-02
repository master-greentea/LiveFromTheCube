using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RhythmGameStarter;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

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


	[SerializeField] Day[] days;
	Day today;
	int dateOfToday = -1;

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


	[SerializeField] GameObject dayEndUI;
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
	[SerializeField] float timer; //seconds
	float timerTemp;


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


	}

	void Update()
	{


		TimeProgress();
		UpdateText();
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
		timerTemp -= Time.deltaTime;
		if (timerTemp <= 0)
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
			timerTemp = timer;
		}

	}

	private void ThingsToDoByMinute()
	{
		if (minute == 1)
		{
			emailLoader.InsertStoryEmails(day, hour); // add evening emails at 15:00
			Debug.Log("story emails inserted");
		}
		if (hour >= 17 && minute == 1) // day ends at 17:00
		{
			
			dayEnd();
			//DayProgress();
		}
	}
	void dayEnd()
	{
		Debug.Log("day end triggered");
		//Time.timeScale = 0;
		if (today.index == 4) // final ending
		{

		}


		if (!today.tasksComplete[0] || !today.tasksComplete[1] || !today.tasksComplete[4]) // if any of the company tasks are not done
		{
			SceneManager.LoadScene("Failed Scene");
		}
		else if (!today.tasksComplete[2] || !today.tasksComplete[3]) // if any of the streaming tasks are not done
		{
			fade.SetActive(true);
			dayEndUI.SetActive(true);
			maxComboUI.text = statsystem.maxCombo + "!";
			highestViewsUI.text = today.views + " views";
			moneyEarnedUI.text = "$" + currensys.money + "";
			susReportUI.text = "Make sure you finish your streaming tasks!";
		}
		else if (CheckAllTasksComplete()) // perfect day end
		{
			fade.SetActive(true);
			dayEndUI.SetActive(true);
			maxComboUI.text = statsystem.maxCombo + "!";
			highestViewsUI.text = today.views + " views";
			moneyEarnedUI.text = "$" + currensys.money + "";
			susReportUI.text = "Perfect Day!";
		}
		else
		{
			Debug.Log("you dumb ass can't even do simple boolean logic");
		}



	}

	public void DayProgress()
	{
		Time.timeScale = 1;
		hour = 9;
		minute = 0;
		fade.SetActive(false);
		dateOfToday++;
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
		taskTexts[0].text = "Send" + today.emailsNeeded + " emails. Emails sent: " + today.emailsSent;
		taskTexts[1].text = "Reply to your morning and evening emails!";
		taskTexts[2].text = "Treat yourself with " + today.decorsNeeded + "new decors! Decors bought: " + today.decorsBought;
		taskTexts[3].text = "Reach " + today.viewersNeeded + " views. Views: " + today.views;
		taskTexts[4].text = "Match " + today.clientMatchesNeeded + " Client Tasks. " + "Clients matched: " + clientsMatched;
	}

	void CheckTasksComplete()
	{
		// check each task
		today.tasksComplete[0] = today.emailsSent >= today.emailsNeeded; // emails sent
		today.tasksComplete[1] = emailLoader.TodayStoryEmailsReplied(day);
		today.tasksComplete[2] = today.decorsBought >= today.decorsNeeded;
		today.tasksComplete[3] = viewership.viewers >= today.viewersNeeded; // viewers 
		today.tasksComplete[4] = clientmatch.clientMatched >= today.clientMatchesNeeded; // client matches




		// Display
		for (int i = 0; i < today.tasks; i++)
		{
			if (today.tasksComplete[i])
			{
				taskTexts[i].fontStyle = FontStyles.Strikethrough;
				taskCircles[i].GetComponent<Image>().color = new Color32(255, 195, 0, 100);
			}
			else
			{
				taskTexts[i].fontStyle = FontStyles.Normal;
				taskCircles[i].GetComponent<Image>().color = new Color32(0, 0, 0, 100);
			}
		}



	}
	bool CheckAllTasksComplete()
	{

		// finalize checking
		bool allTasksComeplete = true;
		foreach (bool taskComplete in today.tasksComplete)
		{
			if (!taskComplete)
			{
				allTasksComeplete = false;
			}
		}
		return allTasksComeplete;
	}

}
