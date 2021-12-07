using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchScreen : MonoBehaviour
{
    public GameObject gameCanvas;
    private CatchPlayer catchPlayer;

    // Start is called before the first frame update
    void Start()
    {
        catchPlayer = GetComponent<CatchPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            gameCanvas.SetActive(!gameCanvas.activeInHierarchy);
            catchPlayer.playing = !catchPlayer.playing;
        }
    }
}
