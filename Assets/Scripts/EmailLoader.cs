using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class EmailLoader : MonoBehaviour
{

	List<Email> emails;
	Dictionary<string, string[]> varDictionary;
	// Start is called before the first frame update
	void Start()
	{
		//set up dictionary
		var dictionaryCSV = Resources.Load<TextAsset>("variableDictionary");
		varDictionary = new Dictionary<string, string[]>();
		string[] dictionaryArray = dictionaryCSV.text.Split(new char[] { '\n' });
		Debug.Log("dict has " + dictionaryArray.Length + "rows");
		for (int i = 0; i < dictionaryArray.Length; i++)
		{
			string[] row = dictionaryArray[i].Split(new char[] { ',' });
			string identifier = row[0]; // the first item in the row is the identifier in the text
			string[] replacements = new string[row.Length - 1];
			Array.ConstrainedCopy(row, 1, replacements, 0, row.Length - 1);   // every item except the first in the row is a replacement in the text
			varDictionary.Add(identifier, replacements);
		}


		// read and parse emails
		emails = new List<Email>();
		var emailCSV = Resources.Load<TextAsset>("placeholderEmail");
		string[] emailsArray = emailCSV.text.Split(new char[] { '\n' }); // split csv by row
		for (int i = 0; i < emailsArray.Length; i++) // for every row in csv
		{
			string[] row = emailsArray[i].Split(new char[] { ',' }); // row = a row in csv split by column (id, email, response)
			Email email = new Email(); // refers to the Email class, 
			int.TryParse(row[0], out email.index);
			email.sender = ParseText(row[1], varDictionary);
			email.subject = ParseText(row[2], varDictionary);
			email.body = ParseText(row[3], varDictionary);
			email.responseBody = ParseText(row[4], varDictionary);
			email.responseSubject = "RE: " + email.subject;



			emails.Add(email);
		}

		foreach (KeyValuePair<string, string[]> entry in varDictionary)
		{
			Debug.Log(entry.Key + entry.Value);
		}

			foreach (Email email in emails)
		{
			Debug.Log(email.index + email.sender + email.subject + email.body + email.responseSubject + email.responseBody);
		}


	}

	// Update is called once per frame
	void Update()
	{

	}


	string ParseText(string text, Dictionary<string, string[]> myDict)
	{
		string output = text;
		foreach (KeyValuePair<string, string[]> entry in myDict)
		{
			int numberOfReplacements = entry.Value.Length;
			int randomIndex = Random.Range(0, numberOfReplacements - 1);
			if (entry.Value!= null)
			{
				//string uppercaseReplacement = char.ToUpper(entry.Value[randomIndex][0]) + entry.Value[randomIndex].Substring(1);
				//output.Replace(". " + entry.Key, uppercaseReplacement);
				//output.Replace("! " + entry.Key, uppercaseReplacement);
				//output.Replace("? " + entry.Key, uppercaseReplacement);
				output.Replace(entry.Key, entry.Value[randomIndex]);
			}
			
		}
		output.Replace("COMMA", ",");


		return output;
	}
}
