using System.Collections.Generic;
using UnityEngine;

public static class InterfaceUtility 
{
    static MonoBehaviour[] listy;
    public static void Init()
    {
        listy = new MonoBehaviour[0];
    }

    public static void GetInterfaces<T>(out List<T> resultList, GameObject objectToSearch) where T: class
    {
        listy = objectToSearch.GetComponents<MonoBehaviour>();
        resultList = new List<T>();
        foreach (MonoBehaviour mb in listy)
        {
            if (mb is T)
            {
                resultList.Add((T)((object)mb));
            }
        }
    }

    public static T GetInterface<T>(GameObject objectToSearch) where T: class
    {
        listy = objectToSearch.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour mb in listy)
        {
            if (mb is T)
            {
                return (T)((object)mb);
            }
        }
        return null;
    }
}
