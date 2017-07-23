using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerCharacter character;

    void Awake()
    {
        character = FindObjectOfType<PlayerCharacter>();
    }

    void Update ()
    {
        if (!character.isAlive)
        {
            return;
        }

        bool jump;
        bool transColor;

        if (Application.isMobilePlatform)
        {
            jump = false;
            transColor = false;
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {

                if (touch.position.x > (Screen.width / 2))
                {
                    jump = true;
                }
                else
                {
                    transColor = true;
                }
            }
        }
        else
        {
            jump = Input.GetKeyDown(KeyCode.UpArrow);
            transColor = Input.GetKeyDown(KeyCode.Space);
        }

        character.Move();

        if (jump)
        {
            character.Jump();
        }

        if (transColor)
        {
            character.ChangeColorState();
        }
    }
}
