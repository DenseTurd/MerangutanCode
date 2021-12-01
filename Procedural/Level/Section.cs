using System;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour, INotifyable
{
    bool autoFilled;

    public Dir direction;
    public SectionAnchor sectionAnchor;

    public bool enemies;
    public List<Transform> enemySpawnLocations;

    public bool spawnables;
    public List<SpawnablesAndLocation> spawnsAndLocations;

    public bool surprise;
    public SpawnablesAndLocation surprisesAndLocation;
    public Triggerer triggerer;

    public bool coins;
    public List<GameObject> coinParents;

    public bool checkPointSection;
    public CheckPoint checkPoint;
    public Transform checkPointLocation;

    public bool levelEndSection;
    public LevelEnd levelEnd;
    public Transform levelEndSpawnLocation;

    List<GameObject> SectionActivatables;
    ISectionActivatable tempInterface;
    bool active;

    public void Init(LevelGenerator generator)
    {
        if (enemies)
            Enemies();

        if (spawnables)
            Spawnables();

        if (surprise)
            Surprise();

        if (coins)
            Coins();

        if (checkPointSection)
            CheckPoint();

        if (levelEndSection)
            LevelEnd();

        SectionActivatables = new List<GameObject>();
        FindSectionActivatables(transform);
        DeActivateISectionActivatables();

        GetComponentInChildren<SectionAnchor>().Init(generator);
    }

    void FindSectionActivatables(Transform tran)
    {
        if (tran.childCount > 0)
        {
            for (int i = 0; i < tran.childCount; i++)
            {
                FindSectionActivatables(tran.GetChild(i));
            }
        }

        tempInterface = InterfaceUtility.GetInterface<ISectionActivatable>(tran.gameObject);
        if (tempInterface != null)
        {
            SectionActivatables.Add(tran.gameObject);
        }
    }

    void DeActivateISectionActivatables()
    {
        if (SectionActivatables == null) return;
        for (int i = 0; i < SectionActivatables.Count; i++)
        {
            if (SectionActivatables[i] != null)
                SectionActivatables[i].SetActive(false);
        }
        active = false;
    }

    void ActivateISectionActivatables()
    {
        if (SectionActivatables == null) return;
        for (int i = 0; i < SectionActivatables.Count; i++)
        {
            if (SectionActivatables[i] != null)
                SectionActivatables[i].SetActive(true);
        }
        active = true;
    }

    void Enemies()
    {
        foreach (var location in enemySpawnLocations)
        {
            var enemy = Instantiate(Overseer.Instance.enemyManager.GetEnemy());
            enemy.transform.position = location.position;
        }
    }

    void Spawnables()
    {
        foreach (var spawnAndLoc in spawnsAndLocations)
        {
            if (spawnAndLoc.alsoEnemies)
            {
                foreach (Spawnable enemy in Overseer.Instance.enemyManager.enemies)
                {
                    spawnAndLoc.spawnables.Add(enemy);
                }
            }
            var spawned = Instantiate(spawnAndLoc.spawnables[UnityEngine.Random.Range(0, spawnAndLoc.spawnables.Count)]);
            spawned.transform.position = spawnAndLoc.location.position;
        }
    }

    void Surprise()
    {
        if (surprisesAndLocation.alsoEnemies)
        {
            foreach (Spawnable enemy in Overseer.Instance.enemyManager.enemies)
            {
                surprisesAndLocation.spawnables.Add(enemy);
            }
        }
        triggerer.notifyable = this;
    }

    void Coins()
    {
        for (int i = 0; i < coinParents.Count; i++)
        {
            if (UnityEngine.Random.Range(0f, 1f) > 0.333f)
                Destroy(coinParents[i].gameObject);
        }
    }

    void CheckPoint()
    {
        //Debug.Log("CheckPoint level section Init, spawn CheckPoint");
        var spawned = Instantiate((Spawnable)checkPoint);
        spawned.transform.position = checkPointLocation.position;
    }

    void LevelEnd()
    {
        var end = Instantiate((Spawnable)levelEnd);
        end.transform.position = levelEndSpawnLocation.position;
    }

    public void Notify()
    {
        Spawnable surprise = Instantiate(surprisesAndLocation.spawnables[UnityEngine.Random.Range(0, surprisesAndLocation.spawnables.Count)]);
        surprise.transform.position = surprisesAndLocation.location.position;
    }

    public void Activate()
    {
        if (!active) ActivateISectionActivatables();
    }
    public void DeActivate()
    {
        if (active) DeActivateISectionActivatables();
    }

    public void SetDir()
    {
        sectionAnchor = GetComponentInChildren<SectionAnchor>();
        if (!sectionAnchor)
        {
            Debug.Log($"Section: {this.name} doesn't contain a section anchor");
            return;
        }

        if (sectionAnchor.transform.position.y < transform.position.y)
        {
            direction = Dir.Down;
            return;
        }

        if (sectionAnchor.transform.position.y > transform.position.y + 2)
        {
            direction = Dir.Up;
            return;
        }

        direction = Dir.Right;
    }
}

public enum Dir
{
    Right,
    Up,
    Down
}

[System.Serializable]
public class SpawnablesAndLocation
{
    public Transform location;
    public bool alsoEnemies;
    public List<Spawnable> spawnables;
    public SpawnablesAndLocation()
    {
        spawnables = new List<Spawnable>();
    }
}

