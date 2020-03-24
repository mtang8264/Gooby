using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField]
    GhostHand ghostHand;

    Transform body;                     // This a reference to the body of Gooby. I had the hand childed to the body before but it messed with the physics because it was concidered part of the same rigidbody.
    DistanceJoint2D distanceJoint;      // This is the joint which controls how far the body can be from the hand when it's grabbing something.

    enum State { FREE, HOLDING };       // This is just a simple enum to remember if we're holding onto something or not.
    [SerializeField]
    State state;

    [SerializeField]
    float minDistance, maxDistance;     // The min and max distance the hand can extend.
    [SerializeField]
    [Range(0,1)]
    float distance;                     // This is the lerp value between the min and max distance that the arm is currently extended to.
    [SerializeField]
    Vector2 direction;                  // This is the vector direction that the hand it from the body.

    [SerializeField]
    float holdDistance;                 // This distance at which the body is from the arm during a hold
    [SerializeField]
    Vector3 holdPosition;               // The position that the hand is during a hold.
    [SerializeField]
    Quaternion holdRotation;            // The rotation that the hand is during a hold.

    [SerializeField]
    float armExtentionSpeed;            // The speed at which the arm can extend and retract.

    bool leftBumperPressed = false;

    void Awake()
    {
        body = GameObject.FindWithTag("Player").transform;      // Find the body.
        direction = Vector2.up;                                 // We just default to having the arm point straight up at the beginning.
        distanceJoint = body.GetComponent<DistanceJoint2D>();   // Find the distance joint attatched to the body.
        distanceJoint.enabled = false;                          // It shouldn't be enabled by default so the body and hand can move freely.
    }

    void FixedUpdate()
    {
        // If the hand is not holding onto anything.
        if (state == State.FREE)
        {
            // If the right stick is being held in any direction we use its normalized value as the direction of the hand.
            if (InputHandler.rightStick != Vector2.zero)
                direction = InputHandler.rightStick.normalized;

            // The distance is determined by how far the player holds the right stick and then is used as a lerp proportion between min and max.
            distance = InputHandler.rightStick.magnitude;
            float actualDistance = Mathf.Lerp(minDistance, maxDistance, distance);
            Debug.Log(actualDistance);

            /* This section ensures that the hand never extend too far into a solid platform.
             * The mask ensures we only collide with platforms.
             * We raycast from the center of the body toward the hand at a distance equal to the distance from the body to the hand.
             */
            LayerMask mask = LayerMask.GetMask("Platform");
            RaycastHit2D hit = Physics2D.Raycast(body.position, direction, actualDistance, mask);
            while(distance > 0f && hit.collider != null)
            {
                // If we collided with a platform, we shorten the distance of the hand
                // and check again until we are either not colliding or we have reached the min distance.
                distance -= 0.01f;
                actualDistance = Mathf.Lerp(minDistance, maxDistance, distance);
                hit = Physics2D.Raycast(body.position, direction, actualDistance, mask);
            }

            // We position the hand by placing it relative to the body at a distance and direction based on our calculations.
            transform.position = body.position + (Vector3)(direction * actualDistance);

            // The angle of the hand it figured out by getting the signed angle equal to direction the hand is extended
            // relative to the up direction because 0 degrees rotation is pointing up.
            float angle = Vector2.SignedAngle(Vector2.up, direction);
            Vector3 euler = new Vector3(0, 0, angle);
            transform.eulerAngles = euler;
        }
        // If the hand is holding on to something.
        else if(state == State.HOLDING)
        {
            if(InputHandler.leftBumper && !leftBumperPressed)
            {
                leftBumperPressed = true;
                holdPosition = ghostHand.transform.position;
                holdRotation = ghostHand.transform.rotation;
                holdDistance = Vector3.Distance(body.position, ghostHand.transform.position);
                distanceJoint.distance = holdDistance;
            }
            else if(InputHandler.leftBumper == false)
            {
                leftBumperPressed = false;
            }

            distanceJoint.connectedAnchor = transform.position;

            // We just position and rotate the hand like it was at the beginning of the hold action.
            transform.position = holdPosition;
            transform.rotation = holdRotation;

            // The triangle button causes the arm to retract.
            if(InputHandler.triangle)
            {
                holdDistance -= armExtentionSpeed * Time.deltaTime;
                if (holdDistance < minDistance)
                    holdDistance = minDistance;
            }
            // The circle buttons causes the arm to extend.
            if(InputHandler.circle)
            {
                holdDistance += armExtentionSpeed * Time.deltaTime;
                if (holdDistance > maxDistance)
                    holdDistance = maxDistance;
            }
            // I didn't use an else if statement because I didn't want either action to override the other.
            // Having it as two seperate if statements meens that the player can hold both bumpers and the hand will remain at the same distance.

            // We update the distance joint with the new distance.
            distanceJoint.distance = holdDistance;

            // If the right trigger is at least half released we let go.
            if(InputHandler.rightTrigger < 0f)
            {
                distanceJoint.enabled = false;
                state = State.FREE;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // If the hand it colliding with something and you start holding down the right trigger we start holding.
        if(state == State.FREE && InputHandler.rightTrigger > 0f)
        {
            //Debug.Log(transform.position);
            holdPosition = transform.position;                                      // We record the position at the beginning of the hold.
            holdRotation = transform.rotation;                                      // Also the rotation.
            holdDistance = Vector3.Distance(transform.position, body.position);     // And the distance.
            distanceJoint.enabled = true;                                           // We enabled the distance joint on the body so it can't move beyond it's bounds.
            distanceJoint.connectedAnchor = transform.position;                     // We position the connected anchor to where the hand is.
            distanceJoint.distance = holdDistance;                                  // We set the distance of the joint manually.
            state = State.HOLDING;                                                  // We change the state.
            /*
             * I had tried to reference the hand as the connected anchor but it was giving me a weird effect
             * where the body could eventually move beyond its limit. I think it had to do with the recursive
             * nature of positioning the hand relative to the hand and the hand relative to the body. This may
             * not be the most efficient way to position the two but it is a way which produces nearly the exact
             * effect I desire.
             */
        }
    }
}
