using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class WinMenuManager : MonoBehaviour
{
    [SerializeField] private string menuScene;
    private AsyncOperationHandle<SceneInstance> trackHandle;
    public void ToMainMenu()
    {
        SceneManager.LoadScene(menuScene);
    }

    public void RaceAgain()
    {
        trackHandle = Addressables.LoadSceneAsync(PersistantPlayerData.data.lastPlayedLevel);
    }
}
