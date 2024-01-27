using UnityEngine;

[CreateAssetMenu(menuName = "Create possession", fileName = "Possession")]
public class Possession : ScriptableObject
{
    public bool IsPossesssing => currentPossession != null;

    public Possessable FallbackPossessable {get;set;}

    [SerializeField]
    private Possessable currentPossession;
    public Possessable CurrentPossession => currentPossession;

    [SerializeField, Space]
    private Possessable.PossessableEvent onPossessEvent = new();
    public Possessable.PossessableEvent OnPossessEvent => onPossessEvent;

    [SerializeField]
    private Possessable.PossessableEvent onUnpossessEvent = new();
    public Possessable.PossessableEvent OnUnpossessEvent => onUnpossessEvent;

    [SerializeField]
    private Possessable.PossessableEvent onDeathEvent = new();
    public Possessable.PossessableEvent OnDeathEvent => onDeathEvent;

    public void Possess(Possessable newPossessable)
    {
        if (newPossessable == currentPossession) {return;}
        if (newPossessable.IsDead) { return; }
    
        FireOnUnpossess(currentPossession);
        currentPossession = newPossessable;
        FireOnPossess(currentPossession);
    }

    public void Unpossess() {
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
