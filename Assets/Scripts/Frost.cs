using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Frost : MonoBehaviour
{
    public GameObject frostOverlay;
    public float multiplier;
    float frostValue;
    bool cold = false;

    public void Start()
    {
        SetFrost(0);
        SetMultiplier(1);
    }

    public void FixedUpdate()
    {
        frostValue = Mathf.Clamp(frostValue, 0, 255);
        Color32 temp = frostOverlay.GetComponent<RawImage>().color;
        temp.a = (byte)frostValue; //color.a is transparency
        frostOverlay.GetComponent<RawImage>().color = temp;

        if (cold) AddFrost(0.1f * multiplier);
        else RemoveFrost(0.1f * multiplier);
    }

    public void SetFrost(float alpha)
    {
        frostValue = alpha;
    }

    public void AddFrost(float alpha)
    {
        frostValue += alpha;
    }

    public void RemoveFrost(float alpha)
    {
        frostValue -= alpha;
    }

    public void SetCold(bool status)
    {
        cold = status;
    }

    public void SetMultiplier(float value)
    {
        multiplier = value;
    }
}
