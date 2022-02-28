using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatMessageController : MonoBehaviour
{

    public ChatMessage myChat;
    GameObject nameObject;
    GameObject messageObject;
    // Start is called before the first frame update
    void Start()
    {
        nameObject = transform.GetChild(0).gameObject;
        messageObject = transform.GetChild(1).gameObject;
        nameObject.GetComponent<TextMeshPro>().text = "test name";//myChat.name;
        messageObject.GetComponent<TextMeshPro>().text = "test message";//myChat.message;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
