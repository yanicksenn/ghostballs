using UnityEngine;

public class Deflator : MonoBehaviour
{
    [SerializeField]
    private Possession possession;

    private Possessable possessable;
    private Animator animator;

    private void Awake()
    {
        possessable = GetComponent<Possessable>();
        animator = GetComponentInChildren<Animator>();
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

    private void OnUnpossess(Possessable otherPossessable)
    {
        if (possessable == otherPossessable)
        {
            animator.ResetTrigger("Inflate");
            animator.SetTrigger("Deflate");
        }
    }

    private void OnPossess(Possessable otherPossessable)
    {
        if (possessable == otherPossessable){
            animator.ResetTrigger("Deflate");
            animator.SetTrigger("Inflate");
        }
    }
}