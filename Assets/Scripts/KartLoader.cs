using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class KartLoader : MonoBehaviour
{
    private List<SelectableKart> kartSelection = new();
    [SerializeField] private CatalogLoader catalogs;
    [SerializeField] private AsyncOperationHandle<IList<SelectableKart>> kartDataHandler;

    public event KartLoadingCompleteHandler LoadingComplete;
    public delegate void KartLoadingCompleteHandler();

    // Start is called before the first frame update
    void Start()
    {
        //catalogs.CatalogLoadingComplete += () =>
        {
            kartDataHandler = Addressables.LoadAssetsAsync<SelectableKart>("selectableKart", null);
            kartDataHandler.Completed += LoadSelectableKarts;
        };
    }

    private void LoadSelectableKarts(AsyncOperationHandle<IList<SelectableKart>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Failed) //If anything goes wrong, warn us, but load what you can.
            Debug.LogWarning("Not everything was loaded successfully. Make sure catalog is loaded correctly");

        if (handle.Result != null) //If anything WAS loaded, go ahead and load it.
            kartSelection = (List<SelectableKart>)handle.Result;

        kartSelection.Sort((x, y) => x.Order.CompareTo(y.Order)); //Sort it by order.
        LoadingComplete?.Invoke(); //Loading's complete, let's go.
    }

    public void SelectKart(GameObject kartSelectionMenu, GameObject trackSelectionMenu, SelectableKart kart)
    {
        //Make kart selection.
        kartSelectionMenu.SetActive(false);
        PersistantPlayerData.data.chosenKart = kart;
        trackSelectionMenu.SetActive(true);
    }

    public List<SelectableKart> KartSelection { get { return kartSelection;} }
}
