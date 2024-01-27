using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Possession possession;

    [SerializeField]
    private float visionDistance;

    [SerializeField]
    private float attackDistance;

    private CharacterController characterController;
    private Killable killable;

    private State currentState = new Idle();
    private State previousState = new Idle();

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // If this GameObject is possessed, then do not behave like an enemy. :)
        if (possession.CurrentPossession.gameObject == gameObject) {
            return;
        }

        var enteredState = previousState.GetType() != currentState.GetType();
        switch (currentState)
        {
            case Idle _:
                {
                    var possessableInSight = GetAlivePossessableInSight();
                    if (possessableInSight != null)
                    {
                        SetState(new Enraged());
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
                        var direction = possessableInSight.gameObject.transform.position - transform.position;
                        var directionNormalized = direction.normalized;
                        characterController.Move(Time.deltaTime * 2 /*speed*/ * directionNormalized);

                        if (IsInAttackDistance(possessableInSight.gameObject.transform))
                        {
                            SetState(new Attacking());
                            return;
                        }
                    }
                }
                break;
            case Attacking _:
                {
                    if (enteredState) {
                        StartCoroutine(PerformAttack());
                        return;
                    }
                }
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
            if (possessable.IsDead) {
                continue;
            }

            // Ignore unpossessed GameObjects. They're harmless... For now.
            if (!possessable.IsPossessed)
            {
                continue;
            }

            return possessable;
        }

        return null;
    }

    private bool IsInAttackDistance(Transform transformToAttack) {
        var sqrDistance = (transformToAttack.position - transform.position).sqrMagnitude;
        return sqrDistance <= Mathf.Pow(attackDistance, 2);
    }

    private IEnumerator PerformAttack() {
        yield return new WaitForSeconds(2f);
        SetState(new Enraged());
        
        var possessableInSight = GetAlivePossessableInSight();
        if (possessableInSight != null && IsInAttackDistance(possessableInSight.transform)) {
            possessableInSight.Die();
        }
    }

    private void SetState(State newState) {
        previousState = currentState;
        currentState = newState;
        Debug.Log($"{previousState.GetType().Name} : {currentState.GetType().Name}");
    }


    private abstract class State
    {

    }

    private class Idle : State
    {

    }

    private class Enraged : State
    {

    }

    private class Attacking : State
    {
    }
}