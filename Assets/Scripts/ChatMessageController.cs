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
        nameObject.GetComponent<TextMeshProUGUI>().text = myChat.name;
        messageObject.GetComponent<TextMeshProUGUI>().text = myChat.message;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
