using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public GameMode gameMode;

    [HideInInspector]
    PlayerCharacter character;

    [HideInInspector]
    PlayerCamera playerCamera;

    [HideInInspector]
    PlayerHUD hud;

    void Awake()
    {
        gameMode = FindObjectOfType<GameMode>();
        playerCamera = FindObjectOfType<PlayerCamera>();
        hud = FindObjectOfType<PlayerHUD>();
        character = FindObjectOfType<PlayerCharacter>();
    }

    void Update()
    {
        if(gameMode.isGameEnd)
        {
            return;
        }
        
        var moveHorizontal = Input.GetAxis("Horizontal");
        var moveVertical = Input.GetAxis("Vertical");
        
        var cameraTrans = playerCamera.cameraComponent.transform;

        var movement = (moveVertical * cameraTrans.forward + moveHorizontal * cameraTrans.right).normalized;

        character.Move(movement * character.speed);

        var jump = Input.GetKeyDown(KeyCode.Space);

        if (jump)
        {
            character.Jump();
        }

    }

    public void GameEnd()
    {
        hud.GameOver();
        gameMode.isGameEnd = true;
    }

    public void LevelComplete()
    {
        hud.LevelComplete();
        gameMode.isGameEnd = true;
    }
}
