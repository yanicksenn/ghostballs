using UnityEngine;

public class ArrowShooter : MonoBehaviour {

    [SerializeField]
    private Transform projectileSpawnLocation;

    [SerializeField]
    private Projectile projectileTemplate;

    public void Shoot() {
        Debug.Log("Shoot (ArrowShooter)");
        var projectile = Instantiate(projectileTemplate);
        projectile.transform.position = projectileSpawnLocation.transform.position;
        projectile.transform.forward = transform.forward;
        projectile.OnCollide.AddListener((collision) =>
        {
            if (collision.gameObject.TryGetComponent<Killable>(out var killable))
            {
                killable.Die();
            }
        });
    }

}