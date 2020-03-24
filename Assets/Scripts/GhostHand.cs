using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHand : MonoBehaviour
{
    private bool colliding = false;
    public bool isColliding { get { return colliding; } }

    [SerializeField]
    SpriteRenderer[] sprites;

    Transform body;                     // This a reference to the body of Gooby. I had the hand childed to the body before but it messed with the physics because it was concidered part of the same rigidbody.

    [SerializeField]
    float minDistance, maxDistance;     // The min and max distance the hand can extend.
    [SerializeField]
    [Range(0, 1)]
    float distance;                     // This is the lerp value between the min and max distance that the arm is currently extended to.
    [SerializeField]
    Vector2 direction;                  // This is the vector direction that the hand it from the body.

    void Awake()
    {
        body = GameObject.FindWithTag("Player").transform;      // Find the body.
        direction = Vector2.up;                                 // We just default to having the arm point straight up at the beginning.
    }

    void FixedUpdate()
    {
        // If the hand is not holding onto anything.
        if (InputHandler.rightTrigger > 0f)
        {
            foreach (SpriteRenderer sr in sprites)
                sr.enabled = true;

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
            while (distance > 0f && hit.collider != null)
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
        else
        {
            foreach (SpriteRenderer sr in sprites)
                sr.enabled = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        colliding = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        colliding = false;
    }
}
