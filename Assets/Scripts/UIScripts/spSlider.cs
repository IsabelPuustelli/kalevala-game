using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spSlider : MonoBehaviour
{
    public Slider slider;
    public static int maxSp;
    public static int currentSp;

    // Start is called before the first frame update
    void Start()
    {
        slider = slider.GetComponent<Slider>();
    }
}
