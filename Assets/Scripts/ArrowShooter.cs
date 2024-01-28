using UnityEngine;

public class ArrowShooter : MonoBehaviour {

    [SerializeField]
    private Transform projectileSpawnLocation;

    [SerializeField]
    private Projectile projectileTemplate;

    public void Shoot() {
        var projectile = Instantiate(projectileTemplate);
        projectile.transform.position = projectileSpawnLocation.transform.position;
        projectile.transform.forward = transform.forward;
        projectile.OnHit.AddListener((gameObjectHit) =>
        {
            if (gameObjectHit.TryGetComponent<Killable>(out var killable))
            {
                killable.Die();
            }
        });
    }

}