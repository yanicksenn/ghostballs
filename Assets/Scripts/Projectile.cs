using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField]
    private HitEvent onHit = new();
    public HitEvent OnHit => onHit;

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
        OnHit.RemoveAllListeners();
    }

    void Update()
    {
        if (hit) return;

        transform.position += Time.deltaTime * movementSpeed * transform.forward;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent<Projectile>(out var _)) {
            return;
        }
        if (collider.gameObject.TryGetComponent<FencePiece>(out var _)) {
            return;
        }

        if (hit) { return; }
        hit = true;

        OnHit.Invoke(collider.gameObject);
        if (remainInPlaceWhenHit) {
            transform.parent = collider.gameObject.transform;
        } else {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hit) { return; }
        hit = true;

        OnHit.Invoke(collision.collider.gameObject);
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
