using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class TrackLoader : MonoBehaviour
{
    [SerializeField] private string defaultTrack = "KeplerCircuit"; //not good but servicible until something better comes along
    [SerializeField] private string[] keys; //Kepler circuit needs to be "unbaked" into the game, so as to make it so that there is no bloat.
    private AsyncOperationHandle<SceneInstance> trackHandle;

    private void Start()
    {
        //Load Addressables
    }

    public void LoadTrack(int index)
    {
        if (index > -1)
            trackHandle = Addressables.LoadSceneAsync(keys[index], UnityEngine.SceneManagement.LoadSceneMode.Single);
        
    }

    private void OnDestroy()
    {
        //Throwing a warning because it's trying to unload the only loaded scene. This will need to be mirrored some how.
        // if (trackHandle.IsValid())
        //     Addressables.UnloadSceneAsync(trackHandle);
    }

    public string[] TrackKeys { get { return keys; } }

}
