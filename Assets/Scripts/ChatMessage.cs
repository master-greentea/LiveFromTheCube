using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatMessage
{
	public int index;
	public string message;
	public int frequency;
	public int positivity;
	public string game;
	public string dependency;
	
	/*
	public GameObject profilePic;
	public GameObject profileName;
	public GameObject chatMessage;

	public ChatScriptableObj chatData;



	private Image profPicImage;
	private TMPro.TextMeshProUGUI profPicName;
	private TMPro.TextMeshProUGUI chatBox;
	private float counter = 5;


	// Start is called before the first frame update
	void Start()
	{
		profPicImage = profilePic.GetComponent<Image>();
		profPicName = profileName.GetComponent<TMPro.TextMeshProUGUI>();
		chatBox = chatMessage.GetComponent<TMPro.TextMeshProUGUI>();

		Initalize();
	}

	void Update()
	{

	}

	public void Initalize()
	{
		profPicImage.sprite = chatData.profilePicture;
		profPicName.text = chatData.profileName;
		chatBox.text = chatData.chatContent;
	}
	*/
}
