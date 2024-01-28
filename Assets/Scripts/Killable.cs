using System;
using UnityEngine;
using UnityEngine.Events;

public class Killable : MonoBehaviour
{

    [SerializeField]
    private bool isDead;
    public bool IsDead => isDead;

    [SerializeField]
    private KillableEvent onDeathEvent = new();
    public KillableEvent OnDeathEvent => onDeathEvent;

    virtual public void Die()
    {
        if (!isDead)
        {
            isDead = true;
            OnDeathEvent.Invoke(this);
        }
    }

    [Serializable]
    public class KillableEvent : UnityEvent<Killable> { }
}
