using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinish : MonoBehaviour
{
    [SerializeField]
    private SceneSelector sceneSelector;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            sceneSelector.ReloadCurrentLevel();
        }
    }

    void OnTriggerEnter()
    {
        Destroy(gameObject);
        sceneSelector.LoadNextLevel();
    }
}
