using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HueShiftForSprite : MonoBehaviour
{
    public float Speed = 1;

    Renderer m_Renderer;
    public GameObject bar;

    void Start()
    {
      
        m_Renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        bar.GetComponent<Image>().color = new Color(Mathf.PingPong(Time.time * Speed, 1), 0.5f, 0.5f);
        //bar.GetComponent<Image>().color = new Color32(153, 163,153, 100);
    }
}
