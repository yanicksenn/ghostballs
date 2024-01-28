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

    void OnTriggerEnter(Collider collider)
    {
        if (!isPressed && collider.gameObject.GetComponent<Walker>() != null)
        {
            PressButton();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isPressed && collision.gameObject.GetComponent<Walker>() != null)
        {
            PressButton();
        }
    }

    private void PressButton()
    {
        isPressed = true;
        animator.SetBool("isPressed", true);
        target.TriggerButtonEffect();
    }
}
