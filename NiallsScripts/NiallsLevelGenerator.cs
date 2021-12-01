using System.Collections.Generic;
using UnityEngine;

public class NiallsLevelGenerator : LevelGenerator
{
    public Section nevilleSection;
    bool hasNevilled;

    public override Section ChooseSection(SectionAnchor anchor)
    {
        if (!hasNevilled && instantiatedSections.Count > 24)
        {
            hasNevilled = true;
            return nevilleSection;
        }
        return base.ChooseSection(anchor);
    }
}
