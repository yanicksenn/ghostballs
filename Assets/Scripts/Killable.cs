using System;
using UnityEngine;
using UnityEngine.Events;

public class Killable : MonoBehaviour
{

    [SerializeField]
    private bool isDead;
    public bool IsDead => isDead;
    private Animator animator;

    [SerializeField]
    private KillableEvent onDeathEvent = new();
    public KillableEvent OnDeathEvent => onDeathEvent;

    virtual protected void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    virtual public void Die()
    {
        if (!isDead)
        {
            isDead = true;
            if (animator != null)
            {
                animator.SetTrigger("Die");
            }
            OnDeathEvent.Invoke(this);
        }
    }

    [Serializable]
    public class KillableEvent : UnityEvent<Killable> { }
}
