using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject player, bigBoy;
    [SerializeField]
    float distance;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        bigBoy = GameObject.Find("bigBoy");
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        distance = CalculateDistance(bigBoy.transform, player.transform);
    }

    float CalculateDistance(Transform other, Transform player)
    {
        return Vector3.Distance(other.position, player.position);
    }

    public float GetDistance()
    {
        return distance;
    }
}
