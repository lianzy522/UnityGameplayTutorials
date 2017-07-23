using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerCharacter>();

        if (player)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
        }
    }
}
