using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.Linq;
using TMPro;

public class EmailLoader : MonoBehaviour
{

	public List<Email> emails;
	public Dictionary<string, string[]> varDictionary;

	[SerializeField] GameObject emailSubject;
	[SerializeField] GameObject emailSender;
	[SerializeField] GameObject emailBody;
	[SerializeField] GameObject emailResponseSender;
	[SerializeField] GameObject emailResponseSubject;
	[SerializeField] GameObject emailResponseBody;
	[SerializeField] GameObject susManager;
	[SerializeField] float reduceSusCount;

	public int emailIndex = -1;

	int responseColoringIndex = 0;

	public string responseColor;
	public int emailsSent = 0;

	bool responseComplete = true;

	public List<Email>[] morningEmails; // 5 lists of morning emails for 5 days
	public List<Email>[] eveningEmails; // 5 lists of evening emails for 5 days
	[SerializeField] int[] morningEmailIndexesDay1;
	[SerializeField] int[] morningEmailIndexesDay2;
	[SerializeField] int[] morningEmailIndexesDay3;
	[SerializeField] int[] morningEmailIndexesDay4;
	[SerializeField] int[] morningEmailIndexesDay5;
	[SerializeField] int[] eveningEmailIndexesDay1;
	[SerializeField] int[] eveningEmailIndexesDay2;
	[SerializeField] int[] eveningEmailIndexesDay3;
	[SerializeField] int[] eveningEmailIndexesDay4;
	[SerializeField] int[] eveningEmailIndexesDay5;
	[SerializeField] GameObject objectiveManager;
	ObjectiveManager objectiveManagerScr;
	bool canUpdateEmailMorning = true;
	bool canUpdateEmailEvening = true;
	int[][] morningEmailIndexes;
	int[][] eveningEmailIndexes;
	// Start is called before the first frame update
	void Start()
	{

		//set up dictionary
		var dictionaryCSV = Resources.Load<TextAsset>("variableDictionary");
		varDictionary = new Dictionary<string, string[]>();
		string[] dictionaryArray = dictionaryCSV.text.Split(new char[] { '\n' });
		for (int i = 0; i < dictionaryArray.Length; i++)
		{
			string[] row = dictionaryArray[i].Split(new char[] { ',' });
			string identifier = row[1]; // the first item in the row is the identifier in the text
			string[] replacements = new string[row.Length - 1];
			Array.ConstrainedCopy(row, 2, replacements, 0, row.Length - 2);   // every item except the first two in the row is a replacement in the text
			replacements = replacements.Where(x => !string.IsNullOrEmpty(x)).ToArray();
			varDictionary.Add(identifier, replacements);
		}


		// read and parse emails
		emails = new List<Email>();
		var emailCSV = Resources.Load<TextAsset>("Procedural Emails");
		string[] emailsArray = emailCSV.text.Split(new char[] { '\n' }); // split csv by row
		for (int i = 1; i < emailsArray.Length; i++) // for every row in csv
		{
			string[] row = emailsArray[i].Split(new char[] { ',' }); // row = a row in csv split by column (id, email, response)
			Email email = new Email(); // refers to the Email class, 
			int.TryParse(row[0], out email.index);
			email.sender = ParseText(row[1], varDictionary);
			email.subject = ParseText(row[2], varDictionary);
			email.body = ParseText(row[3], varDictionary);
			email.responseBody = ParseText(row[4], varDictionary);
			email.responseSubject = "RE: " + email.subject;


			string[] signatures;
			varDictionary.TryGetValue("SIGNATURE", out signatures);

			if (signatures != null)
			{
				email.body = email.body + "\n\n" + signatures[Random.Range(0, signatures.Length - 1)] + "\n" + email.sender;
				email.responseBody = email.responseBody + "\n\n" + signatures[Random.Range(0, signatures.Length - 1)] + "\n" + email.responseSender;
			}

			emails.Add(email);
		}

		// text boxes setup
		emailIndex = 0;
		emailResponseBody.GetComponent<TextMeshProUGUI>().color = new Color(180, 180, 180);
		emailSubject.GetComponent<TextMeshProUGUI>().text = emails[emailIndex].subject;
		emailSender.GetComponent<TextMeshProUGUI>().text = emails[emailIndex].sender;
		emailBody.GetComponent<TextMeshProUGUI>().text = emails[emailIndex].body;
		emailResponseSender.GetComponent<TextMeshProUGUI>().text = emails[emailIndex].responseSender;
		emailResponseSubject.GetComponent<TextMeshProUGUI>().text = emails[emailIndex].responseSubject;
		emailResponseBody.GetComponent<TextMeshProUGUI>().text = emails[emailIndex].responseBody;
		responseComplete = false;

		// special emails

		morningEmailIndexes = new int[5][];
		morningEmailIndexes[0] = morningEmailIndexesDay1;
		morningEmailIndexes[1] = morningEmailIndexesDay2;
		morningEmailIndexes[2] = morningEmailIndexesDay3;
		morningEmailIndexes[3] = morningEmailIndexesDay4;
		morningEmailIndexes[4] = morningEmailIndexesDay5;

		eveningEmailIndexes = new int[5][];
		eveningEmailIndexes[0] = morningEmailIndexesDay1;
		eveningEmailIndexes[1] = morningEmailIndexesDay2;
		eveningEmailIndexes[2] = morningEmailIndexesDay3;
		eveningEmailIndexes[3] = morningEmailIndexesDay4;
		eveningEmailIndexes[4] = morningEmailIndexesDay5;

		morningEmails = new List<Email>[5];
		for (int i = 0; i < morningEmails.Length; i++)
		{
			morningEmails[i] = new List<Email>();
			for (int j = 0; j < morningEmailIndexes[i].Length; j++)
			{
				morningEmails[i].Add(emails[morningEmailIndexes[i][j]]);
				//emails.RemoveAt(morningEmailIndexes[i][j]);
			}
		}
		eveningEmails = new List<Email>[5];
		for (int i = 0; i < eveningEmails.Length; i++)
		{
			eveningEmails[i] = new List<Email>();
			for (int j = 0; j < eveningEmailIndexes[i].Length; j++)
			{
				eveningEmails[i].Add(emails[eveningEmailIndexes[i][j]]);
				//emails.RemoveAt(eveningEmailIndexes[i][j]);
			}
		}


		//set up objectives and days
		objectiveManagerScr = objectiveManager.GetComponent<ObjectiveManager>();


	}

