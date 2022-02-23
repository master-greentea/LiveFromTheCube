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

	int emailIndex = -1;

	int responseColoringIndex = 0;
	public string responseColor;
	bool responseComplete = true;

	// Start is called before the first frame update
	void Start()
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


		// read and parse emails
		emails = new List<Email>();
		var emailCSV = Resources.Load<TextAsset>("placeholderEmail");
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


		// debug
		foreach (Email email in emails)
		{
			Debug.Log(email.index + email.sender + email.subject + email.body + email.responseSubject + email.responseBody);
		}

		// text boxes setup
		emailIndex = 0;
		emailResponseBody.GetComponent<TextMeshProUGUI>().color = new Color(180, 180, 180);


		NextEmail();
		responseComplete = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.inputString.Length > 0) // typing
		{
			foreach(char inputChar in Input.inputString)
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
					responseColoringIndex ++;
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

			susManager.GetComponent<CatchPlayer>().ReduceSus(10);
		}
	}
}


