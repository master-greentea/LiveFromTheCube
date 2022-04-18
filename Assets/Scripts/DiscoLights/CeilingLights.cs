using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingLights : MonoBehaviour
{
    // Renderer[] childrenLights;
    // Transform[] childrenLightsTransform;
    public DiscoLights discoLights;

    // float timer;
    // float gapTime = .001f;
    void Start()
    {
        // childrenLights = GetComponentsInChildren<Renderer>();
        // childrenLightsTransform = GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        // light show
        if (discoLights.lightSwitched) {
            // for (int i = 0; i < childrenLights.Length; i++) {
            //     childrenLights[i].GetComponent<HueShift>().shifting = true;
            //     if (childrenLightsTransform[i] != transform) {
            //         childrenLightsTransform[i].rotation = Quaternion.Euler(-180, 0, 0);
            //     }
            // }
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        else {
            // for (int i = 0; i < childrenLights.Length; i++) {
            //     if (childrenLightsTransform[i] != transform) {
            //         childrenLightsTransform[i].rotation = Quaternion.Euler(0, 0, 0);
            //     }
            // }
            transform.rotation = Quaternion.Euler(-180, 0, 0);
        }
    }
}