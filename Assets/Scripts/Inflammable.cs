using System.Collections;
using UnityEngine;

public class Inflammable : MonoBehaviour
{

    [SerializeField, Tooltip("Indicates whether this object is currently burning.")]
    private bool isBurning;

    [SerializeField, Tooltip("Indicates how long this object can stay on fire before it is killed. "
        + "Set to -1 to never die from fire.")]
    private float timeToKill;

    [SerializeField, Tooltip("Indicates how long this object will keep burning after it is killed. "
        + "Set to -1 to keep burning indefinitely.")]
    private float timeToStopBurningWhenKilled;

    private Coroutine runningKillCoroutine = null;
    private Coroutine runningStopBurningCoroutine = null;

    private Killable killable;

    [SerializeField]
    private GameObject firePrefab;

    private ParticleSystem firePaticles;

    // Start is called before the first frame update
    void Start()
    {
        GameObject firePaticlesObject = Instantiate(firePrefab, transform) as GameObject;
        firePaticles = firePaticlesObject.GetComponent<ParticleSystem>();
        firePaticlesObject.transform.parent = transform;
        if (isBurning) {
            firePaticles.Play();
        } else {
            firePaticles.Stop();
        }
    }

    void Awake()
    {
        killable = GetComponent<Killable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isBurning && killable != null && killable.IsDead && runningStopBurningCoroutine == null)
        {
            runningStopBurningCoroutine = StartCoroutine(StopBurningWhenTimeToStopBurningIsReached());
        }
    }

    void OnCollisionEnter(Collision collider)
    {
        Debug.Log("Test");
        Inflammable otherInflammable = collider.gameObject.GetComponent<Inflammable>();
        if (otherInflammable != null && otherInflammable.isBurning)
        {
            StartFire();
        }
    }

    void StartFire()
    {
        if (!isBurning && (killable == null || !killable.IsDead))
        {
            isBurning = true;
            firePaticles.Play();
            if (timeToKill != -1f && killable != null && runningKillCoroutine == null)
            {
                runningKillCoroutine = StartCoroutine(KillWhenTimeToKillIsReached(killable));
            }
        }
    }

    void ExtinguishFire()
    {
        if (isBurning)
        {
            isBurning = false;
            firePaticles.Stop();
            if (runningKillCoroutine != null)
            {
                StopCoroutine(runningKillCoroutine);
                runningKillCoroutine = null;
            }
            if (runningStopBurningCoroutine != null)
            {
                StopCoroutine(runningStopBurningCoroutine);
                runningStopBurningCoroutine = null;
            }
        }
    }

    private IEnumerator KillWhenTimeToKillIsReached(Killable killable)
    {
        yield return new WaitForSeconds(timeToKill);
        killable.Die();
    }

    private IEnumerator StopBurningWhenTimeToStopBurningIsReached()
    {
        yield return new WaitForSeconds(timeToStopBurningWhenKilled);
        isBurning = false;
        firePaticles.Stop();
    }
}
