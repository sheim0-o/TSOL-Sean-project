using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestBarrier : MonoBehaviour
{
    private TextMeshProUGUI infoUIText;
    public string textToPlayer = "";
    private void Start()
    {
        infoUIText = GameObject.FindWithTag("InteractableText").GetComponent<TextMeshProUGUI>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            infoUIText.text = textToPlayer;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            infoUIText.text = "";
        }
    }
}
