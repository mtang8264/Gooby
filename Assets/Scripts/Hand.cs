using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    Transform body;
    DistanceJoint2D distanceJoint;

    enum State { FREE, HOLDING };
    [SerializeField]
    State state;

    [SerializeField]
    float minDistance, maxDistance;
    [SerializeField]
    [Range(0,1)]
    float distance;
    [SerializeField]
    Vector2 direction;

    [SerializeField]
    float holdDistance;
    [SerializeField]
    Vector3 holdPosition;
    [SerializeField]
    Quaternion holdRotation;

    [SerializeField]
    float armExtentionSpeed;

    void Awake()
    {
        body = GameObject.FindWithTag("Player").transform;
        direction = Vector2.up;
        distanceJoint = body.GetComponent<DistanceJoint2D>();
        distanceJoint.enabled = false;
    }

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (state == State.FREE)
        {
            if (InputHandler.rightStick != Vector2.zero)
                direction = InputHandler.rightStick.normalized;

            distance = InputHandler.rightStick.magnitude;
            float actualDistance = Mathf.Lerp(minDistance, maxDistance, distance);

            LayerMask mask = LayerMask.GetMask("Platform");
            RaycastHit2D hit = Physics2D.Raycast(body.position, direction, actualDistance, mask);
            while(distance > 0f && hit.collider != null)
            {
                distance -= 0.01f;
                actualDistance = Mathf.Lerp(minDistance, maxDistance, distance);
                hit = Physics2D.Raycast(body.position, direction, actualDistance, mask);
            }

            transform.position = body.position + (Vector3)(direction * actualDistance);

            float angle = Vector2.SignedAngle(Vector2.up, direction);
            Vector3 euler = new Vector3(0, 0, angle);
            transform.eulerAngles = euler;
        }
        else if(state == State.HOLDING)
        {
            transform.position = holdPosition;
            transform.rotation = holdRotation;
            if(InputHandler.rightBumper)
            {
                holdDistance -= armExtentionSpeed * Time.deltaTime;
                if (holdDistance < minDistance)
                    holdDistance = minDistance;
            }
            if(InputHandler.leftBumper)
            {
                holdDistance += armExtentionSpeed * Time.deltaTime;
                if (holdDistance > maxDistance)
                    holdDistance = maxDistance;
            }

            distanceJoint.distance = holdDistance;

            if(InputHandler.rightTrigger < 0f)
            {
                distanceJoint.enabled = false;
                state = State.FREE;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(state == State.FREE && InputHandler.rightTrigger > 0f)
        {
            Debug.Log(transform.position);
            holdPosition = transform.position;
            holdRotation = transform.rotation;
            holdDistance = Vector3.Distance(transform.position, body.position);
            distanceJoint.enabled = true;
            distanceJoint.connectedAnchor = transform.position;
            distanceJoint.distance = holdDistance;
            state = State.HOLDING;
        }
    }
}
