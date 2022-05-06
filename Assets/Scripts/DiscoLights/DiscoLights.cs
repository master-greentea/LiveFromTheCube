using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoLights : MonoBehaviour
{
    Light light;
    public GameObject rhythmGame;
    public CatchPlayer susManager;
    public GameObject heartsOn;
    //public GameObject keys; 
    public GameObject lightsoff; 
    public bool lightSwitched = false;

    void Start()
    {
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rhythmGame.active == true && susManager.enabled == true) {
            lightSwitched = true;
            
        } else
        {
            lightSwitched = false;
        }

        if (lightSwitched) {
            light.color = Color.HSVToRGB(Mathf.PingPong(Time.time * .25f, 1), .85f, 1f);
            lightsoff.SetActive(false);
            heartsOn.SetActive(true);

           // keys.SetActive(true);


        }
        else {
            light.color = Color.black;
            lightsoff.SetActive(true);
            heartsOn.SetActive(false);

          // keys.SetActive(false);
        }
    }
}
