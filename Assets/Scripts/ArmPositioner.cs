using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmPositioner : MonoBehaviour
{
    [SerializeField]
    Transform body, hand;

    void LateUpdate()
    {
        float distance = Vector3.Distance(body.position, hand.position);
        Vector3 midpoint = Vector3.Lerp(body.position, hand.position, 0.5f);

        float rise = hand.position.y - body.position.y;
        float run = hand.position.x - body.position.x;
        float angle = Mathf.Atan(rise / run) * Mathf.Rad2Deg;
        Vector3 rot = new Vector3(0, 0, angle +90);

        transform.localScale = new Vector3(0.07f, distance, 1f);
        transform.position = midpoint;
        transform.eulerAngles = rot;
    }
}
