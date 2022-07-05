using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameExtension
{
    public static void SetActiveObjects(this List<GameObject> objectLists, bool active)
    {
        if (objectLists != null && objectLists.Count > 0)
        {
            for (int i = 0; i < objectLists.Count; i++)
            {
                var obj = objectLists[i];
                if (obj == null) { continue; }
                if (obj.activeInHierarchy == active) { continue; }
                obj.SetActive(active);
            }
        }
    }

    //todo other extensions
}
