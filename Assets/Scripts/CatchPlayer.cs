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
	bool mayReduceSus; //might not need this if I just attach the tasks to if the player is not playing, aka tabbed
	float counter;

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

	/*
	void Start() {
		bossRenderer.enabled = false;
		playing = false;
		firstPlayDone = false;
		mayReduceSus = false;

		suspicionCount = startingSuspicion;
	}
	*/


	void Update()
	{

		discoVar = discolights.GetComponent<DiscoLights>().lightSwitched;
		playing = rhythmGameScreen.activeInHierarchy; //first play done stops the boss from firing before the player presses play the first time


		while (bossRenderer.enabled == false && CR_ROLL_running == false && firstPlayDone == true) {
			StartCoroutine(rollBoss());
			//if game is active roll where boss should spawn 

		}

		//counter += 1 * Time.deltaTime;
		//Debug.Log("It has been " + Mathf.RoundToInt(counter) + "seconds");

		if (bossRenderer.enabled == true && CR_BOSS_running == false) {
			StartCoroutine(BossActive());
		}
	}

	IEnumerator Start() {
		var point = transform.position;

		bossRenderer.enabled = false;
		playing = false;
		firstPlayDone = false;
		mayReduceSus = false;

		suspicionCount = startingSuspicion;

		while (discoVar == false) {
			yield
			return new WaitForSeconds(0.2f);

			while (discoVar == true) {
				Debug.Log("moving");
				yield
				return StartCoroutine(MoveObject(transform, pointA, pointB, point, speed));
				yield
				return StartCoroutine(MoveObject(transform, pointB, pointA, point, speed));
			}
		}

		yield
		return new WaitForSeconds(1f);
	}

	IEnumerator rollBoss(){
		CR_ROLL_running = true;

		int rollNum = 0;
		int spawnRollNum = Random.Range(0, spawnLocations.Count);

		while (rollNum != activateNum){
			rollNum = Random.Range(1, maxRange);
			yield return new WaitForSeconds(timeBetweenRolls);

		}

		if (rollNum == activateNum){
			boss.transform.position = spawnLocations[spawnRollNum].transform.position;
			bossRenderer.enabled = true;

			Debug.Log(spawnRollNum);

			StopCoroutine(rollBoss());
			yield return null;
			CR_ROLL_running = false;
		}
	}

	IEnumerator BossActive(){
		GetComponent<AudioSource>().Play(); // play boss aduio

		CR_BOSS_running = true;
		while (bossRenderer.enabled == true){

			mangaLines.SetActive(true);

			while (suspicionCount < 100 && playing == true){
				suspicionCount += suspicionGain;
				mayReduceSus = false;

				Debug.Log(suspicionCount);
				yield return new WaitForSeconds(1);
			}

			while (suspicionCount < 100 && playing == false && suspicionCount > 0) {
				suspicionCount -= suspicionLoss;

				mayReduceSus = true;

				Debug.Log(suspicionCount);
				yield return new WaitForSeconds(1);
			}


			if (suspicionCount <= 0){
				Debug.Log("The boss is satisfied.");

				yield return null;
				StopCoroutine(rollBoss());

				GetComponent<AudioSource>().Stop();// stop boss audio
												   
				bossRenderer.enabled = false;
				CR_BOSS_running = false;

				suspicionCount = startingSuspicion;


			}else if (suspicionCount >= 100){
				SceneManager.LoadScene("Failed Scene");
				//game should end here

				yield return null;
				StopCoroutine(rollBoss());
				CR_BOSS_running = false;
			}
		}

		mangaLines.SetActive(false);
	}

	IEnumerator MoveObject(Transform thisTransform, Vector3 startPos, Vector3 endPos, Vector3 point, float time) {

		var i = 0.0f;
		var rate = 3.0f / time;

		while (i < 1.0f) {
			i += Time.deltaTime * rate;
			thisTransform.position = Vector3.Lerp(endPos, startPos, i);
			yield
			return null;
		}
		

	}



	public void GameStart(){
		playing = true;
		firstPlayDone = true;

	}

	public void ReduceSus(float loss) {
		if (mayReduceSus == true) {
			suspicionCount -= loss;

		}
	}
}
