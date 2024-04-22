using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorByMR : MonoBehaviour
{
    public List<Transform> levers;
    private Animator animator;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool allLowered = true;
        foreach (Transform lever in levers)
        {
            if(!lever.GetComponent<Lever>().lowered)
                allLowered = false;
        }
        if (allLowered)
        {
            animator.Play("Open"); 
            audioSource.Play();
            transform.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
