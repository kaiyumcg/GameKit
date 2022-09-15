using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class GameobjectEx
{
    public static void ExSetActiveObjects(this List<GameObject> objectLists, bool active)
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

    //todo set layer, set layer to all children

}