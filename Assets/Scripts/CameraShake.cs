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
    bool canShake = true;

    void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        distance = gm.GetDistance();
        if(canShake && distance <= range)
        {
            cam.transform.localPosition = originalPosition + Random.insideUnitSphere * intensity;
        }
        else if(canShake)
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

    public void SetShake(bool b)
    {
        canShake = b;
    }
}
