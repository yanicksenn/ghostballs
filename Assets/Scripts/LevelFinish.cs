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
    private PlayersControls playersControls;

    private void OnEnable()
    {
        playersControls.Enable();
    }

    private void OnDisable()
    {
        playersControls.Disable();
    }

    private void Awake()
    {
        playersControls = new PlayersControls();
    }

    // Update is called once per frame
    void Update()
    {
        var resetButtonPressed = playersControls.Controls.Reset.ReadValue<float>() == 1.0;
        if (resetButtonPressed)
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
