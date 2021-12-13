using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class newScene : MonoBehaviour
{
    void OnGUI() {
        if (Input.GetMouseButtonDown(0))
            SceneManager.LoadScene("Game Scene");

    }
}
