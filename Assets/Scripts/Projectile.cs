using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField]
    private CollisionEvent onCollide = new();
    public CollisionEvent OnCollide => onCollide;

    [SerializeField]
    private float movementSpeed;

    [SerializeField, Tooltip("Indicates the time to live in seconds until the projectile is destroyed.")]
    private float ttl = 3;

    private void Awake()
    {
        StartCoroutine(DestroyWhenTtlIsReached());
    }

    private void OnDisable() {
        OnCollide.RemoveAllListeners();
    }

    void Update()
    {
        transform.position += Time.deltaTime * movementSpeed * transform.forward;
    }

    void OnCollisionEnter(Collision collision)
    {
        OnCollide.Invoke(collision);
        Destroy(gameObject);
    }

    private IEnumerator DestroyWhenTtlIsReached()
    {
        yield return new WaitForSeconds(ttl);
        Destroy(gameObject);
    }
}
