using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private string QuickRaceSetUpScene;

    public void LoadQuickRace()
    {
        SceneManager.LoadScene(QuickRaceSetUpScene);
    }
}
