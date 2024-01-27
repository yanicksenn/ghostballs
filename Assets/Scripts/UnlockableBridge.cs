using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockableBridge : ButtonReceiver
{
    private Animator animator;
    private new Collider collider;

    public override void TriggerButtonEffect()
    {
        animator.SetBool("isUnlocked", true);
        collider.enabled = false;
    }

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        collider = GetComponent<Collider>();
    }
}
