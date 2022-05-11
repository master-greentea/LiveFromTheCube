using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SongUnlock : MonoBehaviour
{
    [SerializeField] GameObject introEasy;
    [SerializeField] GameObject Song1;
    [SerializeField] GameObject Song2;
    [SerializeField] GameObject Song3;
    [SerializeField] GameObject introHard;
    [SerializeField] GameObject Bosu;

    private int dateOfToday;
    public GameObject ObjectiveManager;

    public UnityEvent onMenuOpened;

    void Start()
    {
        onMenuOpened.Invoke();
    }

    void OnEnable()
    {
        dateOfToday = ObjectiveManager.GetComponent<ObjectiveManager>().dateOfToday;

        if (dateOfToday == 0)
        {
            introEasy.SetActive(true);
            Song1.SetActive(false);
            Song2.SetActive(false);
            Song3.SetActive(false);
            introHard.SetActive(false);

        }
        else if (dateOfToday == 1)
        {
            introEasy.SetActive(false);
            Song1.SetActive(true);
            Song2.SetActive(false);
            Song3.SetActive(false);
            introHard.SetActive(false);

        }
        else if (dateOfToday == 2)
        {
            introEasy.SetActive(false);
            Song1.SetActive(false);
            Song2.SetActive(true);
            Song3.SetActive(false);
            introHard.SetActive(false);

        }
        else if (dateOfToday == 3)
        {
            introEasy.SetActive(false);
            Song1.SetActive(false);
            Song2.SetActive(false);
            Song3.SetActive(true);
            introHard.SetActive(false);

        }
        else if (dateOfToday == 4)
        {
            introEasy.SetActive(false);
            Song1.SetActive(true);
            Song2.SetActive(true);
            Song3.SetActive(true);
            introHard.SetActive(true);
        }
    }


    void Update()
    {
        
    }
}
