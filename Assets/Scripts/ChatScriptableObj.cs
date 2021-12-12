using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Chat Data", menuName = "Chat System/Chat Data", order = 1)]
public class ChatScriptableObj : ScriptableObject
{
    public Sprite profilePicture;

    public string profileName;
    public string chatContent;
}
