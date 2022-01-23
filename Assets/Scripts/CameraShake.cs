using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    GameManager gm;
    [SerializeField]
    Camera cam;
    [SerializeField]
    float distance;
    [SerializeField]
    float range = 10f;
    [SerializeField]
    float intensity = 1f;
    [SerializeField]
    Vector3 originalPosition;

    void Awake()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        distance = gm.GetDistance();
        if(distance <= range)
        {
            cam.transform.localPosition = originalPosition + Random.insideUnitSphere * intensity;
        }

        else
        {
            cam.transform.localPosition = originalPosition;
        }

    }

    void OnEnable()
    {
        originalPosition = cam.transform.localPosition;
    }

    void Shake()
    {

    }

    void SetRange(float val)
    {
        range = val;
    }

    void SetIntensity(float val)
    {
        intensity = val;
    }
}
