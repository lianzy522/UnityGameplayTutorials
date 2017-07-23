using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotCharacter : MonoBehaviour
{
    bool isAlive = true;
    Vector3 initialPosition;

    [HideInInspector]
    public Bounds bounds;

    [HideInInspector]
    public float waveAmplitude;
    [HideInInspector]
    public float waveRadianOffset;
    public float yHeight;

    void Awake()
    {
        bounds = GetComponent<Collider>().bounds;
        GetComponent<Collider>().enabled = false;
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        if (isAlive)
        {
            var pos = transform.localPosition;
            pos.y = yHeight;

            //控制怪物来回左右波动
            //2.0f * Mathf.PI 是波动的值域，代表360度
            //Mathf.Repeat(Time.time, 1f) 是获取一个0~1之间递增且重复的数值
            //waveRadianOffset 是每个怪物不同的偏移
            //Mathf.Sin(waveRadian) 将waveRadian 映射在 +1~-1之间，
            float waveRadian =  2.0f * Mathf.PI * Mathf.Repeat(Time.time, 1f) + waveRadianOffset;
            Debug.Log(Mathf.Sin(waveRadian));
            float waveOffset = waveAmplitude * Mathf.Sin(waveRadian);
            pos.x = initialPosition.x + waveOffset;
            transform.localPosition = pos;

            //控制怪物来回旋转
            if (waveAmplitude > 0.0f)
            {
                transform.rotation = Quaternion.AngleAxis(15 * Mathf.Sin(waveRadian), Vector3.up);
            }
  

        }
    }

    public void Blowout(Vector3 blowout, Vector3 angularVelocity)
    {
        GetComponent<Animator>().SetTrigger("Collapse");
        transform.parent = null;
        isAlive = false;
        Destroy(gameObject, 3);
        GetComponent<Rigidbody>().velocity = blowout;
        GetComponent<Rigidbody>().angularVelocity = angularVelocity;
    }
}
