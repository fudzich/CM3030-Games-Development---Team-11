using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{
    public Slider expSlider;
    public float maxExp = 10f;
    public float currentExp;
    private float lerpSpeed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        currentExp = 0f;
        expSlider.minValue = 0f;
        expSlider.maxValue = maxExp;
    }

    // Update is called once per frame
    void Update()
    {
        if (expSlider.value != currentExp)
        {
            expSlider.value = Mathf.Lerp(expSlider.value, currentExp, lerpSpeed);
        }
    }

    public void updateExp(float value)
    {
        currentExp = value;
        expSlider.value = currentExp;
    }

    public void updateMaxExp(float value)
    {
        maxExp = value;
        expSlider.maxValue = maxExp;
    }
}