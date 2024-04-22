using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    public enum MovementType
    {
        Moveing,
        Lerping
    }

    public MovementType type = MovementType.Moveing;
    public MovementPath myPath;
    public float speed = 1;
    public float maxDistance = .1f;

    private float waitTime;
    public float startWaitTime;

    private IEnumerator<Transform> pointInPath;

    // Start is called before the first frame update
    void Start()
    {
        if (myPath == null)
        {
            Debug.Log("Путь не применен");
            return;
        }

        pointInPath = myPath.GetNextPathPoint();

        pointInPath.MoveNext();

        if (pointInPath.Current == null)
        {
            return;
        }

        transform.position = pointInPath.Current.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pointInPath == null || pointInPath.Current == null)
        {
            return;
        }
        if (type == MovementType.Moveing)
        {
            transform.position = Vector3.MoveTowards(transform.position, pointInPath.Current.position, Time.deltaTime * speed);

        }
        else if (type == MovementType.Lerping)
        {
            transform.position = Vector3.Lerp(transform.position, pointInPath.Current.position, Time.deltaTime * speed);
        }

        var distanceSqure = (transform.position - pointInPath.Current.position).sqrMagnitude;
        GetComponent<Animator>().SetFloat("speed", speed);

        Vector3 dirVector = pointInPath.Current.position - transform.position;

        dirVector.Normalize();
        GetComponent<Animator>().SetFloat("horizontal", dirVector.x);
        GetComponent<Animator>().SetFloat("vertical", dirVector.y);

        if (distanceSqure < maxDistance * maxDistance)
        {
            if (waitTime <= 0)
            {
                pointInPath.MoveNext();
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
            
        }
    }
}
