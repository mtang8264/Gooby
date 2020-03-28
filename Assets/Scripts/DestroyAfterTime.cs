using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField]
    float timeTillDestruction;

    void Start()
    {
        
    }

    void Update()
    {
        timeTillDestruction -= Time.deltaTime;
        if(timeTillDestruction <= 0)
        {
            Destroy(gameObject);
        }
    }
}
