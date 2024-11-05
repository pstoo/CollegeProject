using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class CatalogLoader : MonoBehaviour
{
    private List<AsyncOperationHandle<IResourceLocator>> catalogHandles = new();
    private string addressablePath;
    private string[] dlcPackages;
    private string buildPlatform;

    public event CatalogLoadingCompleteHandler CatalogLoadingComplete;
    public delegate void CatalogLoadingCompleteHandler();


    // Start is called before the first frame update
    void Start()
    {
        buildPlatform = Application.platform.ToString();
#if UNITY_EDITOR
        buildPlatform = EditorUserBuildSettings.activeBuildTarget.ToString();
#endif

        //addressablePath = Path.Combine(Addressables.RuntimePath, buildPlatform);
        addressablePath = Addressables.RuntimePath + "/" + buildPlatform;
        dlcPackages = Directory.GetDirectories(addressablePath);
        StartCoroutine(LoadCatalogs());

    }

    private IEnumerator LoadCatalogs()
    {
        foreach (string path in dlcPackages)
        {
            if (path.Contains("Base"))
                continue;

            string catalogPath = path + "\\catalog.json";
            if (File.Exists(catalogPath))
            {
                AsyncOperationHandle<IResourceLocator> handle = Addressables.LoadContentCatalogAsync(catalogPath, true);
                catalogHandles.Add(handle);
                yield return handle;
            }
            else
            {
                Debug.LogWarning($"No such catalog exists at {path}. Is it in the right location?");
            }
        }
        CatalogLoadingComplete?.Invoke();
    }
}
