using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsManager : MonoBehaviour
{
    public AudioClip[] effects;
    public AudioSource audS;

    void Start()
    {
        audS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            audS.PlayOneShot(effects[1]);
        }
        if (Input.GetKeyDown(KeyCode.Tab)) {
            audS.PlayOneShot(effects[2]);
        }
    }
}
