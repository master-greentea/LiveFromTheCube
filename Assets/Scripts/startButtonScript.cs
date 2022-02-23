using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class startButtonScript : MonoBehaviour
{

    public Button cutScene;

    void Start() {
        Button btn = cutScene.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void TaskOnClick() {
        Debug.Log("Next");
        SceneManager.LoadScene("Game Scene");
    }
 }
