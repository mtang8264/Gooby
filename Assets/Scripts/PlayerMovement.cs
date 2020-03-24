using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    float speed = 1f;                                       // The max speed the player can move horizontally.
    [SerializeField]
    float jumpForce;                                        // The force applied to cause the player to jump
    private bool jumpFlag;                                  // This is just a flag so that I can check if the player has pressed the jump button but apply the force in FixedUpdate.
    [SerializeField]
    private bool grounded;                                  // A read of wether or not the player is grounded.
    public bool isGrounded { get { return grounded; } }     // This getter allows other classes to see wether the player is grounded but not have access to set it.

    void Awake()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        // The X button calls a jump during the next fixed update.
        if(Input.GetButtonDown("X"))
        {
            jumpFlag = true;
        }
    }

    private void FixedUpdate()
    {
        // The left stick is used to figure out how fast the player should move.
        Vector2 movement = new Vector2(InputHandler.leftStick.x, 0f);
        movement *= speed;
        rb.AddForce(movement, ForceMode2D.Force);

        // Check if the player is grounded.
        grounded = GroundCheck();

        // If the jump flag was set
        if (jumpFlag)
        {
            // Turn of the jump flag
            jumpFlag = false;
            // If the body is grounded it applies an impulse causing the player to jump.
            if(grounded)
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    /*
     * To check if the player is grounded we raycast down from the body of the player
     * starting just below its collider for a short distance.
     */
    bool GroundCheck()
    {
        Ray2D ray = new Ray2D(new Vector2(transform.position.x, transform.position.y - 0.51f), Vector2.down);
        Debug.DrawRay(ray.origin, ray.direction, Color.green);

        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 0.01f);
        
        if(hit.collider != null && hit.collider.tag == "Platform")
        {
            return true;
        }

        return false;
    }
}
