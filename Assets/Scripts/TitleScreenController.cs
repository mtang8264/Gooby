using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenController : MonoBehaviour
{
    [SerializeField]
    int sceneToLoad;

    void Start()
    {
        
    }

    void Update()
    {
        if(InputHandler.x)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
