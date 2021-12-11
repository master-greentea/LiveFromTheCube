using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchPlayer : MonoBehaviour
{
    public int suspicionCount;
    public GameObject boss;
    public MeshRenderer bossRenderer;
    public bool playing;
    public List<GameObject> spawnLocations = new List<GameObject>();
    private bool CR_ROLL_running;
    private bool CR_BOSS_running;
    private bool firstPlayDone;
    private bool mayReduceSus; //might not need this if I just attach the tasks to if the player is not playing, aka tabbed
    private float counter;

    [Header("Roll Values")]

    [Tooltip("System rolls between one and this number to determine when the boss will activate.")]
    public int maxRange;

    [Tooltip("The number that activates the roll. This number has to be within the range (between 1 to Max Range) to activate.")]
    public int activateNum = 2;

    public int suspicionGain = 1;
    public int suspicionLoss = 1;
    public int staringSuspicion = 10;

    public float timeBetweenRolls = 1.0f;

    void Start()
    {
        bossRenderer.enabled = false;
        playing = false;
        firstPlayDone = false;
        mayReduceSus = false;
        suspicionCount = staringSuspicion;
    }


    // Update is called once per frame
    void Update()
    {
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
            while (suspicionCount < 100 && playing == true)
            {
                suspicionCount += suspicionGain;
                mayReduceSus = false;

                Debug.Log(suspicionCount);
                yield return new WaitForSeconds(1);
            }

            while (suspicionCount < 100 && playing == false && suspicionCount > 0)
            {
                suspicionCount += Mathf.RoundToInt(suspicionGain/2);

                mayReduceSus = true;

                Debug.Log(suspicionCount);
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
                suspicionCount = staringSuspicion;
            }
            else if (suspicionCount >= 100)
            {
                Debug.Log("The game ended.");
                //game should end here
                yield return null;
                StopCoroutine(rollBoss());
                CR_BOSS_running = false;
            }
        }
    }

    public void GameStart()
    {
        playing = true;
        firstPlayDone = true;

    }

    public void GameResume()
    {
        playing = true;
    }

    public void GameStopped()
    {
        playing = false;
    }

    public void ReduceSus()
    {
        if (mayReduceSus == true)
        {
            suspicionCount -= suspicionLoss;
        }
    }
}
