using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoLights : MonoBehaviour
{
    Light light;
    public GameObject rhythmGame;
    public CatchPlayer susManager;
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
        }
        else {
            light.color = Color.black;
        }
    }
}
