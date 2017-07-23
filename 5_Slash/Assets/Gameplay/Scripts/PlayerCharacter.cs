using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public enum AttackMotion
    {
        Left,
        Right,
    }

    Rigidbody rigid;
    Animator animator;

    AttackMotion attackMotion = AttackMotion.Left;

    public float runSpeed = 5.0f;
    public const float runSpeedMax = 10.0f;
    public const float runSpeedAcc = 5.0f;

    public float attackingTime;
    public float hitingTime;

    bool canAttack = true;

    public ParticleSystem swordHitEffect;
    public ParticleSystem swordEffectRight;
    public ParticleSystem swordEffectLeft;
    public AudioClip swordSound;
    public AudioClip swordHitSound;
    public AudioClip FaintSound;

    GameMode gameMode;

    // 攻击判定的持续时间 [sec]
    const float AttackTime = 0.3f;

    void Awake ()
    {
        rigid = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        gameMode = FindObjectOfType<GameMode>();
        swordEffectLeft.Stop();
        swordEffectRight.Stop();
        swordHitEffect.Stop();


    }
	
	void Update ()
    {
        Vector3 velocityTemp = rigid.velocity;

        //当前的runSpeed + 每秒的加速度
        runSpeed += runSpeedAcc * Time.deltaTime;
        //限定最大速度
        runSpeed = Mathf.Clamp(runSpeed, 0.0f, runSpeedMax);

        velocityTemp.z = runSpeed;
        if (velocityTemp.y > 0f)
        {
            //丢弃y轴的位置变化
            velocityTemp.y = 0f;
        }

        rigid.velocity = velocityTemp;


        //攻击动作时间递减
        if(attackingTime > 0)
        {
            attackingTime = attackingTime - Time.deltaTime;
        }

        //受击动作时间递减
        if (hitingTime > 0)
        {
            hitingTime = hitingTime - Time.deltaTime;
        }

    }

    public void Attack()
    {
        
        if (!canAttack) return;

        //受击时，不能攻击
        if (hitingTime > 0) return;

        //攻击动作一左一右
        if (attackMotion == AttackMotion.Left)
        {
            animator.SetTrigger("AttackLeft");
            swordEffectLeft.Play();
            attackMotion = AttackMotion.Right;

        }
        else
        {
            animator.SetTrigger("AttackRight");
            swordEffectRight.Play();
            attackMotion = AttackMotion.Left;
        }

        AudioSource.PlayClipAtPoint(swordSound, transform.position, 1);
        attackingTime = AttackTime;

        //刚攻击完，不能立即攻击
        canAttack = false;

        // attackingTime + 1 秒后，重置为可攻击状态
        CancelInvoke("ResetCanAttack");
        Invoke("ResetCanAttack", attackingTime + 1);
    }

    public void HitEnemy(GameObject target)
    {
        //击中敌人后，立即刷新攻击状态
        ResetCanAttack();
        AudioSource.PlayClipAtPoint(swordHitSound, transform.position);
        swordHitEffect.transform.position = target.transform.position;
        swordHitEffect.Play();
        gameMode.Scored();
    }

    public void ResetCanAttack()
    {
        canAttack = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<BotGroup>())
        {
            if(attackingTime > 0)
            {
                return;
            }

            animator.SetTrigger("Faint");

            //清空累加速度，并将角色向后上方弹开
            this.runSpeed = 0.0f;
            rigid.AddForce(-Vector3.forward * 6000 + Vector3.up * 2000);

            AudioSource.PlayClipAtPoint(FaintSound, transform.position);
            hitingTime = animator.GetCurrentAnimatorStateInfo(0).length;
        }
    }
}
