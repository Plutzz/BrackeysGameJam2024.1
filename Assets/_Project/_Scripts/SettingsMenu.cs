using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    public void CloseSettingsMenu()
    {
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
