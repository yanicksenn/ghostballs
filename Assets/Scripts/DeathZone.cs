using UnityEngine;

public class DeathZone : MonoBehaviour
{   
    AudioManager audioManager;
    void OnTriggerEnter(Collider collider)
    {
        var killable = collider.gameObject.GetComponent<Killable>();
        if (killable != null)
        {
            audioManager =GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
            audioManager.PlaySFX(audioManager.falling);
            killable.Die();
        }
    }
}
