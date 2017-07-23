using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AIController : MonoBehaviour
{
    NavMeshAgent agent;
    PlayerCharacter character;
    Transform targetTrans;

    void Start ()
    {
        character = GetComponent<PlayerCharacter>();
        agent = GetComponent<NavMeshAgent>();

        targetTrans = GameObject.FindGameObjectWithTag("Player").transform;
        
        InvokeRepeating("FireControl", 1, 3);
    }

    void FireControl()
    {
        character.Fire();
    }

	void Update ()
    {
        agent.destination = targetTrans.position;
        transform.LookAt(targetTrans);
    }
}
