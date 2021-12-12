using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatSystem : MonoBehaviour
{

    public GameObject chatObject;
    public GameObject baseChatObject;
    public CatchPlayer susManager;
    public int maxChatValue = 4;

    public List<GameObject> chatMessage = new List<GameObject>();
    public int timeToRollChat = 3;
    public bool listenToGame;

    public List<ChatScriptableObj> posResponse = new List<ChatScriptableObj>();
    public List<ChatScriptableObj> negResponse = new List<ChatScriptableObj>();
    public List<ChatScriptableObj> normResponse = new List<ChatScriptableObj>();

    private List<GameObject> activeChatList = new List<GameObject>();
    private bool CR_ROLL_running;
    private float counter = 0;

    void Start()
    {
        CR_ROLL_running = false;
    }

    // Update is called once per frame
    void Update()
    {
        counter += 1 * Time.deltaTime;

        if (listenToGame != true)
        {
            if (Mathf.RoundToInt(counter) == timeToRollChat && susManager.bossRenderer.enabled == true)
            {
                ManageChat();
                counter = 0;
            }
        } else
        {
            if (Mathf.RoundToInt(counter) == timeToRollChat && susManager.playing == true)
            {
                ManageChat();
                counter = 0;
            }
        }
    }

    private ChatScriptableObj RollMessage(List<ChatScriptableObj> responseList)
    {
        CR_ROLL_running = true;

        int rollNum = 0;
        int chatRollNum = Random.Range(0, responseList.Count);
        ChatScriptableObj selectedInput = responseList[chatRollNum];
        return selectedInput;
    }

    private void ManageChat()
    {
        //grab a message, apply it to a chat message prefab and add it to the active chat list
        //spawn the prefab at the bottom and move all other objects in the list up by the height of their background image

        //ON OBJECTS add a timer to delete itself after a couple of seconds
        for (int i = 0; i < activeChatList.Count; i++)
        {
            //chatMessage[i].GetComponent<ChatMessage>().chatData = RollMessage(normResponse);
        }

        GameObject newChatObject = Instantiate(chatObject) as GameObject;
        newChatObject.GetComponent<ChatMessage>().chatData = RollMessage(normResponse);
        newChatObject.transform.SetParent(gameObject.transform, false);

        RectTransform baseRectTransform = baseChatObject.GetComponent<RectTransform>();
        RectTransform newRectTransform = newChatObject.GetComponent<RectTransform>();

        newRectTransform.anchorMin = baseRectTransform.anchorMin;
        newRectTransform.anchorMax = baseRectTransform.anchorMax;
        newRectTransform.anchoredPosition = baseRectTransform.anchoredPosition;
        newRectTransform.sizeDelta = baseRectTransform.sizeDelta;
        newRectTransform.pivot = baseRectTransform.pivot;


        //newChatObject.GetComponent<ChatMessage>().Initalize();
        activeChatList.Insert(0, newChatObject);

        if (activeChatList.Count > 1)
        {
            for (int i = 0; i < activeChatList.Count; i++)
            {
                if (i != 0)
                {
                    activeChatList[i].transform.position = activeChatList[i].transform.position + new Vector3(0, 2, 0);
                }

                if (i == maxChatValue)
                {
                    var terminalChat = activeChatList[i];
                    activeChatList.RemoveAt(i);

                    Destroy(terminalChat);
                }
            }
        }
        // go through each chat object in the list except for the first one and move them up by their height, then spawn the chat on its location. don't do any of this if chat list count == 1
    }
}
