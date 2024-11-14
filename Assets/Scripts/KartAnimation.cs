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
            tireGFX[index].localEulerAngles = new Vector3
            (
                tireGFX[index].localEulerAngles.x,
                y,
                tireGFX[index].localEulerAngles.z
            );
    }

    public void UpdateTirePosition(int index, float x, float z)
    {
        if (suspensionPoints.ContainsKey(index))
            tireGFX[index].localPosition = new Vector3
            (
                x,
                suspensionPoints[index],
                z
            );
    }

    public void UpdateSuspensionPoint(int index, RaycastHit hit)
    {
        Vector3 local = transform.InverseTransformPoint(hit.point);

        if (!suspensionPoints.ContainsKey(index))
            suspensionPoints.Add(index, hit.point.y);
        else
        {
            if (hit.collider != null)
                suspensionPoints[index] = local.y + kart.TireRadius;
            else
                suspensionPoints[index] = kart.TireRadius;
        }
    }
}
