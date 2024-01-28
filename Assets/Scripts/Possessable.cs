using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController))]
public class Possessable : Killable
{
    [SerializeField]
    private Possession possession;
    public bool IsPossessed => possession.CurrentPossession == this;

    [SerializeField]
    private bool possessAtStart;

    [SerializeField]
    private bool immortalWhenUnpossesed;

    private new Collider collider;
    private PlayersControls playersControls;

    private void Awake()
    {
        collider = GetComponent<Collider>();
        playersControls = new PlayersControls();
        if (possessAtStart)
        {
            possession.FallbackPossessable = this;
        }
    }

    private void OnEnable()
    {
        possession.OnPossessEvent.AddListener(OnPossess);
        possession.OnUnpossessEvent.AddListener(OnUnpossess);
        OnDeathEvent.AddListener(OnDeath);
        playersControls.Enable();
    }

    private void OnDisable()
    {
        possession.OnPossessEvent.RemoveListener(OnPossess);
        possession.OnUnpossessEvent.RemoveListener(OnUnpossess);
        OnDeathEvent.RemoveListener(OnDeath);
        playersControls.Disable();
    }

    private void Start()
    {
        // Multiple objects could be possessed at the start, thus the last one be
        // called by `Start` will win.
        if (possessAtStart)
        {
            // Reset possession on awake to ensure events are fired as intended.
            possession.Possess(this);
        }
    }

    public override void Die()
    {
        if (IsPossessed || !immortalWhenUnpossesed) {
            base.Die();
        }
    }

    private void OnDeath(Killable killable)
    {
        if (IsPossessed)
        {
            possession.Unpossess();
        }
    }

    public void Possess() {
        possession.Possess(this);
    }

    private void OnPossess(Possessable otherPossessable)
    {
        if (otherPossessable == this && immortalWhenUnpossesed) {
            collider.enabled = true;
        }
    }

    private void OnUnpossess(Possessable otherPossessable)
    {
        if (otherPossessable == this && immortalWhenUnpossesed) {
            collider.enabled = false;
        }
    }

    [Serializable]
    public class PossessableEvent : UnityEvent<Possessable> { }
}
