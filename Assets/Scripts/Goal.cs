using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{ 
    [SerializeField]
    Animator flag;
    [SerializeField]
    float requiredTime;
    float timer;

    Coroutine winCoroutine;

    [SerializeField]
    int idxToLoad;

    void Start()
    {
        
    }

    void Update()
    {
        if (timer > requiredTime && winCoroutine == null)
        {
            Debug.Log("Player Wins");
            winCoroutine = StartCoroutine(Win());
        }
    }

    IEnumerator Win()
    {
        flag.SetTrigger("Go");
        yield return new WaitUntil(() => flag.GetCurrentAnimatorClipInfo(0)[0].clip.name == "visible");
        SceneManager.LoadScene(idxToLoad);
        yield return null;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
            return;
        Debug.Log("Player entered");
        timer = 0f;
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        timer += Time.deltaTime;
    }
}
