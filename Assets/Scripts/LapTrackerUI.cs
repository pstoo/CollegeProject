using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LapTrackerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currentLapText;

    private void Start() 
    {
        CheckpointMonitor.OnLapPassed += UpdateUI;
    }

    private void OnDestroy()
    {
        CheckpointMonitor.OnLapPassed -= UpdateUI;
    }

    private void UpdateUI(int currentLap)
    {
        currentLapText.text = $"{currentLap}";
    }
}
