using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Welcome : MonoBehaviour
{
    [SerializeField]
    private SceneSelector sceneSelector;

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            Debug.Log("A key or mouse click has been detected");
            sceneSelector.LoadFirstLevel();
        }
    }
}
