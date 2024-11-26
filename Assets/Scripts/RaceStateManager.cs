using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public class RaceStateManager : MonoBehaviour
{
    private CheckpointMonitor monitor;
    [SerializeField] private LapTrackerUI lapTracker;
    [SerializeField] private RectTransform winScreen;
    
    private void Start() {
        monitor = GetComponent<CheckpointMonitor>();

        CheckpointMonitor.OnTrackComplete += Win;
    }

    private void Win()
    {
        //Disable Lap UI element
        lapTracker.gameObject.SetActive(false);
        //Show win screen
        winScreen.gameObject.SetActive(true);
    }

}
