using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideWalls : MonoBehaviour
{
    private string tagToHide = "HideInArea";
    private Transform transform;
    private Transform player;
    private List<Transform> transformsToHide;

    void Start()
    {
        transform = GetComponent<Transform>();
        if (GameObject.FindWithTag("Player"))
        {
            player = GameObject.FindWithTag("Player").transform;
        }
        transformsToHide = new List<Transform>();
        FindChildWithTag(transform);
    }
    void FindChildWithTag(Transform father)
    {
        for (int i = 0; i < father.childCount; i++)
        {
            if (father.GetChild(i).tag == tagToHide)
                transformsToHide.Add(father.GetChild(i));
            else if (father.GetChild(i).childCount > 0)
                FindChildWithTag(father.GetChild(i));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player.gameObject)
        {
            foreach (Transform transToHide in transformsToHide)
            {
                transToHide.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .25f);
                transToHide.GetComponent<SpriteRenderer>().sortingOrder = player.GetComponent<SpriteRenderer>().sortingOrder + 1;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player.gameObject)
        {
            foreach (Transform transToHide in transformsToHide)
            {
                transToHide.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                transToHide.GetComponent<SpriteRenderer>().sortingOrder = player.GetComponent<SpriteRenderer>().sortingOrder;
            }
        }
    }
}
