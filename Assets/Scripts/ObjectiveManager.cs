using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
	public TMPro.TextMeshProUGUI objList;

	public int hour; // from 9 to 17 [System.NonSerialized] 
	 public int minute; // 0 - 59 [System.NonSerialized]
	 public int day = 1; // 1-5 [System.NonSerialized]
	[SerializeField] TextMeshProUGUI timeText;
	public GameObject viewrshipManager;
	public GameObject mailScreen;

	EmailLoader emailloader;
	Viewship viewership;

	public TMPro.TextMeshProUGUI task1;
	public TMPro.TextMeshProUGUI task2;
	public TMPro.TextMeshProUGUI task3;
	public TMPro.TextMeshProUGUI task4;

	private bool task1complete = false;
	private bool task2complete = false;
	private bool task3complete = false;
	private bool task4complete = false;


	//in theory i would import the days from the day manager 
	private bool day1 = false;
	private bool day2 = false;
	private bool day3 = false;
	private bool day4 = false;
	private bool day5 = false;

	private float viewBenchmark = 0;
	private int emailNumBenchmark = 0;
	private int phoneNumBenchmark = 0;
	private bool caught = false;

	private float views = 0;
	private float sentmails = 0;
	float timer = 1f; //seconds

	void Start()
	{
		hour = 9;
		minute = 0;
		viewership = viewrshipManager.GetComponent<Viewship>();
		emailloader = mailScreen.GetComponent<EmailLoader>();

		sentmails = emailloader.emailIndex;

		objList.text = "vibe. no objectives yet";
		day1 = true;

		DAY1();

		viewBenchmark = 1000;
		emailNumBenchmark = 1;
		task2complete = true;

		


	}

	void Update()
	{
		string hourStr;
		string minuteStr;
		if (hour<10)
		{
			 hourStr = "0"+hour.ToString();
		}
		else
		{
			 hourStr = hour.ToString();
		}
		if (minute<10)
		{
			 minuteStr = "0" + minute.ToString();
		}
		else
		{
			 minuteStr = minute.ToString();
		}
		timeText.text = hourStr + ":" + minuteStr;
		TimeProgress();


		views = viewership.viewers;
		sentmails = emailloader.emailIndex;

		if (views > viewBenchmark)
		{
			task4complete = true;

		}

		if (sentmails > emailNumBenchmark)
		{
			task1complete = true;
		}
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
		switch (day)
		{
			case 1:

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
	void DAY1()
	{
		objList.text = "";
		task1.text = " - " + emailNumBenchmark + " emails";
		task2.text = " - Use phone less than 3 times but make 2 calls";
		task3.text = " - Don't get Caught";
		task4.text = " - reach " + viewBenchmark + " views. View num: " + views;


		if (task1complete == true)
		{
			task1.fontStyle = FontStyles.Strikethrough;

		}
		else if (task1complete == false)
		{
			task1.fontStyle = FontStyles.Bold;
		}

		if (task2complete == true)
		{
			task2.fontStyle = FontStyles.Strikethrough;

		}
		else if (task2complete == false)
		{
			task2.fontStyle = FontStyles.Bold;
		}

		if (task3complete == true)
		{
			task3.fontStyle = FontStyles.Strikethrough;

		}
		else if (task3complete == false)
		{
			task3.fontStyle = FontStyles.Bold;
		}

		if (task4complete == true)
		{
			task4.fontStyle = FontStyles.Strikethrough;

		}
		else if (task4complete == false)
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

}
