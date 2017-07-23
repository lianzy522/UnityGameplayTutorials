using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacter : MonoBehaviour
{
    public float speed;
    public AudioClip dieSound;
    public int coinCount;


    Rigidbody rigid;
    bool isAlive = true;

    [HideInInspector]
    public PlayerController controller;

    void Start ()
    {
        controller = FindObjectOfType<PlayerController>();
        rigid = GetComponent<Rigidbody>();
        coinCount = 0;
    }

    void Update()
    {
        if(transform.position.y < -30)
        {
            if(isAlive == true && !controller.gameMode.isGameEnd)
            {
                Die();
            }

        }
    }

    void Die()
    {
        isAlive = false;
        AudioSource.PlayClipAtPoint(dieSound, transform.position);
        controller.GameEnd();
    }

    public void Move(Vector3 force)
    {
        rigid.AddForce(force);
    }

    public void Jump()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 0.6f))
        {
            rigid.AddForce((Vector3.up * 8f), ForceMode.Impulse);
        }
    }

    public void AddCoin()
    {
        coinCount++;
    }
}
