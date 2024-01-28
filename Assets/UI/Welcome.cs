using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Welcome : MonoBehaviour
{
    [SerializeField]
    private SceneSelector sceneSelector;

    private PlayersControls playersControls;

    private void Awake()
    {
        playersControls = new PlayersControls();
    }
    private void OnEnable()
    {
        playersControls.Enable();
    }

    private void OnDisable()
    {
        playersControls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        var resetButtonPressed = playersControls.Controls.Reset.ReadValue<float>() == 1.0;

        if (Input.anyKey || resetButtonPressed)
        {
            Debug.Log("A key or mouse click has been detected");
            sceneSelector.LoadFirstLevel();
        }
    }
}
