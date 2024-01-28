using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Scene Selector", fileName = "Scene Selector")]
public class SceneSelector : ScriptableObject
{
    public List<string> levelNames = new();

    [SerializeField]
    private int currentLevelIndex = 0;

    public void LoadFirstLevel()
    {
        currentLevelIndex = 0;
        SceneManager.LoadScene(levelNames[0]);
    }

    public void ReloadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadNextLevel()
    {
        currentLevelIndex++;
        SceneManager.LoadScene(levelNames[currentLevelIndex % levelNames.Count]);
    }
}
