using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script just makes sure that the shading on Gooby is not rotating.
public class RotationStabilizer : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Quaternion.identity;
    }
}
