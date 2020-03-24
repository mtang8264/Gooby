using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmPositioner : MonoBehaviour
{
    [SerializeField]
    Transform body, hand;

    void LateUpdate()
    {
        float distance = Vector3.Distance(body.position, hand.position);        // The distance between the body and the hand is used to scale the arm to the right length.
        Vector3 midpoint = Vector3.Lerp(body.position, hand.position, 0.5f);    // The arm is positioned directly between the arm and body to allow it to reach both.

        float rise = hand.position.y - body.position.y;                         // This is trigonometry to find the angle the arm needs to be rotated at.
        float run = hand.position.x - body.position.x;                          // I found the slope of the line by finding the delta in the x and y positions.
        float angle = Mathf.Atan(rise / run) * Mathf.Rad2Deg;                   // You can the find the arctan of that to find the angle and I converted that to degrees.
        Vector3 rot = new Vector3(0, 0, angle +90);                             // Because Unity has 0 degrees as up I had to rotate an additional 90 degrees.

        transform.localScale = new Vector3(0.07f, distance, 1f);                // This scales it to the right length.
        transform.position = midpoint;                                          // Position it.
        transform.eulerAngles = rot;                                            // Rotate it.
    }
}
