using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSwitch : SimpleSwitch
{
    [SerializeField]
    private float timeToFlip = 0.5f;
    private float timer;

    // euler rotation variables
    private float startRotation;
    private float onRotation = 90f;

    new private void Start()
    {
        base.Start();

        startRotation = transform.rotation.eulerAngles.x;
    }

    public override void BaseUpdate()
    {
        // Check if the handle is rotated to the 'on' position
        if (simpleTriggerComp.Activated && Mathf.DeltaAngle(transform.rotation.eulerAngles.x, startRotation + onRotation) > 0.001f)
        {
            // Update timer
            timer += Time.deltaTime;

            LerpHandle(timer);
        }
        else if (!simpleTriggerComp.Activated && Mathf.Abs(Mathf.DeltaAngle(transform.rotation.eulerAngles.x, startRotation)) > 0.001f)
        {
            // Update timer
            timer -= Time.deltaTime;

            LerpHandle(timer);
        }
    }


    public override void Interact(PlayerInteractor player)
    {
        // Do nothing if we're already active
        if (simpleTriggerComp.Activated)
            return;

        // We now know we are inactive at this point
        simpleTriggerComp.Activate();
    }

    private void LerpHandle(float timer)
    {
        // Lerp rotation towards 'on' or 'off' position based on timer
        Vector3 newEuler = transform.rotation.eulerAngles;
        newEuler.x = Mathf.LerpAngle(startRotation, startRotation + onRotation, timer / timeToFlip);

        Quaternion newRotation = Quaternion.Euler(newEuler);

        transform.rotation = newRotation;
    }
}
