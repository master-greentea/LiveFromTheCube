using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RhythmGameStarter;

public class Viewship : MonoBehaviour
{

    public GameObject rhythmGame;
    public GameObject mailScreen;

    public TextMeshProUGUI viewerDisplay;

    public static int viewers;
    public int defaultViewerNum; 

    StatsSystem statsystem;

    

    void Start()
    {
        viewerDisplay.text = ""+viewers;
        viewers = defaultViewerNum;
        statsystem = rhythmGame.GetComponent<StatsSystem>();
    }

    void Update()
    {

        viewerDisplay.text = "" + viewers;

        if(mailScreen.activeInHierarchy == true) {
            ifworking();

        } else {
            viewers += statsystem.score + defaultViewerNum;
        }
    }

    void ifworking() {
        viewers = viewers - 10; 
    }
}
