using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
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
    [SerializeField] private TextMeshProUGUI[] _solutionChoiceButtonTexts;
    [SerializeField] private ClientMatchingLoader _loader;
    [SerializeField] private TextMeshProUGUI _clientText;
    [SerializeField] private TextMeshProUGUI _lookingToText;
    [SerializeField] private ClientSuspicion _clientSuspicion;
    [SerializeField] private float _reduceSusCount;


    [SerializeField] GameObject currencyManager;

    CurrencySystem currensys; 

    private int _correctChoice;
    public int clientMatched = 0;
    public int moneyDecreased = 100; 

    void Start()
    {
        clientMatched = 0;
        GenerateChoice();

        currensys = currencyManager.GetComponent<CurrencySystem>();

    }

    void Update()
    {
        
    }

    /// <summary>
    /// 
    /// </summary>
    private void GenerateChoice()
    {
        _clientText.text = GetRandomElement(_loader.Clients);
        int correctIndustryId = GetRandomElement(_loader.Industries);
        Solution[] correctPotentialLookingTos = Array.FindAll(_loader.LookingTos, x => x.IndustryId == correctIndustryId);
        _lookingToText.text = GetRandomElement(correctPotentialLookingTos).Name;
        _correctChoice = Random.Range(0, _solutionChoiceButtonTexts.Length);
        Solution[] correctPotentialSolutions = Array.FindAll(_loader.PotentialSolutions, x => x.IndustryId == correctIndustryId);
        Solution correctSolution = GetRandomElement(correctPotentialSolutions);
        _solutionChoiceButtonTexts[_correctChoice].text = correctSolution.Name;
        List<Solution> incorrectPotentialSolutions = Array.FindAll(_loader.PotentialSolutions, x => x.IndustryId != correctIndustryId).ToList();
        for (int i = 0; i < _solutionChoiceButtonTexts.Length; i++)
        {
            if(i == _correctChoice)
            {
                continue;
            }
            Solution incorrectSolution = GetRandomElement(incorrectPotentialSolutions);
            incorrectPotentialSolutions.Remove(incorrectSolution);
            _solutionChoiceButtonTexts[i].text = incorrectSolution.Name;
        }
    }

    public void OnSolutionChosen(int choice)
    {
        if (choice == _correctChoice)
        {
            MatchClient();
            clientMatched++;
        }
        else
        {
            Debug.Log("Incorrect.");
            currensys.money -= moneyDecreased;

            if (!TutorialManager.Instance.excelHasBeenTutorialized)
            {
                TutorialManager.Instance.correctClientMatch = false;
                TutorialManager.Instance.excelHasBeenTutorialized = true;
            }
        }
        GenerateChoice();
    }

    private void MatchClient()
    {
        //player got match correct
        Debug.Log("Correct!");
        _clientSuspicion.ReduceSus(_reduceSusCount);

        if (!TutorialManager.Instance.excelHasBeenTutorialized)
        {
            TutorialManager.Instance.correctClientMatch = true;
            TutorialManager.Instance.excelHasBeenTutorialized = true;
        }
    }

    private T GetRandomElement<T>(T[] deck)
    {
        int randomIndex = Random.Range(0, deck.Length);
        return deck[randomIndex];
    }

    private T GetRandomElement<T>(List<T> deck)
    {
        int randomIndex = Random.Range(0, deck.Count);
        return deck[randomIndex];
    }
}
