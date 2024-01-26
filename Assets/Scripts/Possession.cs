using UnityEngine;

[CreateAssetMenu(menuName = "Create possession", fileName = "Possession")]
public class Possession : ScriptableObject
{
    public bool IsPossesssing => currentPossession != null;

    [SerializeField]
    private Possessable currentPossession;
    public Possessable CurrentPossession => currentPossession;

    [SerializeField, Space]
    private Possessable.PossessableEvent onPossessEvent = new();
    public Possessable.PossessableEvent OnPossessEvent => onPossessEvent;

    [SerializeField]
    private Possessable.PossessableEvent onUnpossessEvent = new();
    public Possessable.PossessableEvent OnUnpossessEvent => onUnpossessEvent;

    private void Awake() {
        // Reset possession on awake to ensure events are fired as intended.
        currentPossession = null;
    }


    public void Possess(Possessable possessable)
    {
        if (currentPossession == possessable) return;
        FireOnUnpossess(currentPossession);
        currentPossession = possessable;
        FireOnPossess(currentPossession);
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
