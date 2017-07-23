using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    PlayerCharacter character;
    GameMode gameMode;

    private void Awake()
    {
        character = FindObjectOfType<PlayerCharacter>();
        gameMode = FindObjectOfType<GameMode>();
    }

    void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(gameMode.gameState == GameMode.GameState.End)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            character.Attack();
        }
    }
}
