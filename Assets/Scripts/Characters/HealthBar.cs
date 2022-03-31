using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//created with help from https://www.youtube.com/watch?v=BLfNP4Sc_iA

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private Gradient gradient;
    [SerializeField]
    private Image fill;
    /// <summary>
    /// set up slider max health
    /// </summary>
    /// <param name="value"></param>
    public void SetMaxHealth(float value)
    {
        slider.maxValue = value;
        slider.value = value;
        fill.color = gradient.Evaluate(1f);
    }
    /// <summary>
    /// set up slider current health
    /// </summary>
    /// <param name="value"></param>
    public void SetHealth(float value)
    {
        slider.value = value;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
