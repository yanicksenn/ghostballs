using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUnpossessHint : ButtonReceiver
{
    private Killable killable;
    private Canvas canvas;

    private bool isTriggered = false;

    private void Awake()
    {
        killable = GetComponentInParent<Killable>();
        canvas = GetComponent<Canvas>();
    }

    public override void TriggerButtonEffect()
    {
        if (isTriggered)
        {
            return;
        }
        isTriggered = true;
        StartCoroutine(ShowHint());
    }

    private IEnumerator ShowHint()
    {
        yield return new WaitForSeconds(7f);
        if (canvas != null && killable != null && !killable.IsDead)
        {
            canvas.enabled = true;
        }
    }

}
