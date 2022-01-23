using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Torch : MonoBehaviour
{
    LightMecanic l;
    Frost frost;
    float red = 25;

    // Start is called before the first frame update
    void Start()
    {
        l = GameObject.Find("Player").GetComponent<LightMecanic>();
        frost = GameObject.Find("Player").GetComponent<Frost>();
    }

    // Update is called once per frame
    void Update()
    {
        if (l.torchValue <= 0 && !frost.AtFirecamp()) frost.SetCold(true);
        this.GetComponent<RawImage>().color = new Color32((byte)(CalculateColor()), 125, 125, 255);
    }

    float CalculateColor()
    {
        return ((130 * l.torchValue) / 7) + 125;
    }
}