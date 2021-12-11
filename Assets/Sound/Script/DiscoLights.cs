using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoLights : MonoBehaviour
{
    Light light;
    bool lightSwitched = true;

    void Start()
    {
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            lightSwitched = !lightSwitched;
        }

        if (lightSwitched) {
            light.color = Color.HSVToRGB(Mathf.PingPong(Time.time * .85f, 1), 1f, 1f);
        }
        else {
            light.color = Color.black;
        }
    }
}
