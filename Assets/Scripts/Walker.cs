using UnityEngine;

public class Walker : MonoBehaviour
{

    [SerializeField]
    private float movementSpeed;

    private Possessable possessable;
    private CharacterController characterController;
    private Animator animator;
    private PlayersControls playersControls;

    private bool hasMovedThisFrame = false;

    public void WalkInDirection(Vector3 direction)
    {
        hasMovedThisFrame = true;
        characterController.SimpleMove(movementSpeed * direction);
        if (animator != null)
        {
            animator.SetBool("isWalking", direction.sqrMagnitude >= Mathf.Epsilon);
        }
    }

    private void Awake()
    {
        possessable = GetComponent<Possessable>();
        characterController = GetComponent<CharacterController>();
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
        if (possessable.IsPossessed && !possessable.IsDead)
        {
            var move = playersControls.Controls.Movement.ReadValue<Vector2>();
            var direction = new Vector3(move.x, 0, move.y).normalized;
            WalkInDirection(direction);
        }
        if (!hasMovedThisFrame)
        {
            WalkInDirection(new Vector3(0, 0, 0));
        }
        hasMovedThisFrame = false;
    }
}