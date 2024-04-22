using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    private Transform transform;
    private Animator animator;
    private bool down, up, hold;

    void Start()
    {
        animator = GetComponent<Animator>();
        transform = GetComponent<Transform>();
    }

    void Update()
    {
        down = Input.GetKeyDown(KeyCode.E);
        up = Input.GetKeyUp(KeyCode.E);
        hold = Input.GetKey(KeyCode.E);

        if (down)
        {
            animator.Play("DoorOpen");
            transform.GetComponent<Collider2D>().enabled = false;
            //StartCoroutine(Open());

        }
    }

}
