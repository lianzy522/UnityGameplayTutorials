using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    public float lifeTimeMax = 2f;
    public AudioSource explosionAudioSource;
    public ParticleSystem explosionParticles;
    public float explosionRadius;
    public float explosionForce = 1000f;
    public float damageMax = 100f;
    public LayerMask damageMask;
    public bool isRotate = false;

    void Start ()
    {
        Destroy(gameObject, lifeTimeMax);
        if(isRotate)
        {
            GetComponent<Rigidbody>().AddTorque(transform.right * 1000);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, damageMask);

        foreach(var collider in colliders)
        {
            var targetCharacter = collider.GetComponent<PlayerCharacter>();
            if (targetCharacter)
            {
                targetCharacter.TakeDamage(CalculateDamage(collider.transform.position));
            }
        }
        explosionParticles.transform.parent = null;
        explosionAudioSource.Play();
        explosionParticles.Play();

        ParticleSystem.MainModule mainModule = explosionParticles.main;
        Destroy(explosionParticles.gameObject, mainModule.duration);
        Destroy(gameObject);
    }


    float CalculateDamage(Vector3 targetPosition)
    {
        
        var distance = (targetPosition - transform.position).magnitude;
        //距离越小，伤害比例越大
        var damageModify = (explosionRadius - distance) / explosionRadius;
        var damage = damageModify * damageMax;
        return Mathf.Max(2f, damage);
    }
}
