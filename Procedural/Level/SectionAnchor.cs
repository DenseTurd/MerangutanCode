using UnityEngine;

public class SectionAnchor : MonoBehaviour
{
    public LevelGenerator generator;

    public void Init(LevelGenerator gen)
    {
        generator = gen;
        GetNextLevelSection();
    }

    void GetNextLevelSection()
    {
        generator.GetSection(this);
    }
}
