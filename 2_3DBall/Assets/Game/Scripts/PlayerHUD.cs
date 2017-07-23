using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public Text goldCount;
    public GameObject gameoverPanel;
    public GameObject levelCompletePanel;

    PlayerCharacter character;

    private void Awake()
    {
        character = FindObjectOfType<PlayerCharacter>();
    }

    public void OnClick_RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Start");
    }

    public void GameOver()
    {
        gameoverPanel.SetActive(true);
        levelCompletePanel.SetActive(false);
    }

    public void LevelComplete()
    {
        levelCompletePanel.SetActive(true);
        gameoverPanel.SetActive(false);
    }

    public void Update()
    {
        if (!character) return;

        goldCount.text = character.coinCount.ToString();
    }

}
