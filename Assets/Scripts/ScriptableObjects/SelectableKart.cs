using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "New Kart Selection", menuName = "ScriptableObjects/SelectableKart")]
public class SelectableKart : ScriptableObject
{
    [SerializeField] private string m_name = "Untitled Kart";
    [SerializeField] private string address;
    [SerializeField] private int order;
    //[SerializeField] private KartLocomotion prefab;

    public string Name { get { return m_name; } }
    public int Order { get { return order; } }
    public string Address { get { return address;} }
    //public KartLocomotion Prefab { get { return prefab; } }
}
//It pains me that this isn't an interface - it's practically a 1:1 copy of the track SO
//BUT DAMMIT, WE'RE SO CLOSE TO THE FINAL STRETCH - CAN'T AFFORD ABSTRACTION