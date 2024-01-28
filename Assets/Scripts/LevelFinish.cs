using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinish : MonoBehaviour
{
    [SerializeField]
    private SceneSelector sceneSelector;
    
    [SerializeField]
    private Possession possession;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            sceneSelector.ReloadCurrentLevel();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        var possessible = collider.gameObject.GetComponent<Possessable>();
        if (possessible != null && possessible == possession.FallbackPossessable && possessible.IsPossessed)
        {
            Destroy(gameObject);
            sceneSelector.LoadNextLevel();
        }
    }
}
