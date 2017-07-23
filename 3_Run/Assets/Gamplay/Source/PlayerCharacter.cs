using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TransColor
{
    Red,
    Green,
    Undefine,
}

public class PlayerCharacter : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float doubleJumpForce;
    public float maxVelocity;
    public bool isAlive;

    public ParticleSystem particleRed;
    public ParticleSystem particleGreen;
    public ParticleSystem particleDie;
    public AudioClip jumpClip;
    public AudioClip transClip;


    int jumpCount = 0;
    bool inGround;

    TransColor colorCurrent;
    Rigidbody rigid;
    Renderer render;
    Animator animator;
    Collision collisionRet;

    void Start ()
    {
        rigid = GetComponent<Rigidbody>();
        render = GetComponentInChildren<Renderer>();
        animator = GetComponentInChildren<Animator>();
        
        render.material.color = Color.red;
        colorCurrent = TransColor.Red;
        particleRed.gameObject.SetActive(true);
        particleGreen.gameObject.SetActive(false);

        particleDie.Stop();
        isAlive = true;
    }

    private void FixedUpdate()
    {
        if (!isAlive) return;
        
        if (collisionRet != null)
        {
            if (collisionRet.gameObject.CompareTag("Red"))
            {
                if (colorCurrent != TransColor.Red)
                {
                    Die();
                }
            }
            else if (collisionRet.gameObject.CompareTag("Green"))
            {
                if (colorCurrent != TransColor.Green)
                {
                    Die();
                }
            }
            else if (collisionRet.gameObject.CompareTag("Undefine"))
            {
                Die();
            }
        }

        inGround = GroundCheck();
        if (inGround)
        {

            SwitchDustWithState();
        }
        else
        {
            particleRed.gameObject.SetActive(false); 
            particleGreen.gameObject.SetActive(false);
        }

        if (animator)
        {
            animator.SetBool("InGround", inGround);
        }
    }

    public void Move()
    {
        if (!isAlive) return;

        var vel = rigid.velocity;
        vel.z = (Vector3.forward * speed).z;
        rigid.velocity = vel;
    }

    public void Jump()
    {
        if (!isAlive) return;

        if (inGround || jumpCount < 2)
        {
            if (jumpCount == 0)
            {
                rigid.velocity = new Vector3(rigid.velocity.x, 0f, rigid.velocity.z);
                rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
            else if (jumpCount == 1)
            {
                rigid.velocity = new Vector3(rigid.velocity.x, 0f, rigid.velocity.z);
                rigid.AddForce(Vector3.up * doubleJumpForce, ForceMode.Impulse);
            }

            AudioSource.PlayClipAtPoint(jumpClip, transform.position);
            jumpCount++;
        }
    }

    public bool GroundCheck()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.01f);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                return true;
            }
        }
        return false;
    }

    void Die()
    {
        isAlive = false;
        particleDie.Play();
        rigid.velocity = Vector3.zero;
        render.enabled = false;
        particleRed.Stop();
        particleGreen.Stop();
        Invoke("RestartGame", 1);
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    void SwitchDustWithState()
    {
        switch (colorCurrent)
        {
            case TransColor.Green:
                particleGreen.gameObject.SetActive(true);
                particleRed.gameObject.SetActive(false);
                break;
            case TransColor.Red:
                particleGreen.gameObject.SetActive(false);
                particleRed.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void ChangeColorState()
    {
        if (!isAlive) return;

        if (colorCurrent == TransColor.Green)
        {
            colorCurrent = TransColor.Red;
            render.material.color = Color.red;
        }
        else if (colorCurrent == TransColor.Red)
        {
            colorCurrent = TransColor.Green;
            render.material.color = Color.green;
        }

        SwitchDustWithState();
        animator.SetTrigger("ChangeColor");
        AudioSource.PlayClipAtPoint(transClip, transform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        jumpCount = 0;
        collisionRet = collision;
    }

    private void OnCollisionStay(Collision collision)
    {
        collisionRet = collision;
    }

    private void OnCollisionExit(Collision collision)
    {
        collisionRet = null;
    }
}
