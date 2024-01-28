using UnityEngine;

public class ArrowShooter : MonoBehaviour {

    [SerializeField]
    private Transform projectileSpawnLocation;

    [SerializeField]
    private Projectile projectileTemplate;

    AudioManager audioManager;
    public void Shoot() {
        audioManager =GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        audioManager.PlaySFX(audioManager.arrow);
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