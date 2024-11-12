using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CheckpointMonitor : MonoBehaviour
{
    public static CheckpointMonitor Singleton { get; private set; }

    [SerializeField] private int totalLaps = 3;
    [SerializeField] private int currentLap = 1;
    
    private LapTrackerUI UI;
    private List<Checkpoint> checkpoints = new();
    [SerializeField] private int nextCheckpoint = 0;

    public delegate void LapDelegate(int currentLap);
    public event LapDelegate OnLapPassed;

    void Awake()
    {
        if (Singleton == null)
            Singleton = this;
        else
            Destroy(this);

        foreach (Transform child in transform)
        {
            Checkpoint checkpoint;
            if (child.TryGetComponent<Checkpoint>(out checkpoint))
                checkpoints.Add(checkpoint);
        }
    }

    public void CheckpointPassed(Checkpoint checkpoint)
    {
        //If our next checkpoint would be higher than our current checkpoint, loop back around
        if (nextCheckpoint == checkpoints.Count)
            nextCheckpoint = 0;

        //If we crossed checkpoint 0, that means we've made a new lap
        if (nextCheckpoint == 0)
            {
                currentLap++;
                OnLapPassed?.Invoke(currentLap);
            }
        
        //If this is the right order,
        if (checkpoints.IndexOf(checkpoint) == nextCheckpoint)
            nextCheckpoint++;
    }
}
