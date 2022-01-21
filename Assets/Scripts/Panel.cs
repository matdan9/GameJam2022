using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel
{
    GameObject panel;

    public Panel(string name, Transform parent) //Constructor
    {
        panel = new GameObject(name);
        panel.transform.SetParent(parent);
        panel.AddComponent<RectTransform>();
    }
    
    //Add methods
    public void AddImage()
    {
        panel.AddComponent<Image>();
    }

    public void AddRawImage()
    {
        panel.AddComponent<RawImage>();
    }

    public void AddText()
    {
        panel.AddComponent<Text>();
    }

    public void AddButton()
    {
        panel.AddComponent<Button>();
    }

    //Set methods
    public void SetPosition(Vector2 position)
    {
        panel.GetComponent<RectTransform>().anchoredPosition = position;
    }

    public void SetSize(Vector2 size)
    {
        panel.GetComponent<RectTransform>().sizeDelta = size;
    }

    public void SetImageColor(Color32 color)
    {
        panel.GetComponent<Image>().color = color;
    }

    public void SetRawImageColor(Color32 color)
    {
        panel.GetComponent<RawImage>().color = color;
    }

    public void SetRawImageTexture(string name)
    {
        panel.GetComponent<RawImage>().texture = Resources.Load(name) as Texture;
    }

    public void SetRawImageTransparency(byte alpha)
    {
        Color32 color = panel.GetComponent<RawImage>().color;
        color.a = alpha;
        panel.GetComponent<RawImage>().color = color;
    }

    public void SetAnchors(Vector2 min, Vector2 max)
    {
        panel.GetComponent<RectTransform>().anchorMin = min;
        panel.GetComponent<RectTransform>().anchorMax = max;
    }

    public void SetPivot(Vector2 pivot)
    {
        panel.GetComponent<RectTransform>().pivot = pivot;
    }

    public void SetTextColor(Color32 color)
    {
        panel.GetComponent<Text>().color = color;
    }

    public void SetTextFont()
    {
        panel.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
    }

    public void SetTextString(string text)
    {
        panel.GetComponent<Text>().text = text;
    }

    public void SetTextAlignment()
    {
        panel.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
    }

    public void SetButtonTargetGraphic(Image image)
    {
        panel.GetComponent<Button>().targetGraphic = image;
    }

    //Get methods
    public GameObject GetPanelObject()
    {
        return panel;
    }

    public Transform GetTransform()
    {
        return panel.transform;
    }

    public Image GetImage()
    {
        return panel.GetComponent<Image>();
    }

    public Button GetButton()
    {
        return panel.GetComponent<Button>();
    }
}
