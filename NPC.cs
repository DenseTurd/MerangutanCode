using UnityEngine;

[RequireComponent(typeof(ProximityActivator))]
public class NPC : Spawnable, IProximityActivatable
{
    public string npcName;
    public DialogueTrigger dialogueTrigger;
    bool active;
    const float cantInterruptTime = 1;
    float cantInterruptTimer;
    ProximityActivator proximityActivator;

    public void Activate()
    {
        if (!active)
        {
            //if (cantInterruptTimer <=0)
            //{
                foreach (DialogueTrigger trigger in GetComponents<DialogueTrigger>())
                {
                    trigger.dialogue.name = npcName;
                }
                dialogueTrigger.TriggerDialogue();
                active = true;
                //cantInterruptTimer = cantInterruptTime;
            //}
        }
    }

    public void DeActivate()
    {
        if (active)
        {
            Overseer.Instance.dialogueManager.EndDialogue();
            active = false;
            //cantInterruptTimer = cantInterruptTime;
        }
    }

    void Update()
    {
        if (cantInterruptTimer > 0)
        {
            cantInterruptTimer -= Time.deltaTime;
        }
    }
}
