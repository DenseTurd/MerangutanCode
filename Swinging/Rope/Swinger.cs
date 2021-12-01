using UnityEngine;
using System.Collections.Generic;

public class Swinger : MonoBehaviour
{
    public GameObject hook;

    GameObject curHook;
    public CharController charController;
    public SwingAnchor currentSwingAnchor;

    public List<SwingAnchor> swingAnchors;
    public List<SwingAnchor> candidates;
    public List<SwingAnchor> candidatesInfrontOfPlayer;

    private void Start()
    {
        charController = GetComponent<CharController>();
        swingAnchors = new List<SwingAnchor>();
    }

    public void SetSwingAnchor(SwingAnchor anchor)
    {
        if (currentSwingAnchor)
        {
            currentSwingAnchor.DeSelect();
        }

        currentSwingAnchor = anchor;
    }

    Vector2 destination;
    Ropey rope;
    public void StartSwing()
    {
        TryAssignNewSwingAnchor();
        if (currentSwingAnchor)
        {
            destination = currentSwingAnchor.transform.position;
            curHook = Instantiate(hook);
            curHook.transform.position = transform.position;
            rope = curHook.GetComponent<Ropey>();
            rope.hookPos = destination;
            rope.swinger = this;
            charController.swinging = true;
        }
        else
        {
            //Debug.Log("No swing anchor");
        }
    }

    public void StopSwing()
    {
        if (currentSwingAnchor)
        {
            charController.swinging = false;
            if (curHook)
            {
                Destroy(curHook);
                charController.canDash = true;
            }
        }
    }

    void TryAssignNewSwingAnchor()
    {
        FindCandidates();
        FindCandidatesInfrontOfPlayer();

        if (candidatesInfrontOfPlayer.Count > 0)
        {
            //Debug.Log("Candidate infront of player");
            AssignSwingAnchorFromList(candidatesInfrontOfPlayer);
        }
        else if (candidates.Count > 0)
        {
            //Debug.Log("Candidate behind player");
            AssignSwingAnchorFromList(candidates);
        }

        if (currentSwingAnchor)
        {
            UnAssignIfNotCloseEnough();
        }
    }

    void FindCandidates()
    {
        candidates = new List<SwingAnchor>();
        foreach (var anchor in swingAnchors)
        {
            if (anchor.IsCandidateForNewSwingAnchor())
            {
                candidates.Add(anchor);
            }
        }
    }

    void FindCandidatesInfrontOfPlayer()
    {
        candidatesInfrontOfPlayer = new List<SwingAnchor>();
        foreach (var anchor in candidates)
        {
            if (anchor.isInfrontOfPlayer())
            {
                candidatesInfrontOfPlayer.Add(anchor);
            }
        }
    }

    SwingAnchor closestSwingAnchor;
    private void AssignSwingAnchorFromList(List<SwingAnchor> list)
    {
        closestSwingAnchor = list[0];
        if (list.Count > 1)
        {
            //Debug.Log("More than one candidate");
            closestSwingAnchor = FindClosestCandidate(list);
        }

        if (currentSwingAnchor)
        {
            currentSwingAnchor.DeSelect();
        }
        closestSwingAnchor.Select();
    }

    SwingAnchor FindClosestCandidate(List<SwingAnchor> list)
    {
        closestSwingAnchor = list[0];
        for (int i = 1; i < list.Count; i++)
        {
            float distToPlayer = Vector2.Distance(list[i].transform.position, transform.position);
            float currentClosestSwingAnchorDistToPlayer = Vector2.Distance(closestSwingAnchor.transform.position, transform.position);
            if (distToPlayer < currentClosestSwingAnchorDistToPlayer)
            {
                closestSwingAnchor = list[i];
            }
        }
        return closestSwingAnchor;
    }

    void UnAssignIfNotCloseEnough()
    {
        if (Vector2.Distance(transform.position, currentSwingAnchor.transform.position) > 20)
        {
            currentSwingAnchor.DeSelect();
        }
    }
}
