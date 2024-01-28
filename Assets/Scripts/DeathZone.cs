using UnityEngine;

public class DeathZone : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        var killable = collider.gameObject.GetComponent<Killable>();
        if (killable != null)
        {
            killable.Die();
        }
    }
}
