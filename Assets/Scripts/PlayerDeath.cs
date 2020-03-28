using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField]
    Vector3 respawnLocation;
    [SerializeField]
    float deathY;

    [SerializeField]
    float respawnTime;

    [SerializeField]
    GameObject deathParticle;

    Coroutine respawnRoutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void LateUpdate()
    {
        if(transform.position.y < deathY)
        {
            if (respawnRoutine == null)
            {
                GameObject temp = Instantiate(deathParticle);
                temp.transform.position = transform.position;

                temp.transform.LookAt(Vector3.zero);

                respawnRoutine = StartCoroutine(Respawn());
            }
        }
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);
        rb.velocity = Vector2.zero;
        transform.localPosition = respawnLocation;
        respawnRoutine = null;
    }
}
