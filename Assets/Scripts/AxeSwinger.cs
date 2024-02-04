using System.Collections;
using UnityEngine;

public class AxeSwinger : MonoBehaviour
{
    [SerializeField]
    private float delayOfImpact = 1.0f;
    [SerializeField]
    private float lethalTime = 0.2f;

    private Animator animator;
    private GameObject axeDamage;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        axeDamage = transform.Find("AxeDamage").gameObject;
    }

    public void Swing()
    {
        StartCoroutine(PerformHit());
    }

    private IEnumerator PerformHit()
    {
        animator.ResetTrigger("Attack");
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(delayOfImpact);
        axeDamage.SetActive(true);
        yield return new WaitForSeconds(lethalTime);
        axeDamage.SetActive(false);
    }
}