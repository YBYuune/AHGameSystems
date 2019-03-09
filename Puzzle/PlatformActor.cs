using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlatformActor : SimpleActor
{
    [SerializeField]
    private float moveHeight = 5f;
    private float initialHeight;

    [SerializeField]
    private float initialMoveTime = 3f;

    [SerializeField]
    private float waitTime = 5f;

    [SerializeField]
    private float returnMoveTime = 0.5f;

    private float timer = 0f;


    public void Start()
    {
        initialHeight = transform.position.y;
    }

    public override void BaseUpdate()
    {
        // Update active state
        base.BaseUpdate();

        UpdatePosition();
    }

    private void UpdatePosition()
    {
        // Do nothing if we're not activated
        if (!Activated)
            return;

        // update timer
        timer += Time.deltaTime;

        Vector3 newPosition = transform.position;
        if (timer < initialMoveTime)
        {
            // Move down
            newPosition.y = Mathf.Lerp(initialHeight, initialHeight - moveHeight, timer / initialMoveTime);
        }
        else if (timer < initialMoveTime + waitTime)
        {
            // Do nothing here
        }
        else if (timer < initialMoveTime + waitTime + returnMoveTime)
        {
            // Move up
            newPosition.y = Mathf.Lerp(initialHeight - moveHeight, initialHeight, (timer - initialMoveTime - waitTime) / returnMoveTime);
        }
        else
        {
            // Deactivate triggers
            foreach (Activatable activatable in activatableList)
            {
                SimpleTrigger trigger = activatable as SimpleTrigger;

                if (trigger != null)
                {
                    trigger.Deactivate();
                }
            }

            // Reset timer
            timer = 0f;

            // Deactivate self
            Activated = false;

            // Just hard set the height back to the default
            newPosition.y = initialHeight;
        }


        transform.position = newPosition;
    }

}
