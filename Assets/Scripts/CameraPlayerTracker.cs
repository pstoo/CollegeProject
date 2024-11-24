using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraPlayerTracker : MonoBehaviour
{
    [SerializeField] private RacerStartPosition startPosition;
    private CinemachineVirtualCamera vcam;

    // Start is called before the first frame update
    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        if (startPosition != null)
            startPosition.RacerSpawned += SetCamera;
    }

    private void SetCamera(GameObject racer)
    {
        vcam.Follow = racer.transform;
        vcam.LookAt = racer.transform;
    }

    private void OnDestroy()
    {
        if (startPosition != null)
            startPosition.RacerSpawned += SetCamera;
    }
}
