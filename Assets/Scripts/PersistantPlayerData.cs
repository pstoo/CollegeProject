using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistantPlayerData : MonoBehaviour
{
    public SelectableKart chosenKart;
    public static PersistantPlayerData data;

    void Awake() //singleton;
    {
        if (data != null)
        {
            Destroy(gameObject);
            return;
        }
        data = this;
        DontDestroyOnLoad(gameObject);
    }
}
