using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class TrackLoader : MonoBehaviour
{
    private static List<string> keys = new() { "levelData" }; //TODO: A collection of keys may not be necessary.

    private List<ScriptableLevel> levelData = new();

    private AsyncOperationHandle<IList<ScriptableLevel>> levelDataHandle;
    private AsyncOperationHandle<SceneInstance> trackHandle;

    public event LoadingCompleteHandler LoadingComplete;
    public delegate void LoadingCompleteHandler();

    public void LoadTrack(string address)
    {
        trackHandle = Addressables.LoadSceneAsync(address, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    private void Start()
    {
        //Load Addressables
        //TODO: Normally keys would go here in place of "levelData". But a collection of keys may not be necessary.
        levelDataHandle = Addressables.LoadAssetsAsync<ScriptableLevel>("levelData", null); 
        levelDataHandle.Completed += LoadLevelData;
    }

    private void LoadLevelData(AsyncOperationHandle<IList<ScriptableLevel>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Failed) //If anything goes wrong, warn us, but load what you can.
            Debug.LogWarning("Not everything was loaded successfully. Make sure catalog is loaded correctly");

        if (handle.Result != null) //If anything WAS loaded, go ahead and load it.
            levelData = (List<ScriptableLevel>)handle.Result;

        levelData.Sort((x, y) => x.Order.CompareTo(y.Order)); //Sort it by order.
        LoadingComplete?.Invoke(); //Loading's complete, let's go.
    }

    private void OnDestroy()
    {
        if (levelDataHandle.IsValid())
            Addressables.Release(levelDataHandle);
        //Throwing a warning because it's trying to unload the only loaded scene. This will need to be mirrored some how.
        // if (trackHandle.IsValid())
        //     Addressables.UnloadSceneAsync(trackHandle);
    }

    public List<ScriptableLevel> LevelData { get { return levelData; } }
}
