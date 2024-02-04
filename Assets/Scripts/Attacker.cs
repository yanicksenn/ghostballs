using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Attacker : MonoBehaviour
{
    [SerializeField]
    private float attackCoolDown = 0.0f;
    // Time for the attack action to be executed. Will block further aiming.
    [SerializeField]
    private float attackTime = 0.0f;

    [SerializeField]
    private UnityEvent onAttack = new();
    public UnityEvent OnAttack => onAttack;

    private new Camera camera;
    private Possessable possessable;
    private PlayersControls playersControls;
    private float currentAttackCoolDown = 0.0f;
    private float currentAttackTime = 0.0f;

    public void Attack()
    {
        if (currentAttackCoolDown <= 0.0)
        {
            currentAttackCoolDown = attackCoolDown;
            currentAttackTime = attackTime;
            onAttack.Invoke();
        }
    }

    public bool IsAttacking()
    {
        return currentAttackTime >= 0.0;
    }

    private void Awake()
    {
        camera = FindObjectOfType<Camera>();
        possessable = GetComponent<Possessable>();
        playersControls = new PlayersControls();
    }

    private void OnEnable()
    {
        playersControls.Enable();
    }

    private void OnDisable()
    {
        playersControls.Disable();
    }

    private void Update()
    {
        if (possessable.IsDead) return;
        if (possessable.IsPossessed)
        {
            PossessedBehaviour();
        }
        currentAttackCoolDown -= Time.deltaTime;
        currentAttackTime -= Time.deltaTime;
    }

    private void PossessedBehaviour()
    {
        // Don't allow turning during attack.
        if (IsAttacking()) return;

        // TODO: if you have a gamepad connected, the mouse scheme won't work for now.
        // Find a way to correctly switch devices. See maybe https://www.youtube.com/watch?v=koRgU2dC5Po&t=988s
        var move = playersControls.Controls.Movement.ReadValue<Vector2>();
        var direction = new Vector3(move.x, 0, move.y).normalized;
        if (Gamepad.current != null)
        {
            var aim = playersControls.Controls.Aim.ReadValue<Vector2>();
            if (!aim.Equals(Vector2.zero))
            {
                transform.forward = new Vector3(aim.x, 0, aim.y);
            }
            else if (!direction.Equals(Vector3.zero))
            {
                transform.forward = direction;
            }
        }
        else
        {
            var cameraRay = camera.ScreenPointToRay(Input.mousePosition);
            var groundPlane = new Plane(Vector3.up, new Vector3(0, 2, 0));
            if (groundPlane.Raycast(cameraRay, out var rayLength))
            {
                Vector3 pointToLook = cameraRay.GetPoint(rayLength);
                transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
            }
            // TODO: implement facing the walking direction if you don't move the mouse
        }

        // Shoot possesion projection towards mouse possition. If no projectile
        // is serialized, then shooting is not possible.
        var shootButtonPressed = playersControls.Controls.Shoot.ReadValue<float>() == 1.0;
        if (shootButtonPressed)
        {
            Attack();
        }
    }
}