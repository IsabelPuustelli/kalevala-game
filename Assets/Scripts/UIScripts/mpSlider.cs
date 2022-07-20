using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mpSlider : MonoBehaviour
{
    public Slider slider;
    public static int maxMp = 100;
    public static int currentMp;

    // Start is called before the first frame update
    void Start()
    {
        slider = slider.GetComponent<Slider>();
        currentMp = maxMp;
    }

}
