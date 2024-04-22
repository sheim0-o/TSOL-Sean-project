using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RchagDown : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(Open());
        }
    }

    IEnumerator Open()
    {
        animator.SetBool("LeverDown", true);
        AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(animatorStateInfo.length);
        animator.enabled = false;
    }
}
