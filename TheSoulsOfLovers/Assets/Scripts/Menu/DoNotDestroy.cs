using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DoNotDestroy : MonoBehaviour
{
    [HideInInspector] public bool isOld = false;
    void Awake()
    {
        List<GameObject> musicObjs = GameObject.FindGameObjectsWithTag("GameMusic").ToList();

        if(musicObjs.Count==1)
        {
            isOld = true;
            DontDestroyOnLoad(this.gameObject);
            return;
        }

        GameObject oldItem = null;
        foreach(GameObject item in musicObjs)
        {
            if (item.GetComponent<DoNotDestroy>() && item.GetComponent<DoNotDestroy>().isOld)
                oldItem = item;
        }
        string oldItemName = oldItem.GetComponent<AudioSource>().clip.name;
        bool isSameMusic = true;
        foreach (GameObject item in musicObjs)
        {
            if (item.GetComponent<AudioSource>() && item.GetComponent<AudioSource>().clip.name != oldItemName)
                isSameMusic = false;
        }

        if (isSameMusic)
        {
            foreach (GameObject item in musicObjs)
            {
                if (!item.GetComponent<DoNotDestroy>().isOld)
                    DestroyImmediate(item);
            }
        }
        else
        {
            foreach (GameObject item in musicObjs)
            {
                if (item.GetComponent<AudioSource>() && item.GetComponent<DoNotDestroy>() && item.GetComponent<AudioSource>().clip.name != oldItemName)
                {
                    item.GetComponent<DoNotDestroy>().isOld = true;
                    DontDestroyOnLoad(item);
                }
                else
                    DestroyImmediate(item);
            }
        }
    }
}