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

    [SerializeField]
    private bool remainInPlaceWhenHit;

    private bool hit;

    private void Awake()
    {
        StartCoroutine(DestroyWhenTtlIsReached());
    }

    private void OnDisable() {
        OnCollide.RemoveAllListeners();
    }

    void Update()
    {
        if (hit) return;

        transform.position += Time.deltaTime * movementSpeed * transform.forward;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hit) { return; }
        hit = true;

        OnCollide.Invoke(collision);
        if (remainInPlaceWhenHit) {
            transform.parent = collision.collider.gameObject.transform;
        } else {
            Destroy(gameObject);
        }
    }

    private IEnumerator DestroyWhenTtlIsReached()
    {
        yield return new WaitForSeconds(ttl);
        if (!hit || !remainInPlaceWhenHit) {
            Destroy(gameObject);
        }
    }
}
