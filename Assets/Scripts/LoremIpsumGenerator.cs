using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class LoremIpsumGenerator : MonoBehaviour
{
    public GameObject l1;
    public GameObject l2;
    public GameObject l3; 

    public Button startscreen;

    // Start is called before the first frame update
    void Start()
    {
        startscreen.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void TaskOnClick() {
        //Output this to console when Button1 or Button3 is clicked
        Debug.Log("You have clicked the button!");

        setAllUnactive();
        int xcount = Random.Range(1, 4);

        if(xcount == 1) {
            l1.SetActive(true);
        }else if (xcount == 2) {
            l2.SetActive(true);
        } else {
            l3.SetActive(true);
        }
    }

    void setAllUnactive() {

        l1.SetActive(false);
        l2.SetActive(false);
        l3.SetActive(false);
    }
}
