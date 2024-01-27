using UnityEngine;

[CreateAssetMenu(menuName = "Create possession", fileName = "Possession")]
public class Possession : ScriptableObject
{
    public bool IsPossesssing => CurrentPossession != null;

    public Possessable FallbackPossessable { get; set; }

    public Possessable CurrentPossession { get; private set; }

    [SerializeField, Space]
    private Possessable.PossessableEvent onPossessEvent = new();
    public Possessable.PossessableEvent OnPossessEvent => onPossessEvent;

    [SerializeField]
    private Possessable.PossessableEvent onUnpossessEvent = new();
    public Possessable.PossessableEvent OnUnpossessEvent => onUnpossessEvent;

    public void OnEnable()
    {
        CurrentPossession = null;
    }

    public void Possess(Possessable newPossessable)
    {
        if (newPossessable == CurrentPossession) { return; }
        if (newPossessable.IsDead) { return; }

        FireOnUnpossess(CurrentPossession);
        CurrentPossession = newPossessable;
        FireOnPossess(CurrentPossession);
    }

    public void Unpossess()
    {
        Possess(FallbackPossessable);
    }

    private void FireOnPossess(Possessable possessable)
    {
        if (possessable != null)
        {
            OnPossessEvent.Invoke(possessable);
        }
    }

    private void FireOnUnpossess(Possessable possessable)
    {
        if (possessable != null)
        {
            OnUnpossessEvent.Invoke(possessable);
        }
    }
}
