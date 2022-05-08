using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CatchPlayer : MonoBehaviour
{
	[SerializeField] GameObject rhythmGameScreen;

	public float suspicionCount;
	public GameObject boss;

	public GameObject endScreen;
	public GameObject mangaLines;

	public MeshRenderer bossRenderer;
	public bool playing;

	public List<GameObject> spawnLocations = new List<GameObject>();

	bool CR_ROLL_running;
	bool CR_BOSS_running;
	bool firstPlayDone;
	float counter;

	Vector3 point;
	public Vector3 pointB;
	public Vector3 pointA;

	public GameObject discolights;
	public float speed; //higher the speed the slower he will move 
	public bool discoVar;

	[Header("Roll Values")]

	[Tooltip("System rolls between one and this number to determine when the boss will activate.")]
	public int maxRange;

	[Tooltip("The number that activates the roll. This number has to be within the range (between 1 to Max Range) to activate.")]
	public int activateNum = 2;

	public float suspicionGain;
	public float suspicionLoss;
	public float startingSuspicion = 0;

	public float timeBetweenRolls = 1.0f;

	[SerializeField] GameObject moneyManager;
	[SerializeField] float fineMultiplier; // the smaller the multiplier, the harsher the penalty
	[SerializeField] int fine;
	[SerializeField] Button bosuIcon;
	[SerializeField] GameObject bosu;
	[SerializeField] GameObject cycleManager;
	[SerializeField] float penaltyTimer;
	[SerializeField] Color _grayedOutColor;
	Color bosuColor;
	float penaltyTimerTemp;
	bool bosuDisabled;


	[SerializeField] float safeTimer;
	float safeTimerTemp;
	bool isSafe = true;

	void Update()
	{

		discoVar = discolights.GetComponent<DiscoLights>().lightSwitched;
		playing = rhythmGameScreen.activeInHierarchy; //first play done stops the boss from firing before the player presses play the first time

		if (playing && firstPlayDone && isSafe)
		{
			safeTimerTemp -= Time.deltaTime;
		}
		else
		{
			safeTimerTemp = safeTimer;
		}
		
		if (safeTimerTemp <= 0 && playing && firstPlayDone)
		{
			isSafe = false;
			safeTimerTemp = safeTimer;
		}


		if (!bossRenderer.enabled && !CR_ROLL_running && firstPlayDone && !isSafe)
		{
			Debug.Log("roll boss");
			StartCoroutine(rollBoss());
			//if game is active roll where boss should spawn 

		}


		if (suspicionCount < 100 && playing && bossRenderer.enabled)
		{
			suspicionCount += suspicionGain * Time.deltaTime;
		}
		if (suspicionCount < 100 && !playing && suspicionCount > 0)
		{
			suspicionCount -= suspicionLoss * Time.deltaTime;
		}

		if (suspicionCount <= 0)
		{
			StopCoroutine(rollBoss());

			GetComponent<AudioSource>().Stop();// stop boss audio
			mangaLines.SetActive(false);
			bossRenderer.enabled = false;
			CR_BOSS_running = false;

			suspicionCount = startingSuspicion;
			isSafe = true;
		}
		else if (suspicionCount >= 100)
		{
			// boss logic
			suspicionCount = 0;
			StopCoroutine(rollBoss());
			GetComponent<AudioSource>().Stop();// stop boss audio
			mangaLines.SetActive(false);
			bossRenderer.enabled = false;
			CR_BOSS_running = false;
			isSafe = true;

			// grey out bosu
			bosuIcon.enabled = false;
			bosu.SetActive(false);
			cycleManager.GetComponent<SwitchScreen>().apps[1] = cycleManager.GetComponent<SwitchScreen>().apps[3];
			bosuDisabled = true;
			bosuColor = bosuIcon.GetComponent<Image>().color;
			bosuIcon.GetComponent<Image>().color = _grayedOutColor;

			// money punishment
			/*
			SceneManager.LoadScene("Failed Scene");
			StopCoroutine(rollBoss());*/
			//moneyManager.GetComponent<CurrencySystem>().money = Mathf.RoundToInt(moneyManager.GetComponent<CurrencySystem>().money * fineMultiplier);
			moneyManager.GetComponent<CurrencySystem>().money -= fine;

		}
		if(bosuDisabled)
		{
			PenaltyTimerOn();
		}	
	}

	void Start()
	{
		point = transform.position;
		bossRenderer.enabled = false;
		playing = false;
		firstPlayDone = false;

		suspicionCount = startingSuspicion;
		penaltyTimerTemp = penaltyTimer;

	}

	IEnumerator rollBoss()
	{
		CR_ROLL_running = true;

		int rollNum = 0;
		int spawnRollNum = Random.Range(0, spawnLocations.Count);

		while (rollNum != activateNum)
		{
			rollNum = Random.Range(1, maxRange);
			yield return new WaitForSeconds(timeBetweenRolls);

		}

		if (rollNum == activateNum)
		{
			boss.transform.position = spawnLocations[spawnRollNum].transform.position;
			bossRenderer.enabled = true;
			GetComponent<AudioSource>().Play(); // play boss aduio
			mangaLines.SetActive(true);
			StartCoroutine(MoveObject(transform, pointA, pointB, point, speed));
			StopCoroutine(rollBoss());
			yield return null;
			CR_ROLL_running = false;
		}
	}

	IEnumerator BossActive()
	{


		CR_BOSS_running = true;
		if (bossRenderer.enabled == true)
		{



		}

		yield return null;
	}

	IEnumerator MoveObject(Transform thisTransform, Vector3 startPos, Vector3 endPos, Vector3 point, float time)
	{

		var i = 0.0f;
		var rate = 3.0f / time;

		while (i < 1.0f)
		{
			i += Time.deltaTime * rate;
			thisTransform.position = Vector3.Lerp(endPos, startPos, i);
			yield return null;
		}


	}



	public void GameStart()
	{
		playing = true;
		firstPlayDone = true;

	}

	public void ReduceSus(float loss)
	{
		suspicionCount -= loss;
	}

	void PenaltyTimerOn()
	{
		penaltyTimerTemp -= Time.deltaTime;
		if (penaltyTimerTemp <= 0)
		{
			bosuIcon.enabled = true;
			cycleManager.GetComponent<SwitchScreen>().apps[1] = bosu;
			penaltyTimerTemp = penaltyTimer;
			bosuIcon.GetComponent<Image>().color = bosuColor;
			bosuDisabled = false;
		}
	}
}
