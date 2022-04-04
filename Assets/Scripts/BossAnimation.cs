using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class BossAnimation : MonoBehaviour
{
    private Vector3 startHeight;
    private Vector3 targetHeight;
    private CatchPlayer sus;
    public Slider slider; 

    public GameObject susManager;
    public float timeBetweenPoints = .5f;
    public float suspicionLevel;

    private Vector3 velocity = Vector3.zero;

    public int maxHealth = 1;
    public int currentHealth; 

    public void SetMaxHealth(int health) {
        slider.maxValue = health;
        slider.value = health; 
    }

    public void SetHealth(int health) {
        slider.value = health; 
    }

    void Start()
    {
        sus = susManager.GetComponent<CatchPlayer>();
        startHeight = new Vector3 (0,0,0) + new Vector3 (0,transform.position.y,0);
        targetHeight = startHeight + new Vector3(0,15,0);

        currentHealth = 0; 
    }

    void Update()
    {
        var currentPosition = transform.position;
        var lerpInterpo = (float)sus.suspicionCount * .01f; // higher sus the higher guy goes up 
        var totalHeightToMove = Vector3.Lerp(startHeight, targetHeight, lerpInterpo);
        var newHeightToMove = new Vector3(transform.position.x, totalHeightToMove.y, transform.position.z);

        //Debug.Log(lerpInterpo);

        suspicionLevel = lerpInterpo;
        slider.value = suspicionLevel;
//        Debug.Log(suspicionLevel);

        transform.position = Vector3.SmoothDamp(currentPosition, newHeightToMove, ref velocity, timeBetweenPoints);
    }
}
