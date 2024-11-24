using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class RacerStartPosition : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private KartLocomotion SelectedCar;
    private AsyncOperationHandle<GameObject> opHandle;

    public delegate void RacerSpawnedDelegate(GameObject racer);
    public RacerSpawnedDelegate RacerSpawned;

    // Start is called before the first frame update
    void Start()
    {
        opHandle = Addressables.LoadAssetAsync<GameObject>(PersistantPlayerData.data.chosenKart.Address);
        opHandle.Completed += LoadKart;

        
    }

    private void LoadKart(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            //var result = handle.Result;
            var instance = Instantiate(handle.Result, transform.position, transform.rotation);
            var locomotion = instance.GetComponent<KartLocomotion>();

            locomotion.Input = inputManager;
            RacerSpawned?.Invoke(instance);
        }
    }

    private void OnDestroy() {
        opHandle.Release();
    }
}
