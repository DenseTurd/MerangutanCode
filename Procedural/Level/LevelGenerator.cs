using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Section startingSection;
    [Space]
    public List<Section> sequentialSections;
    [Space]
    public int checkPoints = 3;
    [Range(1, 9)] public int checkPointFrequency = 5;
    int realCheckPointFrequency;
    public List<Section> checkPointSections;
    [Space]
    public List<Section> necessarySections;
    [Space]
    public List<Section> randomSections;
    [Space]
    public Section levelEndSection;

    [HideInInspector] public List<Section> instantiatedSections;
    [HideInInspector] public List<Section> upSections;
    [HideInInspector] public List<Section> rightSections;
    [HideInInspector] public List<Section> downSections;
    [HideInInspector] public List<List<Section>> eligibleSections;

    int checkPointProbability = 0;
    int necessaryProbability = 0;
    int sectionsTillEnd = 6;
    bool levelDone;
    public void Init()
    {
        instantiatedSections = new List<Section>();
        instantiatedSections.Add(startingSection);
        CreateDirectionalSectionsLists();

        realCheckPointFrequency = (10 - checkPointFrequency) * 7;
        sectionsTillEnd = realCheckPointFrequency / 5;

        //Debug.Log($"realCheckPointFrequency: {realCheckPointFrequency}");
        //Debug.Log($"sectionsTillEnd: {sectionsTillEnd}");
        startingSection.Init(this);
    }

    void CreateDirectionalSectionsLists()
    {
        upSections = new List<Section>();
        rightSections = new List<Section>();
        downSections = new List<Section>();
        foreach (Section section in randomSections)
        {
            section.SetDir();
            if (section.direction == Dir.Up)
            {
                upSections.Add(section);
            }
            if (section.direction == Dir.Right)
            {
                rightSections.Add(section);
            }
            if (section.direction == Dir.Down)
            {
                downSections.Add(section);
            }
        }
        //Debug.Log("Directional lists created");
    }

    public void GetSection(SectionAnchor anchor)
    {
        if (levelDone) return;
        Debug.Log($"Finding sction to attach to {anchor.transform.parent.name}");

        DetermineEligibleSectionsLists(anchor);

        Section chosenSection = ChooseSection(anchor);

        var section = Instantiate(chosenSection, transform);
        section.transform.position = anchor.transform.position;

        instantiatedSections.Add(section);

        section.Init(this);

        Debug.Log($"Attached: {section.gameObject.name}");
    }

    public virtual Section ChooseSection(SectionAnchor anchor)
    {
        Section chosenSection;
        if (sequentialSections.Count > 0)
        {
            chosenSection = sequentialSections[0];
            sequentialSections.RemoveAt(0);
            return chosenSection;
        }

        if (checkPoints == 0 && necessarySections.Count == 0)
        {
            sectionsTillEnd--;
            if (sectionsTillEnd <= 0)
            {
                levelDone = true;
                Overseer.Instance.audioManager.InGameMusic();
                return levelEndSection;
            }
        }

        if (ItsACheckPoint())
        {
            return GetCheckpoint(anchor);
        }

        if (ItsANecessarySection())
        {
            //Debug.Log("Necessary section");
            int k = Random.Range(0, necessarySections.Count);
            chosenSection = necessarySections[k];
            necessarySections.RemoveAt(k);
            return chosenSection;
        }

        return GetRandomSection(anchor);
    }

    Section GetCheckpoint(SectionAnchor anchor)
    {
        bool foundEligibleCheckpointSection = false;
        Section chkSection = checkPointSections.GetRandom();
        if (SectionWillCollideWithTerrain(anchor, chkSection))
        {
            for (int i = 0; i < checkPointSections.Count; i++)
            {
                if (!SectionWillCollideWithTerrain(anchor, checkPointSections[i]))
                {
                    foundEligibleCheckpointSection = true;
                    chkSection = checkPointSections[i];
                }
            }
            if (!foundEligibleCheckpointSection)
            {
                Debug.Log("Couldn't attack a checkpoint section, ending level");
                return levelEndSection;
            }
        }
        return chkSection;
    }

    Section GetRandomSection(SectionAnchor anchor)
    {
        if (eligibleSections.Count < 1)
        {
            Debug.Log("No eligible sections, have you populated the \"Random sections\" list in the level generator?.. Ending level");
            return levelEndSection;
        }

        int i = Random.Range(0, eligibleSections.Count);
        int j = Random.Range(0, eligibleSections[i].Count);
        if (SectionWillCollideWithTerrain(anchor, eligibleSections[i][j]))
        {
            bool foundAnEligibleSection = false;
            Debug.Log($"Trying a new section");
            for (int k = 0; k < eligibleSections.Count; k++)
            {
                int randomStartingPointToCheckListFrom = Random.Range(0, eligibleSections[k].Count);
                for (int l = 0; l < eligibleSections[k].Count; l++)
                {
                    int m = l + randomStartingPointToCheckListFrom;
                    if (m >= eligibleSections[k].Count)
                    {
                        m -= eligibleSections[k].Count;
                    }

                    if (!SectionWillCollideWithTerrain(anchor, eligibleSections[k][m]))
                    {
                        foundAnEligibleSection = true;
                        i = k;
                        j = m;
                        break;
                    }
                }
            }
            if (!foundAnEligibleSection)
            {
                Debug.Log("Couldnt find an eligible section, ending level");
                return levelEndSection;
            }
        }
        return eligibleSections[i][j];
    }

    bool ItsACheckPoint()
    {
        if (checkPoints == 0) return false;

        if (Random.Range(0, realCheckPointFrequency) <= checkPointProbability)
        {
            checkPointProbability = 0;
            checkPoints--;
            return true;
        }
        checkPointProbability++;
        return false;
    }

    bool ItsANecessarySection()
    {
        if (necessarySections.Count == 0) return false;

        if (Random.Range(0, 41) <= necessaryProbability)
        {
            necessaryProbability = 0;
            return true;
        }
        necessaryProbability++;
        return false;
    }    

    public void DetermineEligibleSectionsLists(SectionAnchor anchor)
    {
        eligibleSections = new List<List<Section>>();
        float distToTerrain = DistToTerrain(anchor.transform.position);
        Debug.Log($"Distance to terrain: {distToTerrain}");
        if (distToTerrain > 10 )
        {
            if (float.IsNaN(distToTerrain))
            {
                Debug.Log("isNan");
                return;
            }
            Debug.Log("Down sections eligible");
            AddListIfNotEmpty(downSections);
        }
        AddListIfNotEmpty(rightSections);
        if (anchor.transform.position.y < 100)
        {
            Debug.Log("Up sections eligible");
            AddListIfNotEmpty(upSections);
        }
        if (distToTerrain < 15)
        {
            Debug.Log("Close too terrain");
            Debug.Log("Up sections eligible");
            AddListIfNotEmpty(upSections);
        }
    }

    void AddListIfNotEmpty(List<Section> listy)
    {
        if (listy.Count > 0)
        {
            eligibleSections.Add(listy);
        }
    }

    bool SectionWillCollideWithTerrain(SectionAnchor anchor, Section section)
    {
        Debug.Log($"Checking if {section.name} would collide with terrain");


        Vector3 lowPoint = TryFindLowPoint(section);
        if (lowPoint != Vector3.zero)
            if (PointWillCollideWithTerrain(anchor.transform.position + lowPoint))
                return true;

        Vector3 testSectionAnchorPos = new Vector3(anchor.transform.position.x + SectionLength(section), anchor.transform.position.y + SectionDeltaHeight(section), anchor.transform.position.z);
        return PointWillCollideWithTerrain(testSectionAnchorPos);
    }

    Vector3 TryFindLowPoint(Section section)
    {
        for (int i = 0; i < section.transform.childCount; i++)
        {
            Transform child = section.transform.GetChild(i);
            if (child.CompareTag(Strs.lowPoint)) 
            {
                Debug.Log($"{section.name} has a low point"); 
                return child.position;
            }
        }
        return Vector3.zero;
    }

    private bool PointWillCollideWithTerrain(Vector3 pos)
    {
        if (DistToTerrain(pos) < 4)
        {
            Debug.Log($"{pos} would collide with terrain, getting a new section");
            return true;
        }
        else if (float.IsNaN(DistToTerrain(pos)))
        {
            Debug.Log("Couldn't find terrain, assuming collision");
            return true;
        }
        Debug.Log("Section collision with terrain unlikely");
        return false;
    }

    float SectionDeltaHeight(Section section)
    {
        if (section.GetComponentInChildren<SectionAnchor>())
        {
            float anchorY = section.GetComponentInChildren<SectionAnchor>().transform.position.y;
            Debug.Log($"Section delta height: {anchorY - section.transform.position.y}");
            return anchorY - section.transform.position.y;
        }
        else
        {
            Debug.Log("Could not find section anchor. Needs to be a child of the root gameobject in a level section");
            return -1;
        }
    }

    float SectionLength(Section section)
    {
        if (section.GetComponentInChildren<SectionAnchor>())
        {
            float anchorX = section.GetComponentInChildren<SectionAnchor>().transform.position.x;
            Debug.Log($"Section length: {anchorX - section.transform.position.x}");
            return anchorX - section.transform.position.x;
        }
        else
        {
            Debug.Log("Could not find section anchor. Needs to be a child of the root gameobject in a level section");
            return -1;
        }
    }

    float DistToTerrain(Vector3 pos)
    {
        RaycastHit hitInfo;
        int layerMask = 1 << 7; // not sure why it needs a bitshift??
        if (Physics.Raycast(pos, Vector3.down, out hitInfo, 1000, layerMask))
        {
            Debug.Log($"Dist to terrain: {pos.y - hitInfo.point.y}");
            return pos.y - hitInfo.point.y;
        }
        return float.NaN;
    }

    float SectionDepth(Section section)
    {
        float startY = section.transform.position.y;

        lowestPoint = GetLowestPointRecursive(section.transform, startY);
        
        return startY - lowestPoint;
    }

    float lowestPoint;
    float GetLowestPointRecursive(Transform trans, float currentLowestPoint)
    {
        if (trans.position.y < currentLowestPoint)
        {
            currentLowestPoint = trans.position.y;
        }
        if (trans.childCount > 0)
        {
            foreach (Transform t in trans)
            {
                currentLowestPoint = GetLowestPointRecursive(t, currentLowestPoint);
            }
        }

        return currentLowestPoint;
    }
}
