using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacter : MonoBehaviour
{
    public float speed;
    public float turnSpeed;
    public ParticleSystem explosionParticles;

    public Rigidbody shell;
    public Transform muzzle;

    public float launchForce = 10;
    public AudioSource shootAudioSource;

    public float health;
    float healthMax;
    bool isAlive;

    public Slider healthSlider;                             // The slider to represent how much health the tank currently has.
    public Image healthFillImage;                           // The image component of the slider.
    public Color healthColorFull = Color.green;
    public Color HealthColorNull = Color.red;

    CharacterController cc;

    bool attacking = false;
    public float attackTime;

    Animator animator;

    void Start ()
    {
        animator = GetComponentInChildren<Animator>();
        cc = GetComponent<CharacterController>();
        healthMax = health;
        isAlive = true;
        RefreshHealthHUD();
        explosionParticles.gameObject.SetActive(false);
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        RefreshHealthHUD();
        if (health <= 0f && isAlive)
        {
            Death();
        }
    }

    public void RefreshHealthHUD()
    {
        healthSlider.value = health;
        healthFillImage.color = Color.Lerp(HealthColorNull, healthColorFull, health / healthMax);
    }

    public void Death()
    {
        isAlive = false;
        explosionParticles.transform.parent = null;
        explosionParticles.gameObject.SetActive(true);
        ParticleSystem.MainModule mainModule = explosionParticles.main;
        Destroy(explosionParticles.gameObject, mainModule.duration);
        gameObject.SetActive(false);

    }

    public void Move(Vector3 v)
    {
        if (!isAlive) return;
        if (attacking) return;
        Vector3 movement = v * speed;
        cc.SimpleMove(movement);
        if(animator)
        {
            animator.SetFloat("Speed", cc.velocity.magnitude);
        }
    }

    public void Rotate(Vector3 lookDir)
    {
        var targetPos = transform.position + lookDir;
        var characterPos = transform.position;

        //去除Y轴影响
        characterPos.y = 0;
        targetPos.y = 0;
        //角色面朝目标的向量
        Vector3 faceToDir = targetPos - characterPos;
        //角色面朝目标方向的四元数
        Quaternion faceToQuat = Quaternion.LookRotation(faceToDir);
        //球面插值
        Quaternion slerp = Quaternion.Slerp(transform.rotation, faceToQuat, turnSpeed * Time.deltaTime);

        transform.rotation = slerp;
    }

    public void Fire()
    {
        if (!isAlive) return;
        if (attacking) return;

        Rigidbody shellInstance = Instantiate(shell, muzzle.position, muzzle.rotation) as Rigidbody;
        shellInstance.velocity = launchForce * muzzle.forward;
        shootAudioSource.Play();

        if(animator)
        {
            animator.SetTrigger("Attack");
        }


        attacking = true;
        Invoke("RefreshAttack", attackTime);
    }

    void RefreshAttack()
    {
        attacking = false;
    }
}
