using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Lever : MonoBehaviour
{
    private Animator animator;
    private Transform player;
    [HideInInspector]public bool lowered;
    [SerializeField]
    private TextMeshProUGUI text;
    private bool isCharacterIn = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (GameObject.FindWithTag("Player"))
        {
            player = GameObject.FindWithTag("Player").transform;
        }
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E) && isCharacterIn)
        {
            lowered = true;
            animator.Play("Levered");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player.gameObject)
        {
            text.text = "Нажмите E для взаимодействия";
            isCharacterIn = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player.gameObject)
        {
            text.text = "";
            isCharacterIn = false;
        }
    }
}
