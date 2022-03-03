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

    public int viewers;
    public int startingViewerNum; 
    public Vector2 viewerGrowthOvertimeRange;
    public Vector2 viewerLostOvertimeRange;

    StatsSystem statsystem;

    // timers
    float viewerGrowthTimer;
    float scoreBasedViewerGrowthTimer;
    float loseViewerTimer;

    int lastViewerCount;

    void Start()
    {
        viewerDisplay.text = ""+viewers;
        viewers = startingViewerNum;
        statsystem = rhythmGame.GetComponent<StatsSystem>();
    }

    void Update()
    {

        viewerDisplay.text = "" + viewers;

        if(mailScreen.activeInHierarchy == true) {
            LoseViewer(); // per .5 sec in mail

        } else {
            ScoreBasedViewerGrowth(); // based on score
        }

        RandomViewerGrowth(); // per .5 sec

        if (viewers < 0) viewers = 0; // clamp
    }

    void FixedUpdate() {
        lastViewerCount = viewers;
    }

    void LoseViewer() {
        if (loseViewerTimer > .5f) {
            viewers = viewers - Random.Range((int)viewerLostOvertimeRange.x, (int)viewerLostOvertimeRange.y);
            loseViewerTimer = 0;
        }
        loseViewerTimer += Time.deltaTime;
    }

    void ScoreBasedViewerGrowth() {
        if (scoreBasedViewerGrowthTimer > 1) {
            viewers += statsystem.score - lastViewerCount;
            scoreBasedViewerGrowthTimer = 0;
        }
        scoreBasedViewerGrowthTimer += Time.deltaTime;
    }

    void RandomViewerGrowth() {
        if (viewerGrowthTimer > .5f) {
            viewers += Random.Range((int)viewerGrowthOvertimeRange.x, (int)viewerGrowthOvertimeRange.y);
            viewerGrowthTimer = 0;
        }
        viewerGrowthTimer += Time.deltaTime;
    }
}
