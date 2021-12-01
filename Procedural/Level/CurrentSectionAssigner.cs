using UnityEngine;
using System.Collections.Generic;
using System;

public class CurrentSectionAssigner : MonoBehaviour
{
    public LevelGenerator levelGenerator;
    public static Section currentSection;
    const float assignSectionTime = 0.2f;
    float assignSectiontimer;
    const float sectionActivationTime = 0.2f;
    float sectionActivationTimer = 0.3f;

    int currentSectionIndex;

    public SectionActivation sectionActivation;

    private void Start()
    {
        levelGenerator = this.GetComponentOrComplain<LevelGenerator>();
        currentSection = levelGenerator.startingSection;
        sectionActivation = this.GetComponentOrComplain<SectionActivation>();
        sectionActivation.Init(levelGenerator);
    }

    void Update()
    {
        assignSectiontimer -= Time.deltaTime;
        if (assignSectiontimer < 0)
        {
            assignSectiontimer = assignSectionTime;
            AssignCurrentSection();
        }

        sectionActivationTimer -= Time.deltaTime;
        if (sectionActivationTimer <= 0)
        {
            SortSections();
        }
    }

    void SortSections()
    {
        sectionActivation.SortSections(currentSectionIndex);
        sectionActivationTimer = sectionActivationTime;
    }

    public int AssignCurrentSection()
    {
        currentSectionIndex = levelGenerator.instantiatedSections.IndexOf(currentSection) < 1 ? 1 : levelGenerator.instantiatedSections.IndexOf(currentSection);
        for (int i = currentSectionIndex - 1; i < currentSectionIndex + 2; i++)
        {
            if (i != currentSectionIndex)
            {
                if (i > -1 && i < levelGenerator.instantiatedSections.Count)
                {
                    float distToCurrentSection = Vector2.Distance(currentSection.transform.position, Overseer.Instance.player.transform.position);
                    float distToTestAgainst = Vector2.Distance(levelGenerator.instantiatedSections[i].transform.position, Overseer.Instance.player.transform.position);
                    if (distToTestAgainst < distToCurrentSection)
                    {
                        UndoTheVisualTingToThePreviousSectionForNow();

                        currentSection = levelGenerator.instantiatedSections[i];
                        
                        DoSomethingVisualToIndicateCurrentSectionForNow();
                    }
                }
            }
        }
        return currentSectionIndex;
    }

    void UndoTheVisualTingToThePreviousSectionForNow()
    {
        currentSection.GetComponent<SpriteRenderer>().color = Color.white;
    }

    void DoSomethingVisualToIndicateCurrentSectionForNow()
    {
        currentSection.GetComponent<SpriteRenderer>().color = Color.red;
    }
}
