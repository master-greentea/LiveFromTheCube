using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverButton : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseOver()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    void OnMouseExit()
    {

        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
}
