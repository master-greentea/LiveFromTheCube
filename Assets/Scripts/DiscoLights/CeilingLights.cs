using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingLights : MonoBehaviour
{
    Renderer[] childrenLights;
    Transform[] childrenLightsTransform;
    public DiscoLights discoLights;

    float timer;
    float gapTime = .1f;
    void Start()
    {
        childrenLights = GetComponentsInChildren<Renderer>();
        childrenLightsTransform = GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        // light show
        if (discoLights.lightSwitched) {
            int randIndex = Random.Range(0, childrenLights.Length);
            if (timer > gapTime) {
                float randR = Random.Range(0.5f, 1f);
                float randG = Random.Range(0.5f, 1f);
                float randB = Random.Range(0.5f, 1f);
                // childrenLights[randIndex].material.SetColor("_EmissionColor", new Color(randR, randG, randB, 1) * 12f);
                childrenLights[randIndex].material.SetColor("_EmissionColor", Random.ColorHSV(0, 1, 1, 1, .5f, 1, 1, 1) * 3f);
                if (childrenLightsTransform[randIndex] != transform) {
                    childrenLightsTransform[randIndex].rotation = Quaternion.Euler(-180, 0, 0);;
                }
                timer = 0;
            }
            timer += Time.deltaTime;
        }

        else {
            for (int i = 0; i < childrenLights.Length; i++) {
                if (childrenLightsTransform[i] != transform) {
                    childrenLightsTransform[i].rotation = Quaternion.Euler(0, 0, 0);
                }
                // childrenLights[i].material.SetColor("_EmissionColor", Color.white);s
            }
        }
    }
}
