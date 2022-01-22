using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Frost : MonoBehaviour
{
    public GameObject frostOverlay;
    public float frostValue;
    public bool cold = false;

    public void Start()
    {
        SetFrost(0);
    }

    public void FixedUpdate()
    {
        frostValue = Mathf.Clamp(frostValue, 0, 255);
        Color32 temp = frostOverlay.GetComponent<RawImage>().color;
        temp.a = (byte)frostValue;
        frostOverlay.GetComponent<RawImage>().color = temp;

        if (cold) AddFrost(0.1f);
        else RemoveFrost(0.1f);
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
}
