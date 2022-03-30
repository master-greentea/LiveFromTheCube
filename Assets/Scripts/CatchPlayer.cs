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
	public float startingSuspicion = 10;

	public float timeBetweenRolls = 1.0f;

	
	void Update()
	{

		Debug.Log("sus "+ suspicionCount);


		discoVar = discolights.GetComponent<DiscoLights>().lightSwitched;
		playing = rhythmGameScreen.activeInHierarchy; //first play done stops the boss from firing before the player presses play the first time


		if (bossRenderer.enabled == false && CR_ROLL_running == false && firstPlayDone == true)
		{
			Debug.Log("roll boss");
			StartCoroutine(rollBoss());
			//if game is active roll where boss should spawn 

		}

		/*
		if (bossRenderer.enabled == true && CR_BOSS_running == false && suspicionCount > 60 == true)
		{
			StartCoroutine(BossActive());
		}
		*/

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
		}
		else if (suspicionCount >= 100) // game ends
		{
			SceneManager.LoadScene("Failed Scene");
			StopCoroutine(rollBoss());
			CR_BOSS_running = false;
		}
	}

	void Start()
	{
		point = transform.position;
		bossRenderer.enabled = false;
		playing = false;
		firstPlayDone = false;

		suspicionCount = startingSuspicion;
		
		/*
		while (discoVar == false)
		{
			yield return new WaitForSeconds(0.2f);

			//while (discoVar == true) {
			while (suspicionCount < 70)
			{
				suspicionCount += suspicionGain;

				yield return StartCoroutine(MoveObject(transform, pointA, pointB, point, speed));
				yield return StartCoroutine(MoveObject(transform, pointB, pointA, point, speed));
			}
		}

		yield return new WaitForSeconds(1f);
		*/
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

			

			/*
			while (suspicionCount < 100 && playing == true)
			{
				suspicionCount += suspicionGain;

				yield return new WaitForSeconds(1);
			}

			while (suspicionCount < 100 && playing == false && suspicionCount > 0)
			{
				suspicionCount -= suspicionLoss;

				Debug.Log(suspicionCount);
				yield return new WaitForSeconds(1);
			}


			if (suspicionCount <= 0)
			{
				Debug.Log("The boss is satisfied.");

				yield return null;
				StopCoroutine(rollBoss());

				GetComponent<AudioSource>().Stop();// stop boss audio

				bossRenderer.enabled = false;
				CR_BOSS_running = false;

				suspicionCount = startingSuspicion;


			}
			else if (suspicionCount >= 100)
			{
				SceneManager.LoadScene("Failed Scene");
				//game should end here

				yield return null;
				StopCoroutine(rollBoss());
				CR_BOSS_running = false;
			}*/
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
}
