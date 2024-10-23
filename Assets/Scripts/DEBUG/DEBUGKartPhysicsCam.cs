using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class DEBUGKartPhysicsCam : MonoBehaviour
{
    [SerializeField] private List<CinemachineVirtualCamera> cameras;
    private int activeCameraIndex = 0;
    
    public void NextCamera()
    {
        cameras[activeCameraIndex].gameObject.SetActive(false);
        activeCameraIndex = (activeCameraIndex + 1) % cameras.Count;
        cameras[activeCameraIndex].gameObject.SetActive(true);
    }

    public void PrevCamera()
    {
        cameras[activeCameraIndex].gameObject.SetActive(false);
        activeCameraIndex = (activeCameraIndex - 1 + cameras.Count) % cameras.Count;
        cameras[activeCameraIndex].gameObject.SetActive(true);
    }
}
