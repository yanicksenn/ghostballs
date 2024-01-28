using System.Collections;
using UnityEngine;

public class Inflammable : MonoBehaviour
{

    [SerializeField, Tooltip("Indicates whether this object is currently burning.")]
    private bool isBurning = false;

    [SerializeField, Tooltip("Indicates how long this object can stay on fire before it is killed. "
        + "Set to -1 to never die from fire.")]
    private float timeToKill = -1;

    [SerializeField, Tooltip("Indicates how long this object will keep burning after it is killed. "
        + "Set to -1 to keep burning indefinitely.")]
    private float timeToStopBurningWhenKilled = 0;

    private Coroutine runningKillCoroutine = null;
    private Coroutine runningStopBurningCoroutine = null;

    private Killable killable;

    [SerializeField]
    private GameObject firePrefab;

    private ParticleSystem fireParticles;

    private Animator fireLightAnimator;

    // Start is called before the first frame update
    void Start()
    {
        GameObject fireParticlesObject = Instantiate(firePrefab, transform) as GameObject;
        fireParticles = fireParticlesObject.GetComponent<ParticleSystem>();
        var objectSize = GetComponent<Renderer>().bounds.size;
        var objectScale = transform.localScale;
        Debug.Log("Object size: " + objectSize.x + "," + objectSize.y + "," + objectSize.z);
        fireParticlesObject.transform.localScale = new Vector3(
                3 * objectSize.x / objectScale.x,
                3 * objectSize.y / objectScale.y,
                3 * objectSize.z / objectScale.z
            );
        fireParticlesObject.transform.Translate(new Vector3(0, objectSize.y / 3, 0), Space.World);
        fireLightAnimator = fireParticlesObject.transform.GetChild(0).gameObject.GetComponent<Animator>();

        if (isBurning)
        {
            PlayFire();
        }
        else
        {
            StopFire();
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
            PlayFire();
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
            StopFire();
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

    private void PlayFire()
    {
        fireParticles.Play();
        fireLightAnimator.SetBool("isOn", true);
    }

    private void StopFire()
    {
        fireParticles.Stop();
        fireLightAnimator.SetBool("isOn", false);
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
        StopFire();
    }
}
