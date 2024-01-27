using UnityEngine;

public class PossesionIndicatorMover : MonoBehaviour
{
    [SerializeField]
    private Possession possession;

    [SerializeField]
    private Vector3 offset;

    private void LateUpdate()
    {
        if (!possession.IsPossesssing) return;
        transform.position = possession.CurrentPossession.transform.position + offset;
    }
}