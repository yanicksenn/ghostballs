using System.Collections;
using UnityEngine;

public class AxeSwinger : MonoBehaviour {

    [SerializeField]
    private float radiusOfImpact = 1;

    [SerializeField]
    private float delayOfImpact = 1;

    private Animator animator;

    private void Awake() {
        animator = GetComponentInChildren<Animator>();
    }

    public void Swing()
    {
        animator.ResetTrigger("Attack");
        animator.SetTrigger("Attack");
        StartCoroutine(PerformHit());
    }

    private IEnumerator PerformHit() {
        yield return new WaitForSeconds(delayOfImpact);
        
        var hits = Physics.SphereCastAll(transform.position + (transform.forward * 2), radiusOfImpact, transform.forward);
        foreach (var hit in hits) {
            if (hit.collider.gameObject == gameObject) {
                continue;
            }

            if (!hit.collider.gameObject.TryGetComponent<Killable>(out var killable)) {
                continue;
            }

            killable.Die();
        }
    }
}