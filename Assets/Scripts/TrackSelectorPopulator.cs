using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrackSelectorPopulator : MonoBehaviour
{
    [SerializeField] private TrackLoader trackLoader;
    [SerializeField] private LayoutElement referenceInScene; //To be disabled
    [SerializeField] private GameObject trackButtonPrefab;
    private List<GameObject> buttons = new();

    private void Start() 
    {
        if (referenceInScene.gameObject.activeInHierarchy)
            referenceInScene.gameObject.SetActive(false);

        for (int i = 0; i < trackLoader.TrackKeys.Length; i++)
        {
            int index = i; //This was a learning experience. Lambda closures are weird.
            GameObject buttonInstance = Instantiate(trackButtonPrefab, this.transform);
            TextMeshProUGUI buttonText = buttonInstance.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = trackLoader.TrackKeys[i];
            Button button = buttonInstance.GetComponent<Button>();
            button.onClick.AddListener(() => trackLoader.LoadTrack(index));
           
            buttons.Add(buttonInstance);
        }
    }

    private void OnDestroy()
    {
        foreach(GameObject buttonInstance in buttons)
        {
            Button button = buttonInstance.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
        }
    }
}
