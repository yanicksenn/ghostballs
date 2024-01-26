using UnityEngine;

public class PossessionFollower : MonoBehaviour
{

    [SerializeField]
    private Possession possession;

    [SerializeField]
    private Vector3 offset;

    [SerializeField, Min(1)]
    private float lerpModifier = 5;


    private void LateUpdate()
    {
        if (!possession.IsPossesssing) return;
        transform.position = Vector3.Lerp(
            transform.position,
            possession.CurrentPossession.transform.position + offset,
            Time.deltaTime * lerpModifier);
    }
}