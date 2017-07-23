using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    PlayerCharacter character;

    private void Awake()
    {
        character = FindObjectOfType<PlayerCharacter>();
    }

    public void OnAnimationEvent_Attack()
    {
        //在这里试试添加LayerMask,只检测可被攻击的物体
        var colliders = Physics.OverlapSphere(transform.position, (1.5f));
        //Debug.DrawRay(transform.position, Vector3.forward * (1.5f), Color.green, 1000.0f);
        foreach (var collider in colliders)
        {
            var botGroup = collider.GetComponent<BotGroup>();
            if (botGroup)
            {
                character.HitEnemy(collider.gameObject);
                botGroup.TakeDamage();
                return;
            }
        }
    }

}
