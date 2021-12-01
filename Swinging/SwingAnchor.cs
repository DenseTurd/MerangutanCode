using System;
using UnityEngine;

public class SwingAnchor : MonoBehaviour, ISectionActivatable
{
    bool selected;
    Swinger swinger;
    const float maxSwingDistance = 30;
    ParticleSystem ps;
    SpriteRenderer rendy;

    const float inRangeCheckTime = 0.1f;
    float inRangeCheckTimer;

    void Start()
    {
        swinger = Overseer.Instance.player.GetComponent<Swinger>();
        swinger.swingAnchors.Add(this);
        ps = GetComponentInChildren<ParticleSystem>();
        rendy = this.GetComponentOrComplain<SpriteRenderer>();
    }

    void Update()
    {
        inRangeCheckTimer -= Time.deltaTime;
        if (inRangeCheckTimer <= 0)
        {
            if (IsCandidateForNewSwingAnchor())
                ShowImACandidate();
            else
                ShowImNotACandidate();

            inRangeCheckTimer = inRangeCheckTime;
        }
    }

    void ShowImNotACandidate()
    {
        ps.Stop();
        transform.localScale = Vector3.one * 0.5f;
        rendy.color = Color.gray;
    }

    void ShowImACandidate()
    {
        ps.Play();
        transform.localScale = Vector3.one;
        rendy.color = Color.green;
    }

    public bool IsCandidateForNewSwingAnchor()
    {
        if (CheckIfWithinMaxAnchorDistance())
        {
            return true;
        }
        return false;
    }

    float distToPlayer;
    bool CheckIfWithinMaxAnchorDistance()
    {
        //Debug.Log("Checking max swing anchor distance");
        distToPlayer = Vector2.Distance(transform.position, swinger.transform.position);
        if (distToPlayer < maxSwingDistance)
        {
            return true;
        }
        return false;
    }

    public void Select()
    {
        //Debug.Log("Setting new swing anchor");
        swinger.SetSwingAnchor(this);
        selected = true;
    }

    public void DeSelect()
    {
        swinger.currentSwingAnchor = null;
        selected = false;
    }

    public bool isInfrontOfPlayer()
    {
        if (swinger.charController.facingRight && transform.position.x >= swinger.transform.position.x)
        {
            return true;
        }

        if (!swinger.charController.facingRight && transform.position.x <= swinger.transform.position.x)
        {
            return true;
        }
        return false;
    }
}
