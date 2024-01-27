using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{

    [SerializeField, Tooltip("The target of the button.")]
    private ButtonReceiver target;

    private Animator animator;

    private bool isPressed = false;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void OnTriggerEnter()
    {
        if (!isPressed)
        {
            isPressed = true;
            animator.SetBool("isPressed", true);
            target.TriggerButtonEffect();
        }
    }
}
