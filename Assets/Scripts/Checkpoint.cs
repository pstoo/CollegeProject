using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<KartLocomotion>(out KartLocomotion kart))
        {
            CheckpointMonitor.Singleton.CheckpointPassed(this);
        }
    }
}
