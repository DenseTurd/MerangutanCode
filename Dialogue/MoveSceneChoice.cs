using UnityEngine.SceneManagement;

public class MoveSceneChoice : DialogueChoice
{
    public string sceneName;
    public DialogueTrigger yesDialoguetrigger;
    public DialogueTrigger noDialogueTrigger;

    public override void Yes()
    {
        yesDialoguetrigger.onDialogueEndDelegate = delegate () {SceneManager.LoadScene(sceneName);};
        yesDialoguetrigger.TriggerDialogue();
    }

    public override void No()
    {
        noDialogueTrigger.TriggerDialogue();
    }

}
