using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EmailLoader : MonoBehaviour
{

	List<Email> emails;
	System.Collections.Generic.Dictionary<string, string[]> varDictionary;

	// Start is called before the first frame update
	void Start()
	{
		emails = new List<Email>();
		var emailCSV = Resources.Load<TextAsset>("placeholderEmail");
		string[] emailsArray = emailCSV.text.Split(new char[] { '\n' }); // split csv by row
		for (int i = 0; i < emailsArray.Length; i++) // for every row in csv
		{
			string[] row = emailsArray[i].Split(new char[] { ',' }); // row = a row in csv split by column (id, email, response)
			Email email = new Email(); // refers to the Email class, 
			int.TryParse(row[0], out email.index);
			email.text = row[1];
			email.response = row[2];
			emails.Add(email);
		}


		var dictionaryCSV = Resources.Load<TextAsset>("placeholderDictionary");
		varDictionary = new Dictionary<string, string[]>();
		string[] dictionaryArray = dictionaryCSV.text.Split(new char[] { '\n' });
		for (int i = 0; i < dictionaryArray.Length; i++)
		{
			string[] row = dictionaryArray[i].Split(new char[] { ',' });
			string identifier = row[0]; // the first item in the row is the identifier in the text
			string[] replacements = new string[row.Length - 1]; 
			Array.ConstrainedCopy(row, 1, replacements, 0, row.Length - 1);   // every item except the first in the row is a replacement in the text
			varDictionary.Add(identifier, replacements);
		}

	}

	// Update is called once per frame
	void Update()
	{

	}
}
