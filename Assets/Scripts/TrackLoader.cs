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
    [SerializeField] private string[] keys;
    private AsyncOperationHandle<SceneInstance> trackHandle;

    public void LoadTrack(int index)
    {
        if (index != -1)
            trackHandle = Addressables.LoadSceneAsync(keys[index], UnityEngine.SceneManagement.LoadSceneMode.Single);
        else
            StartCoroutine(LoadDefaultTrack());
    }

    private IEnumerator LoadDefaultTrack()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(defaultTrack);

        while (!operation.isDone)
            yield return null;
    }

    private void OnDestroy() 
    {
        if (trackHandle.IsValid())
            Addressables.UnloadSceneAsync(trackHandle);
    }

}
