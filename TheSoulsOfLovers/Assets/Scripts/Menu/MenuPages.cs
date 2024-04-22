using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPages : MonoBehaviour
{
    public MainMenu mainMenu;
    private void OnEnable()
    {
        mainMenu.SelectFirstButton(transform);
    }
}