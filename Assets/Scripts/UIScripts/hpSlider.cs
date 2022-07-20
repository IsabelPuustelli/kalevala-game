using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hpSlider : MonoBehaviour
{
    public Slider slider;
    public static int maxHp = 100;
    public static int currentHp;

    // Start is called before the first frame update
    void Start()
    {
        slider = slider.GetComponent<Slider>();
        currentHp = maxHp;
    }
}
