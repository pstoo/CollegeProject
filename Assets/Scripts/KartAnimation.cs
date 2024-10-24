using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KartAnimation : MonoBehaviour
{
    [SerializeField] private ScriptableKart kart;
    [SerializeField] private List<Transform> tireGFX;
    private Dictionary<int, float> suspensionPoints = new();

    public void UpdateTireRotation(int index, float y)
    {
        if (suspensionPoints.ContainsKey(index))
            tireGFX[index].localEulerAngles = new Vector3(tireGFX[index].localEulerAngles.x, y, tireGFX[index].localEulerAngles.z);
    }

    public void UpdateTirePosition(int index, float x, float z)
    {
        if (suspensionPoints.ContainsKey(index))
            tireGFX[index].position = new Vector3(x, suspensionPoints[index], z);
    }

    public void UpdateSuspensionPoint(int index, float point)
    {
        if (!suspensionPoints.ContainsKey(index))
            suspensionPoints.Add(index, point);
        else
            suspensionPoints[index] = point + kart.TireRadius;
    }
}
