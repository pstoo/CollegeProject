using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class RacerStartPosition : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private KartLocomotion SelectedCar;

    public delegate void RacerSpawnedDelegate(KartLocomotion racer);
    public RacerSpawnedDelegate RacerSpawned;

    // Start is called before the first frame update
    void Start()
    {
        var instance = Instantiate(SelectedCar, this.transform.position, this.transform.rotation);
        var rb = instance.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        instance.Input = inputManager;
        RacerSpawned?.Invoke(instance);
    }
}
