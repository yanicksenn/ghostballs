using UnityEngine;

public class TestPossessionManager : MonoBehaviour
{
    [SerializeField]
    private Possession possession;

    [SerializeField]
    private Possessable possessable1;

    [SerializeField]
    private Possessable possessable2;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            possession.Possess(possessable1);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            possession.Possess(possessable2);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            possession.CurrentPossession.Die();
        }
    }
}
