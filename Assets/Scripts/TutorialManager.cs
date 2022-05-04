using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RhythmGameStarter;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    [SerializeField] private bool _isTutorialShown = true;

    [SerializeField] private Button _mailIcon;
    [SerializeField] private Button _bosuIcon;
    [SerializeField] private Button _bamazonIcon;
    [SerializeField] private Button _excelIcon;
    [SerializeField] private Button _notesIcon;

    [SerializeField] private GameObject _mailScreen;
    [SerializeField] private GameObject _bosuScreen;
    [SerializeField] private GameObject _notesScreen;
    [SerializeField] private GameObject _bamazonScreen;
    [SerializeField] private GameObject _excelScreen;
    [SerializeField] private SongManager _songManager;
    [SerializeField] private CatchPlayer _susManager;
    [SerializeField] private DialogueManager _dialogueManager;
    [SerializeField] private DialogueTrigger _watchOutForBoss;
    [SerializeField] private DialogueTrigger _correctSolution;
    [SerializeField] private DialogueTrigger _incorrectSolution;
    [SerializeField] private DialogueTrigger _whew;
    [SerializeField] private DialogueTrigger _readyToStartGame;

    [SerializeField] private Color _grayedOutColor;

    public int mailSentCount = 0; //this is set in EmailLoader.Update()
    public int tutorialEmailsPreBosu = 1;
    public int tutorialEmailsPostBosu = 2;
    public bool excelHasBeenTutorialized = false;
    public bool correctClientMatch = false;
    private bool _correctItemHasBeenPurchased = false;
    public bool tutorialFinished;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (_isTutorialShown)
        {
            StartCoroutine(Instructions());
        }
    }

    private IEnumerator Instructions()
    {
        //initialize
        //force send 1 mail
        Image bosuImage = _bosuIcon.GetComponent<Image>();
        Color bosuColor = bosuImage.color;
        bosuImage.color = _grayedOutColor;
        _bosuIcon.enabled = false;
        GameObject bosuNotification = _bosuIcon.gameObject.transform.Find("Notification Img").gameObject;
        bosuNotification.SetActive(false);
        Image bamazonImage = _bamazonIcon.GetComponent<Image>();
        Color bamazonColor = bamazonImage.color;
        bamazonImage.color = _grayedOutColor;
        _bamazonIcon.enabled = false;
        GameObject bamazonNotification = _bamazonIcon.gameObject.transform.Find("Notification Img").gameObject;
        bamazonNotification.SetActive(false);
        Image excelImage = _excelIcon.GetComponent<Image>();
        Color excelColor = excelImage.color;
        excelImage.color = _grayedOutColor;
        _excelIcon.enabled = false;
        GameObject excelNotification = _excelIcon.gameObject.transform.Find("Notification Img").gameObject;
        excelNotification.SetActive(false);
        Image notesImage = _notesIcon.GetComponent<Image>();
        Color notesColor = notesImage.color;
        notesImage.color = _grayedOutColor;
        _notesIcon.enabled = false;
        GameObject notesNotification = _notesIcon.gameObject.transform.Find("Notification Img").gameObject;
        notesNotification.SetActive(false);
        yield return new WaitUntil(() => mailSentCount >= tutorialEmailsPreBosu);

        //force play bosu
        _mailScreen.SetActive(false);
        _mailIcon.enabled = false;
        bosuImage.color = bosuColor;
        _bosuIcon.enabled = true;
        bosuNotification.SetActive(true);
        yield return new WaitUntil(() => _susManager.suspicionCount >= 20f);

        //force send 1 mail
        _songManager.PauseSong();
        _watchOutForBoss.TriggerDialogue();
        _bosuIcon.enabled = false;
        _mailIcon.enabled = true;
        float suspicionGain = _susManager.suspicionGain;
        _susManager.suspicionGain = 0f;
        float suspicionLoss = _susManager.suspicionLoss;
        _susManager.suspicionLoss = 0f;
        yield return new WaitUntil(() => _dialogueManager.IsDialoguePlaying == false);

        _bosuScreen.SetActive(false);
        yield return new WaitUntil(() => mailSentCount >= tutorialEmailsPostBosu);

        //force open notes
        _whew.TriggerDialogue();
        _susManager.suspicionGain = suspicionGain;
        _susManager.suspicionLoss = suspicionLoss;
        _mailIcon.enabled = false;
        _notesIcon.enabled = true;
        notesNotification.SetActive(true);
        notesImage.color = notesColor;
        yield return new WaitUntil(() => _dialogueManager.IsDialoguePlaying == false);

        _mailScreen.SetActive(false);
        yield return new WaitUntil(() => _notesScreen.activeSelf == true);

        yield return new WaitUntil(() => _dialogueManager.IsDialoguePlaying == false);

        //force client matching
        _notesIcon.enabled = false;
        _excelIcon.enabled = true;
        excelNotification.SetActive(true);
        excelImage.color = excelColor;
        yield return new WaitUntil(() => excelHasBeenTutorialized == true);

        if (correctClientMatch)
        {
            _correctSolution.TriggerDialogue();
        }
        else
        {
            _incorrectSolution.TriggerDialogue();
        }
        yield return new WaitUntil(() => _dialogueManager.IsDialoguePlaying == false);

        //force bamazon
        _excelIcon.enabled = false;
        _bamazonIcon.enabled = true;
        bamazonNotification.SetActive(true);
        bamazonImage.color = bamazonColor;
        _excelScreen.SetActive(false);
        yield return new WaitUntil(() => _correctItemHasBeenPurchased == true);

        _readyToStartGame.TriggerDialogue();
        yield return new WaitUntil(() => _dialogueManager.IsDialoguePlaying == false);

        _bamazonScreen.SetActive(false);
        _mailIcon.enabled = true;
        _bosuIcon.enabled = true;
        _excelIcon.enabled = true;
        _notesIcon.enabled = true;
        tutorialFinished = true;
        yield break;
    }

    public void PurchaseCorrectItem()
    {
        _correctItemHasBeenPurchased = true;
    }
}
