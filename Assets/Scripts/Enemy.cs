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

    [SerializeField]
    private bool doesNotWalk;

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

    private Killable killable;
    private Walker walker;
    private Attacker attacker;

    private void Awake()
    {
        killable = GetComponent<Killable>();
        walker = GetComponent<Walker>();
        attacker = GetComponent<Attacker>();
    }

    private void Update()
    {
        if (killable != null && killable.IsDead)
        {
            return;
        }

        // If this GameObject is possessed, then do not behave like an enemy. :)
        if (possession.IsPossesssing && possession.CurrentPossession.gameObject == gameObject)
        {
            return;
        }

        BehaveLikeAnNPC();
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
                        if (!doesNotWalk)
                        {
                            walker.WalkInDirection(directionNormalized);
                        }
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
                        attacker.Attack();
                        SetState(new Attacking());
                        return;
                    }
                }
                break;
            case Attacking _:
                if (!attacker.IsAttacking())
                {
                    SetState(new Idle());
                }
                break;
            default:
                break;
        }
    }

    private Possessable GetAlivePossessableInSight()
    {
        var hits = Physics.OverlapSphere(transform.position, visionDistance);
        foreach (var hit in hits)
        {
            // Ignore collisions with myself.
            if (hit.GetComponent<Collider>().gameObject == gameObject)
            {
                continue;
            }

            // If GameObject is not possessable, then we currently also don't attack it.
            if (!hit.GetComponent<Collider>().gameObject.TryGetComponent<Possessable>(out var possessable))
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

    private void SetState(State newState)
    {
        previousState = currentState;
        currentState = newState;
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