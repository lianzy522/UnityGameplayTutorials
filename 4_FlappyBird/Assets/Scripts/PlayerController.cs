using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    PlayerCharacter character;
	void Awake ()
    {
        character = FindObjectOfType<PlayerCharacter>();
    }

	void Update ()
    {
        if (!character.isAlive) return;

		if(Input.GetMouseButtonDown(0))
        {
            character.Up();
        }
	}
}
