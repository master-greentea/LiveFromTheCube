using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;
using TMPro;

public struct Solution
{
    public Solution(string name, int industryId)
    {
        Name = name;
        IndustryId = industryId;
    }
    public string Name { get; }
    public int IndustryId { get; }
}

public class ClientMatching : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] solutionChoiceButtonTexts;
    [SerializeField] private ClientMatchingLoader loader;
    [SerializeField] private TextMeshProUGUI clientText;
    [SerializeField] private TextMeshProUGUI lookingToText;

    private int _correctChoice;

    void Start()
    {
        GenerateChoice();
    }

    void Update()
    {
        
    }

    /// <summary>
    /// 
    /// </summary>
    private void GenerateChoice()
    {
        clientText.text = GetRandomElement(loader.Clients);
        int correctIndustryId = GetRandomElement(loader.Industries);
        Solution[] correctPotentialLookingTos = Array.FindAll(loader.LookingTos, x => x.IndustryId == correctIndustryId);
        lookingToText.text = GetRandomElement(correctPotentialLookingTos).Name;
        _correctChoice = Random.Range(0, solutionChoiceButtonTexts.Length);
        Solution[] correctPotentialSolutions = Array.FindAll(loader.PotentialSolutions, x => x.IndustryId == correctIndustryId);
        Solution correctSolution = GetRandomElement(correctPotentialSolutions);
        solutionChoiceButtonTexts[_correctChoice].text = correctSolution.Name;
        Solution[] incorrectPotentialSolutions = Array.FindAll(loader.PotentialSolutions, x => x.IndustryId != correctIndustryId);
        for (int i = 0; i < solutionChoiceButtonTexts.Length; i++)
        {
            if(i == _correctChoice)
            {
                continue;
            }
            Solution incorrectSolution = GetRandomElement(incorrectPotentialSolutions);
            solutionChoiceButtonTexts[i].text = incorrectSolution.Name;
        }
    }

    public void OnSolutionChosen(int choice)
    {
        if (choice == _correctChoice)
        {
            MatchClient();
        }
        else
        {
            Debug.Log("Incorrect.");
        }
        GenerateChoice();
    }

    private void MatchClient()
    {
        Debug.Log("Correct!");
    }

    public T GetRandomElement<T>(T[] deck)
    {
        int randomIndex = Random.Range(0, deck.Length);
        return deck[randomIndex];
    }
}
