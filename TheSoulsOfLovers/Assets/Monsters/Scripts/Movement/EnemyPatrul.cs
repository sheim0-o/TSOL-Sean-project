using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrul : MonoBehaviour
{
    [HideInInspector]
    public int randomSpot;
    private float waitTime;

    public Transform[] moveSpots;
    public float startWaitTime;

    public void StartPatrul(float speed)
    {
        transform.position = Vector2.MoveTowards(transform.position, moveSpots[randomSpot].position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, moveSpots[randomSpot].position) < 0.2f)
        {
            if (waitTime <= 0)
            {
                randomSpot = Random.Range(0, moveSpots.Length);
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }
}
