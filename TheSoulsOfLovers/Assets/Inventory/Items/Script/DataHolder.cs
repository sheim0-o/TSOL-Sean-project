using UnityEngine;

public static class DataHolder
{
    private static GameObject prefabName;

    public static GameObject Prefab
    {
        get
        {
            return prefabName;
        }
        set
        {
            prefabName = value;
        }
    }
}