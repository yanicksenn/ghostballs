using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Possessable : Killable
{
    [SerializeField]
    private Possession possession;
    public bool IsPossessed => possession.CurrentPossession == this;

    [SerializeField]
    private bool possessAtStart;

    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private Transform projectileSpawnLocation;

    [SerializeField]
    private Projectile projectileTemplate;

    private new Camera camera;
    private CharacterController characterController;
    private Animator animator;
    private PlayersControls playersControls;
    private float attackCoolDown = 0.0f;

    private void Awake()
    {
        camera = FindObjectOfType<Camera>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
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

    private void Update()
    {
        if (IsDead) return;
        if (!IsPossessed) return;

        // Currently, a possessable controls whether you can move via anywhere. 
        // Maybe we need to change this at some point if we want unpossessed 
        // characters to be able to move freely around.
        var move = playersControls.Controls.Movement.ReadValue<Vector2>();
        var direction = new Vector3(move.x, 0, move.y).normalized;
        characterController.SimpleMove(movementSpeed * direction);
        if (animator != null)
        {
            animator.SetBool("isWalking", direction.sqrMagnitude >= Mathf.Epsilon);
        }

        // TODO: if you have a gamepad connected, the mouse scheme won't work for now.
        // Find a way to correctly switch devices. See maybe https://www.youtube.com/watch?v=koRgU2dC5Po&t=988s
        if (Gamepad.current != null)
        {
            var aim = playersControls.Controls.Aim.ReadValue<Vector2>();
            if (!aim.Equals(Vector2.zero))
            {
                transform.forward = new Vector3(aim.x, 0, aim.y);
            }
        }
        else
        {
            var cameraRay = camera.ScreenPointToRay(Input.mousePosition);
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            if (groundPlane.Raycast(cameraRay, out var rayLength))
            {
                Vector3 pointToLook = cameraRay.GetPoint(rayLength);
                transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
            }
        }


        // Shoot possesion projection towards mouse possition. If no projectile
        // is serialized, then shooting is not possible.
        var shootButtonPressed = playersControls.Controls.Shoot.ReadValue<float>() == 1.0;
        if (shootButtonPressed && attackCoolDown <= 0.0 && projectileTemplate != null)
        {
            attackCoolDown = 0.5f;
            var projectile = Instantiate(projectileTemplate);
            projectile.transform.position = projectileSpawnLocation.transform.position;
            projectile.transform.forward = transform.forward;
            //projectile.transform.forward = new Vector3(lookX, 0, lookZ);
            projectile.OnCollide.AddListener((collision) =>
            {
                var possessable = collision.gameObject.GetComponent<Possessable>();
                if (possessable != null)
                {
                    possession.Possess(this);
                }
            });
        }
        attackCoolDown -= Time.deltaTime;
    }

    private void OnDeath(Killable killable)
    {
        if (IsPossessed)
        {
            possession.Unpossess();
        }
    }

    [Serializable]
    public class PossessableEvent : UnityEvent<Possessable> { }
}
