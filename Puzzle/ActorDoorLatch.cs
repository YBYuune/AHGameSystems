using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActorDoorLatch : SimpleActor
{
    [SerializeField]
    private GameObject latchObject;

    private Vector3 startPosition;

    [SerializeField]
    private Vector3 endPosition;

    [SerializeField]
    private float timeToUnlock = 1f;
    private float timer = 0f;

    public bool startedUnlock = false;
    public bool startedLock = false;

    // Use this for initialization
    private void Start ()
    {
        if (latchObject == null)
            Debug.LogError("Please set up the object to be moved when the actor activates");
        else
        {
            startPosition = latchObject.transform.localPosition;
        }

        // if we started activated
        if (Activated)
        {
            startedUnlock = true;
            startedLock = false;
        }
    }
	
	// Update is called once per frame
	public override void BaseUpdate ()
    {
        base.BaseUpdate();

        // Should we start unlocking the latch?
        if (Activated && !startedUnlock && !startedLock)
        {
            startedUnlock = true;
            startedLock = false;
        }
        else if (!Activated && !startedLock && !startedUnlock)
        {
            startedUnlock = false;
            startedLock = true;
        }

        // If the actor is active and the latch hasn't fully opened yet...
        if (startedUnlock || startedLock)
        {
            Vector3 end = startedUnlock ? endPosition : startPosition;

            if ((latchObject.transform.localPosition - end).sqrMagnitude > 0.001)
            {
                if (startedUnlock)
                    timer += Time.deltaTime;
                else
                    timer -= Time.deltaTime;

                latchObject.transform.localPosition = Vector3.Lerp(startPosition, endPosition, timer / timeToUnlock);
            }

            else if (startedUnlock)
            {
                startedUnlock = false;
            }
            else if (startedLock)
            {
                startedLock = false;
            }
        }
	}
}
