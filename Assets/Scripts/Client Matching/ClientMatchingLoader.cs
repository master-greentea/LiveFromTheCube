using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClientMatchingLoader : MonoBehaviour
{
    [SerializeField] private TextAsset _clientList;
    [SerializeField] private TextAsset _solutionList;
    [SerializeField] private TextAsset _lookingToList;

    public int[] Industries { get; private set; }
    public Solution[] PotentialSolutions { get; private set; }
    public Solution[] LookingTos { get; private set; }
    public string[] Clients { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Industries = ParseIndustriesCsv(_solutionList.text);
        PotentialSolutions = ParseSolutionsCsv(_solutionList.text);
        LookingTos = ParseSolutionsCsv(_lookingToList.text);
        Clients = ParseClientsCsv(_clientList.text);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private int[] ParseIndustriesCsv(string csvText)
    {
        string[] rows = csvText.Split('\n');
        if (rows.Length == 0)
        {
            throw new Exception("CSV File is empty.");
        }
        List<int> industries = new();
        //ignore first row
        for (int i = 1; i < rows.Length; i++)
        {
            string[] row = rows[i].Split(',');
            if (int.TryParse(row[0], out int id))
            {
                industries.Add(id);
            }
            else
            {
                Debug.Log("Row " + (i + 1) + " has no id.");
            }
        }
        return industries.ToArray();
    }

    private Solution[] ParseSolutionsCsv(string csvText)
    {
        string[] rows = csvText.Split(Environment.NewLine.ToCharArray());
        if (rows.Length == 0)
        {
            throw new Exception("CSV File is empty.");
        }
        List<Solution> solutions = new();
        //ignore first row
        for (int i = 1; i < rows.Length; i++)
        {
            string[] row = rows[i].Split(',');
            if(!int.TryParse(row[0], out int id))
            {
                Debug.Log("Row " + (i + 1) + " has no id.");
            }
            //ignore first two columns
            for (int j = 2; j < row.Length; j++)
            {
                if (row[j].Length < 2)
                {
                    //reached the end of csv
                    break;
                }
                Solution newSolution = new(row[j], id);
                solutions.Add(newSolution);
                //Debug.Log("Name: " + newSolution.Name + ", id: " + newSolution.IndustryId);
            }
        }
        return solutions.ToArray();
    }

    private string[] ParseClientsCsv(string csvText)
    {
        string[] rows = csvText.Split("\n");
        if (rows.Length == 0)
        {
            throw new Exception("CSV File is empty.");
        }
        List<string> clients = new();
        //ignore first row
        for (int i = 1; i < rows.Length; i++)
        {
            string[] row = rows[i].Split(',');
            if (row[1]?.Length < 2)
            {
                //reached the end of csv
                break;
            }
            clients.Add(row[1]);
        }
        return clients.ToArray();
    } 
}
