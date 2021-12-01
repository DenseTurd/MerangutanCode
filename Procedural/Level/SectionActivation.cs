using System.Collections.Generic;
using UnityEngine;

public class SectionActivation : MonoBehaviour
{
    Queue<Section> sectionsToDeactivate;
    LevelGenerator levelGenerator;
    Section section;

    public void Init(LevelGenerator levelGen)
    {
        sectionsToDeactivate = new Queue<Section>();
        levelGenerator = levelGen;
    }

    public void SortSections(int currentSectionIndex)
    {
        QueueFarAwaySectionsForDeActivation(currentSectionIndex);
        ActivateCloseSections(currentSectionIndex);

        if (sectionsToDeactivate.Count > 0)
            sectionsToDeactivate.Dequeue().DeActivate();
    }

    void ActivateCloseSections(int currentSectionIndex)
    {
        if (currentSectionIndex - 1 > 0) // dont do it to the 0th section
            ActivateSection(currentSectionIndex - 1);

        if (currentSectionIndex + 1 < levelGenerator.instantiatedSections.Count)
            ActivateSection(currentSectionIndex + 1);
    }

    void QueueFarAwaySectionsForDeActivation(int currentSectionIndex)
    {
        if (currentSectionIndex - 2 > 0) 
        {
            section = levelGenerator.instantiatedSections[currentSectionIndex - 2];
            if (!sectionsToDeactivate.Contains(section))
                sectionsToDeactivate.Enqueue(section);
        }

        if (currentSectionIndex + 2 < levelGenerator.instantiatedSections.Count) 
        {
            if (!sectionsToDeactivate.Contains(section))
                sectionsToDeactivate.Enqueue(levelGenerator.instantiatedSections[currentSectionIndex + 2]);
        }
    }

    public void ActivateSection(int sectionIndex)
    {
        levelGenerator.instantiatedSections[sectionIndex].Activate();
    }
}
