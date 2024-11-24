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

    //TODO: In the final game, loading catalogs will take place before loading anything else, so these events will be removed.
    //For now, it's all happening on one scene.
    public event CatalogLoadingCompleteHandler CatalogLoadingComplete;
    public delegate void CatalogLoadingCompleteHandler();


    // Start is called before the first frame update
    void Start()
    {
        buildPlatform = Application.platform.ToString();
        
        addressablePath = Addressables.RuntimePath + "/Data";
        try {
            dlcPackages = Directory.GetDirectories(addressablePath);

        }
        catch 
        {
            dlcPackages = new string[1];
        }
        StartCoroutine(LoadCatalogs());
    }

    private IEnumerator LoadCatalogs()
    {
        foreach (string path in dlcPackages)
        {
            if (dlcPackages[0] == null)
                break;
            //Due to limitations of the Addressables system, at least one catalog must be within the runtime path.
            //Might as well have that be the base addressables.
            if (path.Contains("Base"))
                continue; //If we're in the base assets, we don't care keep going.

            string catalogPath = path + "\\catalog.json";
            if (File.Exists(catalogPath))
            {
                AsyncOperationHandle<IResourceLocator> handle = Addressables.LoadContentCatalogAsync(catalogPath, true);
                catalogHandles.Add(handle);
                yield return handle; //wait until the catalog is loaded or something happens.
                if (handle.Status != AsyncOperationStatus.Succeeded) //Print error if this catalog failed to load.
                    Debug.LogWarning($"There was a problem with loading the catalog in {path}.");
            }
            else
                Debug.LogWarning($"No such catalog exists at {path}. Is it in the right location?");
        }
        Debug.Log("catalog loader's workin'");
        
        CatalogLoadingComplete?.Invoke();
    }
}
