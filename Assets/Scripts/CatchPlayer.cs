using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchPlayer : MonoBehaviour
{
    private float suspicionCount;
    public MeshRenderer boss;
    private bool playing;
    private bool CR_running;

    void Start()
    {
        boss.enabled = false;
        playing = false;
        suspicionCount = 0.0f;
    }


    // Update is called once per frame
    void Update()
    {
        while (playing == true)
        {
            if (suspicionCount < 100)
            {
                suspicionCount += Time.deltaTime;
                Debug.Log(suspicionCount);
            }
        }

        while (boss.enabled == false)
        {
            StartCoroutine(rollBoss());
        }
    }

    IEnumerator rollBoss()
    {
        CR_running = true;
        var rollNum = Random.Range(1,10);
        var activateNum = 10;

        while (rollNum != activateNum)
        {
            Debug.Log(rollNum);
            rollNum = Random.Range(1, 10);
            yield return new WaitForSeconds(1.0f);
        }

        Debug.Log(rollNum);
        if (rollNum == activateNum)
        {
            boss.enabled = true;
            StopCoroutine(rollBoss());
            yield return null;
            CR_running = false;
        }
    }
}
