using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController))]
public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Possession possession;

    [SerializeField]
    private float visionDistance;

    [SerializeField]
    private float attackDistance;

    [SerializeField, Space]
    private EnemyEvent onEnrage = new();
    public EnemyEvent OnEnrage => onEnrage;

    [SerializeField]
    private EnemyEvent onCharge = new();
    public EnemyEvent OnCharge => onCharge;

    [SerializeField]
    private EnemyEvent onAttackHit = new();
    public EnemyEvent OnAttackHit => onAttackHit;

    [SerializeField]
    private EnemyEvent onAttackMiss = new();
    public EnemyEvent OnAttackMiss => onAttackMiss;

    private State currentState = new Idle();
    private State previousState = new Idle();

    private CharacterController characterController;
    private Animator animator;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // If this GameObject is possessed, then do not behave like an enemy. :)
        if (possession.IsPossesssing && possession.CurrentPossession.gameObject == gameObject)
        {
            return;
        }
        else
        {
            BehaveLikeAnNPC();
        }
    }

    private void BehaveLikeAnNPC()
    {

        var enteredState = previousState.GetType() != currentState.GetType();
        switch (currentState)
        {
            case Idle _:
                {
                    var possessableInSight = GetAlivePossessableInSight();
                    if (possessableInSight != null)
                    {
                        SetState(new Enraged());
                        OnEnrage.Invoke(this);
                        return;
                    }
                }
                break;
            case Enraged _:
                {
                    var possessableInSight = GetAlivePossessableInSight();
                    if (possessableInSight == null)
                    {
                        SetState(new Idle());
                        return;
                    }
                    else
                    {
                        var target = possessableInSight.gameObject.transform.position;
                        var direction = possessableInSight.gameObject.transform.position - transform.position;
                        var directionNormalized = direction.normalized;
                        characterController.Move(Time.deltaTime * 4 /*speed*/ * directionNormalized);
                        transform.LookAt(target);

                        if (IsInAttackDistance(possessableInSight.gameObject.transform))
                        {
                            SetState(new Charge());
                            OnCharge.Invoke(this);
                            return;
                        }
                    }
                }
                break;
            case Charge _:
                {
                    if (enteredState)
                    {
                        Attack();
                        return;
                    }
                }
                break;
            case Attacking _:
                break;
            default:
                break;
        }
    }

    private Possessable GetAlivePossessableInSight()
    {
        var hits = Physics.SphereCastAll(transform.position, visionDistance, transform.forward);
        foreach (var hit in hits)
        {
            // Ignore collisions with myself.
            if (hit.collider.gameObject == gameObject)
            {
                continue;
            }

            // If GameObject is not possessable, then we currently also don't attack it.
            if (!hit.collider.gameObject.TryGetComponent<Possessable>(out var possessable))
            {
                continue;
            }

            // Ignore dead possessables. They're harmless.
            if (possessable.IsDead)
            {
                continue;
            }

            // Ignore unpossessed GameObjects. They're harmless... For now.
            if (!possessable.IsPossessed)
            {
                continue;
            }

            // Technically possible, although not realistic that the ray does not hit anyone although 
            // the possessable is right there.
            var rayDirection = (possessable.transform.position - transform.position).normalized;
            if (!Physics.Raycast(transform.position, rayDirection, out var directHit))
            {
                continue;
            }

            // There might be a wall in between the possessable and the enemy.
            if (directHit.collider.gameObject != possessable.gameObject)
            {
                continue;
            }

            return possessable;
        }

        return null;
    }

    private bool IsInAttackDistance(Transform transformToAttack)
    {
        var sqrDistance = (transformToAttack.position - transform.position).sqrMagnitude;
        return sqrDistance <= Mathf.Pow(attackDistance, 2);
    }

    private IEnumerator PerformAttackAfterCharge(float chargeDuration, Action completionAction)
    {
        yield return new WaitForSeconds(chargeDuration);
        var possessableInSight = GetAlivePossessableInSight();
        if (possessableInSight != null && IsInAttackDistance(possessableInSight.transform))
        {
            Debug.Log("Kill " + possessableInSight.name);
            possessableInSight.Die();

            OnAttackHit.Invoke(this);
        }
        else
        {
            OnAttackMiss.Invoke(this);
        }
        completionAction.Invoke();
    }

    private void SetState(State newState)
    {
        previousState = currentState;
        currentState = newState;
    }

    private void Attack()
    {
        SetState(new Attacking());
        animator.SetTrigger("Attack");
        StartCoroutine(PerformAttackAfterCharge(0.87f, () =>
        {
            SetState(new Idle());
        }));
    }


    private abstract class State { }

    private class Idle : State { }

    private class Enraged : State { }

    private class Charge : State { }

    private class Attacking : State { }

    [Serializable]
    public class EnemyEvent : UnityEvent<Enemy>
    {

    }
}