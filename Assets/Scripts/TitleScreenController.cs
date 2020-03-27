using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenController : MonoBehaviour
{
    [SerializeField]
    int playgroundIdx, gameIdx;
    [SerializeField]
    bool playGame;
    [SerializeField]
    RectTransform button;
    [SerializeField]
    Vector3 playgroundPos, gamePos;

    void Start()
    {
        
    }

    void Update()
    {
        if(InputHandler.leftStick.normalized == Vector2.up || InputHandler.rightStick.normalized == Vector2.up)
        {
            playGame = false;
        }
        else if(InputHandler.leftStick.normalized == Vector2.down || InputHandler.rightStick.normalized == Vector2.down)
        {
            playGame = true;
        }

        if(playGame)
        {
            button.anchoredPosition = gamePos;
            if(InputHandler.x)
            {
                SceneManager.LoadScene(gameIdx);
            }
        }
        else
        {
            button.anchoredPosition = playgroundPos;
            if (InputHandler.x)
            {
                SceneManager.LoadScene(playgroundIdx);
            }
        }
    }
}
