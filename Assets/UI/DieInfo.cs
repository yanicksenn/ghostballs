using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieInfo : MonoBehaviour
{
    public void OnPlayerDie()
    {
        GetComponent<Canvas>().enabled = true;
    }
}
