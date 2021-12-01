using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    Queue<string> sentences;

    public DialogueTrigger currentTrigger;

    string currentSentence;
    Coroutine currentlyAnimatingSentence;

    private void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(DialogueTrigger trigger)
    {
        currentTrigger = trigger;
        currentTrigger.speechBubble.name.text = currentTrigger.dialogue.name + ":";

        sentences.Clear();
        sentences = new Queue<string>();
        foreach (string sentence in currentTrigger.dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        ShowCanvas();
        NextSentence();
    }

    void ShowCanvas()
    {
        currentTrigger.speechBubble.gameObject.SetActive(true);
    }

    public void NextSentence()
    {
        if (currentlyAnimatingSentence != null)
        {
            StopCoroutine(currentlyAnimatingSentence);
            currentlyAnimatingSentence = null;
            currentTrigger.speechBubble.convo.text = currentSentence;
            return;
        }
        
        if (currentTrigger)
        {
            if (sentences.Count == 0)
            {
                if (ChoiceCheck())
                {
                    Debug.Log("Awaiting selection (Y/N)");
                    return;
                }
                EndDialogue();
                return;
            }

            currentSentence = sentences.Dequeue();
            currentlyAnimatingSentence = StartCoroutine(AnimateSentence(currentSentence));
        }
        else
        {
            //Debug.Log("No dialogue trigger");
        }
    }

    IEnumerator AnimateSentence(string sentence)
    {
        Overseer.Instance.prompt.ClearPrompt();
        currentTrigger.speechBubble.convo.text = "";
        char[] chars = sentence.ToCharArray();

        foreach (char ch in chars)
        {
            if (currentTrigger)
            {
                currentTrigger.speechBubble.convo.text += ch;
                yield return new WaitForSeconds(0.02f);
            }
            else
            {
                EndDialogue();
            }
        }
        SetPrompt();
        currentlyAnimatingSentence = null;
    }

    void SetPrompt()
    {
        string prompt = currentTrigger.choice ? $"Yes({Overseer.Instance.inputManager.yes})/No({Overseer.Instance.inputManager.no})" : $"Next({Overseer.Instance.inputManager.next})";
        Overseer.Instance.prompt.SetPrompt(prompt);
    }

    public void EndDialogue()
    {
        if (currentTrigger)
        {
            currentTrigger.speechBubble.gameObject.SetActive(false);
            currentTrigger.OnDialogueEnd();
            currentTrigger = null;
        }
        if (currentlyAnimatingSentence != null) StopCoroutine(currentlyAnimatingSentence);
        currentlyAnimatingSentence = null;
        currentSentence = null;
        Overseer.Instance.prompt.ClearPrompt();
    }

    public void Yes()
    {
        if (ChoiceCheck()) currentTrigger.choice.Yes();
    }

    public void No()
    {
        if (ChoiceCheck()) currentTrigger.choice.No();
    }

    bool ChoiceCheck()
    {
        Debug.Log("Choice check");
        if (currentTrigger)
        {
            if (currentTrigger.choice)
            {
                if (sentences.Count == 0)
                {
                    Debug.Log("Check passed");
                    return true;
                }
                else
                {
                    Debug.Log("Not on the last sentence of current dialogue");
                    return false;
                }
            }
            else
            {
                Debug.Log("dialogue trigger has no choice");
                return false;
            }
            
        }
        Debug.Log("No dialogue trigger");
        return false;
    }
}
