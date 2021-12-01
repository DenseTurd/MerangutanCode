using UnityEngine; 

public class Thwomp : MovingPlatform
{
    public Transform home;
    public Transform target;
    [HideInInspector] public float distance;

    bool active;
    AttackPhase attackPhase;

    [HideInInspector] public Vector3 windupTime = new Vector3(1f, 0.1f, 5f);
    float windupTimer;

    [HideInInspector] public Vector3 recoverTime = new Vector3(0.7f, 0.1f, 5f);
    float recoverTimer;

    [HideInInspector] public Vector3 cooldown = new Vector3(1.2f, 0.1f, 5f);
    float cooldownTimer;

    Vector3 prevPos;

    void Start()
    {
        distance = Vector2.Distance(home.position, target.position);
        attackPhase = AttackPhase.Idle;
        windupTimer = windupTime.x;
    }

    public override void FixedUpdate()
    {
        if (active)
        {
            if (attackPhase == AttackPhase.Idle) 
                attackPhase = AttackPhase.Windup;

            float rando = Random.Range(0.8f, 1.2f);
            transform.localScale = new Vector3(rando, rando, rando);
        }

        switch (attackPhase)
        {
            case AttackPhase.Idle:
                windupTimer = windupTime.x;
                break;

            case AttackPhase.Windup:
                Windup();
                break;

            case AttackPhase.Attack:
                Attack();
                break;

            case AttackPhase.Recover:
                Recover();
                break;

            case AttackPhase.Retreat:
                Retreat();
                break;

            case AttackPhase.Cooldown:
                Cooldown();
                break;

            default: 
                Debug.Log("Thwomp attackPhase should not be default");
                break;
        }

        prevPos = transform.position;

        Move(angle);

        deltaPos = transform.position - prevPos;

        deltaVelocity = deltaPos - prevVeloctiy;
        prevVeloctiy = deltaPos;
    }

    void Windup()
    {
        windupTimer -= Time.deltaTime;
        if (windupTimer <= 0) 
            attackPhase = AttackPhase.Attack;
    }

    void Attack()
    {
        angle += Time.deltaTime * frequency.x * 4 * 360;
        if (angle > 330)
        {
            angle = 360;
            recoverTimer = recoverTime.x;
            attackPhase = AttackPhase.Recover;
        }
    }

    void Recover()
    {
        recoverTimer -= Time.deltaTime;
        if (recoverTimer <= 0)
            attackPhase = AttackPhase.Retreat;
    }

    void Retreat()
    {
        angle -= Time.deltaTime * frequency.x * 4 * 72;
        if (angle < 15)
        {
            angle = 0;
            cooldownTimer = cooldown.x;
            attackPhase = AttackPhase.Cooldown;
        }
    }

    void Cooldown()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0)
        {
            attackPhase = AttackPhase.Idle;
        }
    }

    public override void Move(float angle)
    {
        float T = angle / 360;

        transform.position = Vector3.Lerp(home.position, target.position, T);
    }

    ICrushable crushable;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Strs.crushable))
        {
            crushable = InterfaceUtility.GetInterface<ICrushable>(collision.gameObject);
            if (crushable != null)
            {
                crushable.GetCrushed();
                CamShake.Shake(0.3f, 3f);
                crushable = null;
                return;
            }
        }
    }

    public void Activate() => active = true;

    public void DeActivate() => active = false;

    public void OnValidate()
    {
        Start();
        Move(angle);
    }
}

public enum AttackPhase
{
    Idle,
    Windup,
    Attack,
    Recover,
    Retreat,
    Cooldown
}
