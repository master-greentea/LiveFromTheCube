using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.Linq;
using TMPro;

public class ChatSystem : MonoBehaviour
{
    public List<ChatMessage> chatMessages;
    public Dictionary<string, string[]> varDictionary;

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


        if (Mathf.RoundToInt(counter) == timeToRollChat)
        {
            ManageChat();
            counter = 0;
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
        //for (int i = 0; i < activeChatList.Count; i++)
        //{
        //    //chatMessage[i].GetComponent<ChatMessage>().chatData = RollMessage(normResponse);
        //}




        GameObject newChatObject = Instantiate(chatObject);

        /*   MASAYA's Obsolete code
        newChatObject.GetComponent<ChatMessage>().chatData = RollMessage(normResponse);
        newChatObject.transform.SetParent(gameObject.transform, false);
        */



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
                    activeChatList[i].transform.position = activeChatList[i].transform.position + new Vector3(0, 1.6f, 0);
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



    string ParseText(string text, Dictionary<string, string[]> myDict)
    {
        string output = text;
        foreach (KeyValuePair<string, string[]> entry in myDict)
        {
            int numberOfReplacements = entry.Value.Length;
            int randomIndex = Random.Range(0, numberOfReplacements - 1);
            //Debug.Log(entry.Value[randomIndex]);
            if (entry.Value[randomIndex].Length >= 1)
            {
                string uppercaseReplacement = char.ToUpper(entry.Value[randomIndex][0]) + entry.Value[randomIndex].Substring(1);
                //Debug.Log(uppercaseReplacement);
                output = output.Replace(". " + entry.Key, uppercaseReplacement);
                output = output.Replace("! " + entry.Key, uppercaseReplacement);
                output = output.Replace("? " + entry.Key, uppercaseReplacement);
                output = output.Replace(") " + entry.Key, uppercaseReplacement);
                //Debug.Log(entry.Key + string.Join(',',entry.Value));
                output = output.Replace(entry.Key, entry.Value[randomIndex]);
            }

        }
        output = output.Replace("COMMA", ",");

        //Debug.Log("ParseText:"+output);
        return output;
    }

    void LoadChatMessages()
	{
        //set up dictionary
        var dictionaryCSV = Resources.Load<TextAsset>("variableDictionary");
        varDictionary = new Dictionary<string, string[]>();
        string[] dictionaryArray = dictionaryCSV.text.Split(new char[] { '\n' });
        //Debug.Log("dict has " + dictionaryArray.Length + "rows");
        for (int i = 0; i < dictionaryArray.Length; i++)
        {
            string[] row = dictionaryArray[i].Split(new char[] { ',' });
            string identifier = row[1]; // the first item in the row is the identifier in the text
            string[] replacements = new string[row.Length - 1];
            Array.ConstrainedCopy(row, 2, replacements, 0, row.Length - 2);   // every item except the first two in the row is a replacement in the text
            replacements = replacements.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            varDictionary.Add(identifier, replacements);
        }


        // read and parse chat responses
        chatMessages = new List<ChatMessage>();
        var emailCSV = Resources.Load<TextAsset>("Chat Responses");
        string[] emailsArray = emailCSV.text.Split(new char[] { '\n' }); // split csv by row
        for (int i = 1; i < emailsArray.Length; i++) // for every row in csv
        {
            string[] row = emailsArray[i].Split(new char[] { ',' }); 
            ChatMessage chatMessage = new ChatMessage(); // refers to the ChatMessage class,
            // actual parsing
            int.TryParse(row[0], out chatMessage.index);

            chatMessage.message = ParseText(row[1], varDictionary);

            if (row[2].Equals("high"))
            {
                chatMessage.frequency = 1; // high = 1, low = 0
            }
            else if(row[2].Equals("low"))
            {
                chatMessage.frequency = 0; // high = 1, low = 0
            }


            if (row[3].Equals("pos"))
            {
                chatMessage.positivity = 1; // positive = 1, negative = 0
            }
            else if (row[3].Equals("neg"))

            {
                chatMessage.positivity = 0; // positive = 1, negative = 0
            }


            chatMessage.game = row[4];
            chatMessage.dependency = row[5];

            chatMessages.Add(chatMessage);
        }

    }
}
