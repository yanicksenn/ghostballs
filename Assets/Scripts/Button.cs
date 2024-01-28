using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{

    [SerializeField, Tooltip("The target of the button.")]
    private ButtonReceiver target;
    [SerializeField]  
    AudioManager audioManager;
    private Animator animator;

    private bool isPressed = false;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        audioManager =GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
      
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
        audioManager.PlaySFX(audioManager.button);
        isPressed = true;
        animator.SetBool("isPressed", true);
        target.TriggerButtonEffect();
    }
}
