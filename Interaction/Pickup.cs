using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : Interactable
{

    [SerializeField]
    private PickupSO pickupObject;
    public PickupSO PickupObject
    {
        get { return pickupObject; }
        set { pickupObject = value; }
    }

    // Start position and rotation (and scale)
    private Vector3 startPos;
    private Quaternion startRot;


    private void Start()
    {
        startPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        startRot = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
    }

    public override void Interact(PlayerInteractor playerInteractor)
    {
        bool successfulInteract = playerInteractor.InteractWithPickup(this);

        if (successfulInteract)
        {
            gameObject.SetActive(false);
        }
    }

    public void ResetTransform()
    {
        transform.rotation = startRot;
        transform.position = startPos;
    }

}
