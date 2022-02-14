using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject bigBoy;

    [SerializeField]
    GameObject torch, torchFlame, torchFireParticle, torchSmoke;
    
    [SerializeField]
    float distance;

    void Awake()
    {
        FindGameObjects();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        distance = CalculateDistance(bigBoy.transform, player.transform);
    }

    void FindGameObjects()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        bigBoy = GameObject.Find("bigBoy");
        torch = GameObject.FindGameObjectWithTag("Torch");
        torchFlame = GameObject.Find("TorchFlame");
        torchFireParticle = GameObject.Find("TorchFireParticle");
        torchSmoke = GameObject.Find("TorchSmoke");
    }

    float CalculateDistance(Transform other, Transform player)
    {
        return Vector3.Distance(other.position, player.position);
    }

    public float GetDistance()
    {
        return distance;
    }
    
    //Get GameObjects
    public GameObject GetPlayerObject() { return player; }
    public GameObject GetBigBoyObject() { return bigBoy; }
    public GameObject GetTorchObject() { return torch; }
    public GameObject GetTorchFlameObject() { return torchFlame; }
    public GameObject GetTorchFireParticleObject() { return torchFireParticle; }
    public GameObject GetTorchSmokeObject() { return torchSmoke; }

    //Get Scripts
    public LightMechanic GetLightMechanic() { return player.GetComponent<LightMechanic>(); }
    public Frost GetFrost() { return player.GetComponent<Frost>(); }
}
