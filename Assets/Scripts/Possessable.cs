using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController))]
public class Possessable : MonoBehaviour
{
    [SerializeField]
    private Possession possession;
    public bool IsPossessed => possession.CurrentPossession == this;

    [SerializeField, Tooltip("Indicating that this GameObject should immediately "
        + "be possessed at the start. Makes sense to use this on the main player.")]
    private bool possessAtStart;

    [SerializeField, Tooltip("Indicating that this GameObject is being possessed. "
        + "May be null.")]
    private GameObject possessionIndicator;

    [SerializeField, Space]
    private PossessableEvent onPossessEvent = new();
    public PossessableEvent OnPossessEvent => onPossessEvent;

    [SerializeField]
    private PossessableEvent onUnpossessEvent = new();
    public PossessableEvent OnUnpossessEvent => onUnpossessEvent;

    [SerializeField, Space]
    private float movementSpeed;

    private CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        possessionIndicator.SetActive(false);
    }

    private void OnEnable()
    {
        possession.OnPossessEvent.AddListener(OnPossess);
        possession.OnUnpossessEvent.AddListener(OnUnpossess);
    }

    private void OnDisable()
    {
        possession.OnPossessEvent.RemoveListener(OnPossess);
        possession.OnUnpossessEvent.RemoveListener(OnUnpossess);
    }

    private void Start()
    {
        // Multiple objects could be possessed at the start, thus the last one be
        // called by `Start` will win.
        if (possessAtStart)
        {
            possession.Possess(this);
        }
    }
    private void Update()
    {
        if (!IsPossessed) return;

        // Currently, a possessable controls whether you can move via anywhere. 
        // Maybe we need to change this at some point if we want unpossessed 
        // characters to be able to move freely around.
        var x = Input.GetAxisRaw("Horizontal");
        var z = Input.GetAxisRaw("Vertical");
        var direction = new Vector3(x, 0, z).normalized;
        characterController.Move(Time.deltaTime * movementSpeed * direction);
    }

    private void OnPossess(Possessable possessable)
    {
        if (possessable == this)
        {
            OnPossessEvent.Invoke(this);
            OnPossessThis();
        }
    }

    private void OnPossessThis()
    {
        if (possessionIndicator == null) return;
        possessionIndicator.SetActive(true);
    }

    private void OnUnpossess(Possessable possessable)
    {
        if (possessable == this)
        {
            OnUnpossessEvent.Invoke(this);
            OnUnpossessThis();
        }
    }

    private void OnUnpossessThis()
    {
        if (possessionIndicator == null) return;
        possessionIndicator.SetActive(false);
    }

    [Serializable]
    public class PossessableEvent : UnityEvent<Possessable>
    {

    }
}
