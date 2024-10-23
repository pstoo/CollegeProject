using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUGRigidbodyJostler : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Vector3 force = new Vector3(0, 9000, 0);
    [SerializeField] private List<Vector3> jostlePoints;
    
    public void Jostle()
    {
        foreach (Vector3 point in jostlePoints)
            rb.AddForceAtPosition(force, rb.transform.position + point);
    }

    private void OnDrawGizmos() 
    {
        foreach (Vector3 point in jostlePoints)
            Gizmos.DrawSphere(rb.transform.position + point, 0.1f);    
    }
}
