using UnityEngine;

public class Walker : MonoBehaviour
{

    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private bool canMoveDuringAttackCooldown = true;

    private Possessable possessable;
    private CharacterController characterController;
    private Animator animator;
    private PlayersControls playersControls;
    private Attacker attacker; // Can be null
    private Vector3 walk = new Vector3(0, 0, 0);

    public void WalkInDirection(Vector3 direction)
    {
        walk = walk + (movementSpeed * direction);
    }

    private void Awake()
    {
        possessable = GetComponent<Possessable>();
        characterController = GetComponent<CharacterController>();
        attacker = GetComponent<Attacker>();
        animator = GetComponentInChildren<Animator>();
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
        bool attacking = (attacker != null && attacker.IsAttacking());
        bool blockedDueToAttack = !canMoveDuringAttackCooldown && attacking;
        /*if (attacking)
        {
            Debug.Log("is attacking");
        }*/
        if (possessable.IsPossessed && !possessable.IsDead)
        {
            var move = playersControls.Controls.Movement.ReadValue<Vector2>();
            var direction = new Vector3(move.x, 0, move.y).normalized;
            WalkInDirection(direction);
        }

        if (blockedDueToAttack)
        {
            walk = new Vector3(0, 0, 0);
        }
        characterController.SimpleMove(walk);
        if (animator != null)
        {
            animator.SetBool("isWalking", walk.sqrMagnitude >= Mathf.Epsilon);
        }
        walk = new Vector3(0, 0, 0);
    }
}