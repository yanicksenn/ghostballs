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
        + "be possessed at the start. Makes sense to use this on the main player."
        + "Also indicating whether this is the fallback possessable. "
        + "If the host of the current possession dies then the fallback possession "
        + "is being possessed again. The fallback possession cannot die.")]
    private bool possessAtStart;

    [SerializeField, Tooltip("Indicating that this GameObject is being possessed. "
        + "May be null.")]
    private GameObject possessionIndicator;

    [SerializeField, Space]
    private float movementSpeed;

    [SerializeField]
    private Transform projectileSpawnLocation;

    [SerializeField]
    private Projectile projectileTemplate;

    [SerializeField, Space]
    private PossessableEvent onPossessEvent = new();
    public PossessableEvent OnPossessEvent => onPossessEvent;

    [SerializeField]
    private PossessableEvent onUnpossessEvent = new();
    public PossessableEvent OnUnpossessEvent => onUnpossessEvent;

    [SerializeField]
    private PossessableEvent onDeathEvent = new();
    public PossessableEvent OnDeathEvent => onDeathEvent;

    private Camera camera;
    private CharacterController characterController;
    private Animator animator;

    private bool dead;

    public void Possess()
    {
        if (!dead)
        {
            possession.Possess(this);
        }
    }

    public void Die()
    {
        if (IsPossessed && !possessAtStart && !dead)
        {
            dead = true;
            possession.Unpossess();
            OnDeathEvent.Invoke(this);


            // Play death animation here.
        }
    }

    private void Awake()
    {
        camera = FindObjectOfType<Camera>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        possessionIndicator.SetActive(false);

        if (possessAtStart)
        {
            possession.FallbackPossessable = this;
        }
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
            // Reset possession on awake to ensure events are fired as intended.
            Possess();
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
        if (animator != null) {
            animator.SetBool("isWalking", direction.sqrMagnitude >= Mathf.Epsilon);
        }

        // Rotate character towards mouse position.
        var cameraRay = camera.ScreenPointToRay(Input.mousePosition);
        var groundPlane = new Plane(Vector3.up, Vector3.zero);
        if (groundPlane.Raycast(cameraRay, out var rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }

        // Shoot possesion projection towards mouse possition. If no projectile
        // is serialized, then shooting is not possible.
        if (Input.GetMouseButtonDown(0 /* RMB */) && projectileTemplate != null)
        {
            var projectile = Instantiate(projectileTemplate);
            projectile.transform.position = projectileSpawnLocation.transform.position;
            projectile.transform.forward = transform.forward;
            projectile.OnCollide.AddListener((collision) =>
            {
                var possessable = collision.gameObject.GetComponent<Possessable>();
                if (possessable != null)
                {
                    possessable.Possess();
                }
            });
        }
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
