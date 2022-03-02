using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

	[Header("Roll Values")]

	[Tooltip("System rolls between one and this number to determine when the boss will activate.")]
	public int maxRange;

	[Tooltip("The number that activates the roll. This number has to be within the range (between 1 to Max Range) to activate.")]
	public int activateNum = 2;

	public float suspicionGain;
	public float suspicionLoss;
	public float startingSuspicion = 10;

	public float timeBetweenRolls = 1.0f;

	void Start()
	{
		bossRenderer.enabled = false;
		playing = false;
		firstPlayDone = false;
		suspicionCount = startingSuspicion;
	}


	// Update is called once per frame
	void Update()
	{
		playing = rhythmGameScreen.active;
		//first play done stops the boss from firing before the player presses play the first time
		while (bossRenderer.enabled == false && CR_ROLL_running == false && firstPlayDone == true)
		{
			StartCoroutine(rollBoss());
		}

		//counter += 1 * Time.deltaTime;
		//Debug.Log("It has been " + Mathf.RoundToInt(counter) + "seconds");

		if (bossRenderer.enabled == true && CR_BOSS_running == false)
		{
			StartCoroutine(BossActive());
		}
	}

	IEnumerator rollBoss()
	{
		CR_ROLL_running = true;

		int rollNum = 0;
		int spawnRollNum = Random.Range(0, spawnLocations.Count);
		Debug.Log(spawnRollNum);

		while (rollNum != activateNum)
		{
			//Debug.Log(rollNum);
			rollNum = Random.Range(1, maxRange);
			yield return new WaitForSeconds(timeBetweenRolls);
		}

		if (rollNum == activateNum)
		{
			boss.transform.position = spawnLocations[spawnRollNum].transform.position;
			bossRenderer.enabled = true;
			Debug.Log(spawnRollNum);
			StopCoroutine(rollBoss());
			yield return null;
			CR_ROLL_running = false;
		}
	}

	IEnumerator BossActive()
	{
		// play boss aduio
		GetComponent<AudioSource>().Play();

		//
		CR_BOSS_running = true;
		while (bossRenderer.enabled == true)
		{

			mangaLines.SetActive(true);

			while (suspicionCount < 100 && playing == true)
			{
				suspicionCount += suspicionGain;

				yield return new WaitForSeconds(1);
			}

			while (suspicionCount < 100 && playing == false && suspicionCount > 0)
			{
				suspicionCount -= suspicionLoss;

				yield return new WaitForSeconds(1);
			}


			if (suspicionCount <= 0)
			{
				Debug.Log("The boss is satisfied.");
				//game should end here
				yield return null;
				StopCoroutine(rollBoss());
				// stop boss audio
				GetComponent<AudioSource>().Stop();
				//
				bossRenderer.enabled = false;
				CR_BOSS_running = false;
				suspicionCount = startingSuspicion;
			}
			else if (suspicionCount >= 100)
			{
				Debug.Log("The game ended.");
				SceneManager.LoadScene("Failed Scene");
				//game should end here
				yield return null;
				StopCoroutine(rollBoss());
				CR_BOSS_running = false;
			}
		}

		mangaLines.SetActive(false);
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
}
