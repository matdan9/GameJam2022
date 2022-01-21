using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Generator : MonoBehaviour
{
    GameObject canvasObject;

    void Awake()
    {
        AddCanvas();
        AddEventSystems();
        AddFrostOverlay();
    }

    public void AddCanvas()
    {
        Canvas canvas;
        CanvasScaler canvasScaler;
        GraphicRaycaster canvasGraphicRayCaster;
        canvasObject = new GameObject("Canvas");
        canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasScaler = canvasObject.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasGraphicRayCaster = canvasObject.AddComponent<GraphicRaycaster>();
        canvasObject.transform.SetParent(this.gameObject.transform);
        canvasObject.transform.position = new Vector3(0, 0, 0);
    }

    public void AddEventSystems()
    {
        GameObject eventSystem;
        eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        eventSystem.transform.SetParent(this.transform);
        eventSystem.AddComponent<BaseInput>();
    }

    public void AddFrostOverlay()
    {
        Panel frostOverlay = new Panel("Frost Overlay", canvasObject.transform);
        frostOverlay.AddRawImage();
        frostOverlay.SetRawImageTexture("frost");
        frostOverlay.SetSize(new Vector2(800, 350));
        frostOverlay.SetRawImageTransparency(100);
    }
}