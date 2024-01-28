
using UnityEngine;

public class RemovableFence : ButtonReceiver
{

    private float animationTimeLeft = 0.6f;
    private bool isTriggered = false;

    void Update() {
        if (animationTimeLeft > 0 && isTriggered) {
            transform.Translate(Vector3.down * Time.deltaTime * 4);
            animationTimeLeft -= Time.deltaTime;
        }
    }

    public override void TriggerButtonEffect()
    {
        isTriggered = true;
    }
}
