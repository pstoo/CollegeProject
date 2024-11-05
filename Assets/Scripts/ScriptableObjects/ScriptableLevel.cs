using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu( fileName = "New Track", menuName = "ScriptableObjects/Level")]
public class ScriptableLevel : ScriptableObject
{
    [SerializeField] private string m_name = "NEW TRACK";
    [SerializeField] private string address;
    [SerializeField] private int order;

    public string Name { get { return m_name; } }
    public string Address { get { return address;} }
    public int Order { get { return order; } }
}
