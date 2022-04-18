using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HueShift : MonoBehaviour
{
    public bool shifting;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shifting)
        {
            GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.HSVToRGB(Mathf.PingPong(Time.time * .25f, 1), .85f, 1f) * 6f);
        }
    }
}
