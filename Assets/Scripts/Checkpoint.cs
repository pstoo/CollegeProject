using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private CheckpointMonitor monitor;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<KartLocomotion>(out KartLocomotion kart))
        {
            monitor.CheckpointPassed(this);
        }
    }
    public CheckpointMonitor Monitor { set { monitor = value; } }
}
