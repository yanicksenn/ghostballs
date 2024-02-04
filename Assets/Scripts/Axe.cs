using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    private Killable thisKillable;

    private void Awake()
    {
        thisKillable = GetComponentInParent<Killable>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!collider.TryGetComponent<Killable>(out var killable))
        {
            return;
        }
        if (killable == thisKillable)
        {
            return;
        }

        killable.Die();
    }
}