	// Update is called once per frame
	void Update()
	{
		// day
		/*
		if (objectiveManagerScr.hour == 9 && canUpdateEmailMorning)
		{
			emails.InsertRange(0, morningEmails[objectiveManagerScr.day-1]);
			canUpdateEmailMorning = false;
		}
		else if (objectiveManagerScr.hour == 16 && canUpdateEmailEvening)
		{
			emails.InsertRange(0, morningEmails[objectiveManagerScr.day-1]);
			canUpdateEmailEvening = false;
		}
		*/


		if (Input.inputString.Length > 0) // typing
		{
			foreach (char inputChar in Input.inputString)
			{
				if (inputChar == '\n')
				{
					if (responseComplete)
					{
						NextEmail();

					}
				}
				else if (responseColoringIndex < emails[emailIndex].responseBody.Length)
				{
					responseColoringIndex++;
					emailResponseBody.GetComponent<TextMeshProUGUI>().text = ColorizeResponse();
				}
				else
				{
					responseComplete = true;
					break;
				}

			}


		}
	}

	string ColorizeResponse()
	{
		string output = emails[emailIndex].responseBody;

		output = output.Insert(responseColoringIndex, "</color>");
		output = responseColor + output;

		return output;
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
	public void NextEmail()
	{
		if (responseComplete)
		{
			emails[emailIndex].isSent = true;
			emails.RemoveAt(emailIndex);

			if (TutorialManager.Instance.mailSentCount <= TutorialManager.Instance.tutorialEmailsPreBosu)
			{
				TutorialManager.Instance.mailSentCount++;
			}
			else if (TutorialManager.Instance.mailSentCount <= TutorialManager.Instance.tutorialEmailsPostBosu)
			{
				susManager.GetComponent<CatchPlayer>().ReduceSus(100);
				TutorialManager.Instance.mailSentCount++;
			}


			if (emailIndex < emails.Count - 1)
			{
				emailIndex++;
			}
			else
			{
				emailIndex = 0;
			}
			emailSubject.GetComponent<TextMeshProUGUI>().text = emails[emailIndex].subject;
			emailSender.GetComponent<TextMeshProUGUI>().text = emails[emailIndex].sender;
			emailBody.GetComponent<TextMeshProUGUI>().text = emails[emailIndex].body;
			emailResponseSender.GetComponent<TextMeshProUGUI>().text = emails[emailIndex].responseSender;
			emailResponseSubject.GetComponent<TextMeshProUGUI>().text = emails[emailIndex].responseSubject;
			emailResponseBody.GetComponent<TextMeshProUGUI>().text = emails[emailIndex].responseBody;

			responseComplete = false;
			responseColoringIndex = 0;

			susManager.GetComponent<CatchPlayer>().ReduceSus(reduceSusCount);
		}
	}

	private void OnEnable()
	{
		GameObject myEventSystem = GameObject.Find("EventSystem");
		myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
	}

	public void ResetDay()
	{
		canUpdateEmailMorning = true;
		canUpdateEmailEvening = false;
	}
}


