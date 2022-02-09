using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class startButtonScript : MonoBehaviour
{

    public Button startscreen;
    public GameObject player;

    Animator otherAnimator;

    // Start is called before the first frame update
    void Start()
    {

        startscreen.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void TaskOnClick() {
        //Output this to console when Button1 or Button3 is clicked
        Debug.Log("You have clicked the button!");

        player.GetComponent<Animation>().Play();
    }
 }
