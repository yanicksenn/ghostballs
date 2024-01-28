using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    private Killable thisKillable;
    private bool lethal = false;

    private void Awake() {
        thisKillable = GetComponentInParent<Killable>();
    }

    private void OnEnable()
    {
        lethal = true;
    }

    private void OnDisable()
    {
        lethal = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!lethal)
        {
            return;
        }
        if (!collision.collider.TryGetComponent<Killable>(out var killable))
        {
            return;
        }
        if (killable == thisKillable) {
            return;
        }

        killable.Die();
    }
}
