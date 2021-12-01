using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public SpeechBubble speechBubble;

    [Space]
    [Header("Choice")]
    public DialogueChoice choice;
    public delegate void del();
    public del onDialogueEndDelegate;
    public void TriggerDialogue()
    {
        Overseer.Instance.dialogueManager.StartDialogue(this);
    }

    public void OnDialogueEnd()
    {
        if (onDialogueEndDelegate != null)
        {
            onDialogueEndDelegate();
        }
    }
}
