using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerCharacter character;

	void Start ()
    {
        character = GetComponent<PlayerCharacter>();
	}

    void FixedUpdate()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            character.Fire();
        }

        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        character.Move(new Vector3(h, 0, v));
        var lookDir = Vector3.forward * v + Vector3.right * h;
        if (lookDir.magnitude != 0)
        {
            character.Rotate(lookDir);
        }
    }
}
