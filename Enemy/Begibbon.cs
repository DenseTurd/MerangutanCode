using UnityEngine;

public  class Begibbon : Enemy, ICrushable
{
    const float timeBetweenAttacks = 1.9f;
    float attackTmer;
    bool attacking;
    DialogueTrigger dialogueTrigger;
    const float closeDialogueTime = 0.9f;
    float closeDialogueTimer;

    public ParticleSystem poofParticles;

    public override void Start()
    {
        dialogueTrigger = this.GetComponentOrComplain<DialogueTrigger>();
        dialogueTrigger.dialogue.name = "Begibbon";
        base.Start();
    }

    void Attack()
    {
        dialogueTrigger.TriggerDialogue();
        closeDialogueTimer = closeDialogueTime;
        attacking = true;
        Overseer.Instance.audioManager.enemyAudio.Attack(this);
    }

    public void GetCrushed()
    {
        poofParticles.transform.parent = null;
        poofParticles.Play();
        Overseer.Instance.audioManager.enemyAudio.Death(this);

        transform.position = new Vector3(spawnPos.x, spawnPos.y - 10000, spawnPos.z);
        active = false;
        Overseer.Instance.audioManager.adaptiveMusicManager.EnemyDistanceCheck();

    }

    void Update()
    {
        if (active)
        {
            attackTmer -= Time.deltaTime;
            if (attackTmer <= 0)
            {
                attackTmer = timeBetweenAttacks;
                Attack();
            } 
        }

        if (attacking)
        {
            closeDialogueTimer -= Time.deltaTime;
            if (closeDialogueTimer <= 0)
            {
                Overseer.Instance.dialogueManager.EndDialogue();
                attacking = false;
            }
        }
    }

    public override void ResetMe()
    {
        transform.position = spawnPos;

        poofParticles.transform.parent = transform;
        poofParticles.transform.localPosition = Vector3.zero;
    }
}
