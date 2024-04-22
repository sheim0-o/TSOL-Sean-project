using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricPlayer : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        GetComponent<Renderer>().sortingOrder = Mathf.RoundToInt((transform.position.y - 2) * 1f) * -1;
    }
}
