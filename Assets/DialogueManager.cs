using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour {
    private Queue<string> sentences;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public Image notifMail;
    public Image notifBosu;
    public Image notifBamazon;
    public Image notifNotes;

    public Animator animator;

    private bool openedOnce = false;

    private void Start() {
        openedOnce = false;
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue) {
        animator.SetBool("IsOpen", true);

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }


    public void SkipDialogue() {
        EndDialogue();
    }



    public void DisplayNextSentence() {
        if (sentences.Count == 0) {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));

        /* if(openedOnce == false) {
             if (sentences.Count == 0) {
                 EndDialogue();
                 return;
             }

             string sentence = sentences.Dequeue();
             StopAllCoroutines();
             StartCoroutine(TypeSentence(sentence));
         } else {
             EndDialogue();
         }
        */

        //dialogueText.text = sentence; 
    }

    IEnumerator TypeSentence(string sentence) {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray()) {
            dialogueText.text += letter;
            yield return null;
        }
    }

    public void EndDialogue() {
        animator.SetBool("IsOpen", false);
        openedOnce = true;
    }
}