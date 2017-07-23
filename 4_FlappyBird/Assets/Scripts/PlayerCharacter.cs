using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCharacter : MonoBehaviour
{
    Rigidbody2D rigid2d;
    Animator animator;

    public float upForce;
    public bool isAlive = true;

    void Start ()
    {
        rigid2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
	


    public void Up()
    {
        animator.SetTrigger("Up");
        rigid2d.velocity = Vector2.zero;
        rigid2d.AddForce(new Vector2(0, upForce));

    }

    public void Die()
    {
        isAlive = false;
        animator.SetTrigger("Die");
        rigid2d.velocity = Vector2.zero;
        GameMode.instance.GameOver();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Die();
    }


}
