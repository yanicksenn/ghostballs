using UnityEngine;

public class PossessionProjectileShooter : MonoBehaviour {

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
            if (gameObjectHit.TryGetComponent<Possessable>(out var possessable))
            {
                possessable.Possess();
            }
        });
    }

}