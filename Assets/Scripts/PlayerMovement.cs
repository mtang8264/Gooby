using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    float speed = 1f;
    [SerializeField]
    float jumpForce;
    private bool jumpFlag;
    [SerializeField]
    private bool grounded;
    public bool isGrounded { get { return grounded; } }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetButtonDown("X"))
        {
            jumpFlag = true;
        }
    }

    private void FixedUpdate()
    {
        Vector2 movement = new Vector2(InputHandler.leftStick.x, 0f);
        movement *= speed;
        rb.AddForce(movement, ForceMode2D.Force);

        if(jumpFlag)
        {
            jumpFlag = false;
            if(grounded)
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        grounded = GroundCheck();
    }

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
