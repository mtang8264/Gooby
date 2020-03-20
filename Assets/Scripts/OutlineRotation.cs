using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineRotation : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerMovement movement;

    [SerializeField]
    private float rotation;
    [SerializeField]
    private float velocityMultiplier;

    private float lastFrameRotation;

    void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        movement = GetComponentInParent<PlayerMovement>();
    }

    /* To make the rotation of the outline work the way I wanted I had to do some fun programming.
     * First we are keeping track of how much the parent object (which handles the actual physics) is rotation.
     * The parent only rotates at a good speed when it's on the ground because of the friction.
     * Because of this, we use the parent's rotation while it is on the ground.
     * As it rotates we mark the change from last frame to this frame and change the rotation of the outline accordingly.
     * If we are not on the ground however we just use the current velocity of the RigidBody to determine the speed at which the outline rotates.
     * Because of this we get a nice look where the outline appears to rotate even when the body has to reason to actually be rotating.
     */
    void Update()
    {
        float parentRot = transform.parent.eulerAngles.z;

        if(movement.isGrounded)
        {
            float d = parentRot - lastFrameRotation;
            rotation += d;
        }
        else
        {
            rotation -= rb.velocity.x;
        }

        transform.eulerAngles= new Vector3(0f,0f,rotation);

        lastFrameRotation = transform.parent.eulerAngles.z;
    }
}
