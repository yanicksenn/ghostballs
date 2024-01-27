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

    private PlayersControls playersControls;

    private void Awake()
    {
        playersControls = new PlayersControls();
        if (possessAtStart)
        {
            possession.FallbackPossessable = this;
        }
    }

    private void OnEnable()
    {
        OnDeathEvent.AddListener(OnDeath);
        playersControls.Enable();
    }

    private void OnDisable()
    {
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

    [Serializable]
    public class PossessableEvent : UnityEvent<Possessable> { }
}
