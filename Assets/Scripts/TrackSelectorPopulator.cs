using System.Collections.Generic;
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

        trackLoader.LoadingComplete += () =>
        {
            foreach (ScriptableLevel levelData in trackLoader.LevelData)
            {
                //Set up button properties using prefab
                GameObject buttonInstance = Instantiate(trackButtonPrefab, this.transform);
                TextMeshProUGUI buttonText = buttonInstance.GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = levelData.Name;
                Button button = buttonInstance.GetComponent<Button>();
                button.onClick.AddListener(() => trackLoader.LoadTrack(levelData.Address));

                //Add to list to remove it later.
                buttons.Add(buttonInstance);
            }
        };
    }

    private void OnDestroy()
    {
        foreach (GameObject buttonInstance in buttons)
        {
            Button button = buttonInstance.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
        }
    }
}
