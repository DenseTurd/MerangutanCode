using UnityEngine;

public class NPCDialogueTrigger : DialogueTrigger
{
    [HideInInspector]
    public NPC npc;

    void Start()
    {
        npc = this.GetComponentOrComplain<NPC>();
    }

}
