using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class KartSelectorPopulator : MonoBehaviour
{

    [SerializeField] private KartLoader kartLoader;
    [SerializeField] private LayoutElement buttonContainer;
    [SerializeField] private LayoutElement referenceInScene; //disable later
    [SerializeField] private GameObject kartButtonPrefab;
    [SerializeField] private RectTransform trackSelectorPopulator;
    private List<GameObject> buttons = new();

    // Start is called before the first frame update
    void Start()
    {
        if (referenceInScene.gameObject.activeInHierarchy)
            referenceInScene.gameObject.SetActive(false);

        kartLoader.LoadingComplete += () =>
        {
            foreach (SelectableKart kart in kartLoader.KartSelection)
            {
                GameObject buttonInstance = Instantiate(kartButtonPrefab, buttonContainer.transform);
                TextMeshProUGUI buttonText = buttonInstance.GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = kart.Name;
                Button button = buttonInstance.GetComponent<Button>();
                button.onClick.AddListener(() => kartLoader.SelectKart(buttonContainer.transform.parent.gameObject, trackSelectorPopulator.transform.parent.gameObject, kart));

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
