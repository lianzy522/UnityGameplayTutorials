using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public AudioClip triggerSound;


    void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerCharacter>();
        if (player)
        {
            player.controller.LevelComplete();
            AudioSource.PlayClipAtPoint(triggerSound, transform.position);
        }
    }
}
