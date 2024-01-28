using System.Collections;
using UnityEngine;

public class AxeSwinger : MonoBehaviour {

    [SerializeField]
    private float radiusOfImpact = 1;

    [SerializeField]
    private float delayOfImpact = 1;

    private Animator animator;
    private Axe axe;

    private void Awake() {
        animator = GetComponentInChildren<Animator>();
        axe = GetComponentInChildren<Axe>();
    }

    public void Swing()
    {
        animator.ResetTrigger("Attack");
        animator.SetTrigger("Attack");
        StartCoroutine(PerformHit());
    }

    private IEnumerator PerformHit() {
        axe.enabled = true;
        yield return new WaitForSeconds(delayOfImpact);
        axe.enabled = false;
    }
}