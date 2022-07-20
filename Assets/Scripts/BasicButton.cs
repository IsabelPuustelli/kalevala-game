using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicButton : MonoBehaviour
{
    public bool OptionsToggled = false;
    public GameObject options;
    public GameObject buttons;

    public void OpenOptions()
    {
        if (!OptionsToggled)
        {
            options.SetActive(true);
            buttons.SetActive(false);
            OptionsToggled = true;
        }
        else
        {
            options.SetActive(false);
            buttons.SetActive(true);
            OptionsToggled = false;
        }
    }
}
