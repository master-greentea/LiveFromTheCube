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
	[SerializeField] public int dateOfToday = -1;
	[SerializeField] GameObject computerScreen;
	[SerializeField] TMPro.TextMeshProUGUI objList;
	[SerializeField] GameObject BOSU;

	[NonSerialized] public int hour; // from 9 to 17 [System.NonSerialized] 
	[NonSerialized] public int minute; // 0 - 59 [System.NonSerialized]

	[SerializeField] TextMeshProUGUI timeText;
	[SerializeField] GameObject viewrshipManager;
	[SerializeField] GameObject clientManager;
	[SerializeField] GameObject currencyManager;
	[SerializeField] GameObject mailScreen;
	[SerializeField] GameObject confetti;
	[SerializeField] GameObject fade;

	[SerializeField] GameObject calendar;
	[SerializeField] GameObject monday;
	[SerializeField] GameObject tuesday;
	[SerializeField] GameObject wednesday;
	[SerializeField] GameObject thursday;
	[SerializeField] GameObject friday;

	

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
	[SerializeField] TMPro.TextMeshProUGUI companiesMatchedUI;
	[SerializeField] TMPro.TextMeshProUGUI emailsSentUI;

	private int maxCombo;
	private float highestView;
	private int moneyEarned;


	public Slider slider;

	[SerializeField] TMPro.TextMeshProUGUI UItime;
	[SerializeField] TMPro.TextMeshProUGUI UIviews;
	[SerializeField] TMPro.TextMeshProUGUI UIviews2;
	[SerializeField] TMPro.TextMeshProUGUI UImoney;


	// the list of tasks
	[SerializeField] GameObject[] taskCircles;
	[SerializeField] TextMeshProUGUI[] taskTexts;


	private float views = 0;
	private float emailsSent = 0;
	private float clientsMatched = 0;
	[SerializeField] float timer; //seconds
	float timerTemp;

	[SerializeField] GameObject finalSceneManager;

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	void Start()
	{
		calendar.SetActive(true);
		if (dateOfToday  == 0) {
			monday.SetActive(true);

		} else if(dateOfToday == 1 ) {
			tuesday.SetActive(true);

		} else if (dateOfToday == 2) {
			wednesday.SetActive(true);

		} else if (dateOfToday == 3) {
			thursday.SetActive(true);

		} else if (dateOfToday == 4) {
			friday.SetActive(true);
		}

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

		slider.maxValue = 480;

		// time setup
		hour = 9;
		minute = 0;

		// day setup
		days = days.OrderBy(c => c.index).ToArray(); // sort the days, in case they are placed in the wrong order
		DayProgress();
		days[dateOfToday].tasksComplete = new bool[days[dateOfToday].tasks];


	}

	void Update()
	{
		TimeProgress();
		UpdateText();
		timerBar();

		if (CheckAllTasksComplete() == true) {
			confetti.SetActive(true);
		}

	

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
		if (computerScreen.activeInHierarchy)
		{
			timerTemp -= Time.deltaTime;
			if (timerTemp <= 0)
			{

				minute++;
				ThingsToDoByMinute();
				days[dateOfToday].views = viewership.viewers;
				UIviews.text = viewership.viewers + "";
				UIviews2.text = viewership.viewers + "";
				UImoney.text = "$" + currensys.money + "";
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
		
		

	}

	private void timerBar()
	{
		slider.value = minute + ((hour - 9) * 60);

		//Debug.Log("sv" + slider.value);
	}

	private void ThingsToDoByMinute()
	{
		if (minute == 1)
		{
			emailLoader.InsertStoryEmails(dateOfToday, hour); // add evening emails at 15:00
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
		computerScreen.SetActive(false);
		BOSU.SetActive(false);
		//Time.timeScale = 0;



		if (!days[dateOfToday].tasksComplete[0] || !days[dateOfToday].tasksComplete[1] || !days[dateOfToday].tasksComplete[4]) // if any of the company tasks are not done
		{
			SceneManager.LoadScene("Failed Scene");
		}
		else if (!days[dateOfToday].tasksComplete[2] || !days[dateOfToday].tasksComplete[3]) // if any of the streaming tasks are not done
		{
			fade.SetActive(false);
			dayEndUI.SetActive(true);
			maxComboUI.text = statsystem.maxCombo + "!";
			highestViewsUI.text = days[dateOfToday].views + " views";
			moneyEarnedUI.text = "$" + currensys.money + "";
			companiesMatchedUI.text = "" + clientmatch.clientMatched + "";
			emailsSentUI.text = "" + days[dateOfToday].emailsSent + "";
			susReportUI.text = "Make sure you finish your streaming tasks!";
		}
		else if (CheckAllTasksComplete()) // perfect day end
		{
			if (days[dateOfToday].index == 4) // final ending
			{
				int viewsTotal = 0;
				int emailsSentTotal = 0;
				int clientsMatchedTotal = 0;
				int decorsBoughtTotal = 0;
				int comboMax = statsystem.maxCombo;
				int moneyRemaining = currensys.money;

				foreach (Day d in days)
				{
					viewsTotal += d.views;
					emailsSentTotal += d.emailsSent;
					clientsMatchedTotal += d.clientsMatched;
					decorsBoughtTotal += d.clientsMatched;

				}

				SceneManager.LoadScene("Final Scene");
				GameObject finalManager = Instantiate(finalSceneManager);
				finalSceneManager.GetComponent<FinalSceneManager>().SetFinalStats(viewsTotal, emailsSentTotal, clientsMatchedTotal, decorsBoughtTotal, comboMax, moneyRemaining);

			}
			fade.SetActive(false);
			dayEndUI.SetActive(true);
			maxComboUI.text = statsystem.maxCombo + "!";
			highestViewsUI.text = days[dateOfToday].views + "";
			moneyEarnedUI.text = "$" + currensys.money + "";
			companiesMatchedUI.text = "" + clientmatch.clientMatched + "";
			emailsSentUI.text = "" + days[dateOfToday].emailsSent + "";
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
		//fade.SetActive(false);
		dateOfToday++;
		days[dateOfToday].tasksComplete = new bool[days[dateOfToday].tasks];
		computerScreen.SetActive(true);


	}
	public void EmailSent()
	{
		days[dateOfToday].emailsSent++;
	}
	public void DecorBought()
	{
		days[dateOfToday].decorsBought++;
	}
	void UpdateText()
	{
		taskTexts[0].text = "Send" + days[dateOfToday].emailsNeeded + " emails. Emails sent: " + days[dateOfToday].emailsSent;
		taskTexts[1].text = "Reply to your morning and evening emails!";
		taskTexts[2].text = "Treat yourself with " + days[dateOfToday].decorsNeeded + "new decors! Decors bought: " + days[dateOfToday].decorsBought;
		taskTexts[3].text = "Reach " + days[dateOfToday].viewersNeeded + " views. Views: " + days[dateOfToday].views;
		taskTexts[4].text = "Match " + days[dateOfToday].clientMatchesNeeded + " Client Tasks. " + "Clients matched: " + clientsMatched;
	}

	void CheckTasksComplete()
	{
		// check each task
		days[dateOfToday].tasksComplete[0] = days[dateOfToday].emailsSent >= days[dateOfToday].emailsNeeded; // emails sent
		days[dateOfToday].tasksComplete[1] = emailLoader.TodayStoryEmailsReplied(dateOfToday);
		days[dateOfToday].tasksComplete[2] = days[dateOfToday].decorsBought >= days[dateOfToday].decorsNeeded;
		days[dateOfToday].tasksComplete[3] = viewership.viewers >= days[dateOfToday].viewersNeeded; // viewers 
		days[dateOfToday].tasksComplete[4] = clientmatch.clientMatched >= days[dateOfToday].clientMatchesNeeded; // client matches


		// Display
		for (int i = 0; i < days[dateOfToday].tasks; i++)
		{
			if (days[dateOfToday].tasksComplete[i])
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
		foreach (bool taskComplete in days[dateOfToday].tasksComplete)
		{
			if (!taskComplete)
			{
				allTasksComeplete = false;
			}
		}
		return allTasksComeplete;
	}

}
