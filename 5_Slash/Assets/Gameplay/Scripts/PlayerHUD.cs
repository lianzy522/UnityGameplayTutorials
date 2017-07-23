using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHUD : MonoBehaviour
{
    public Transform gameEndTips;
    public Text scoreText;

	public void Scored(int score)
    {
        scoreText.text = "Score: " + score;
    }

    public void ShowGameEnd()
    {
        gameEndTips.gameObject.SetActive(true);
    }
}
