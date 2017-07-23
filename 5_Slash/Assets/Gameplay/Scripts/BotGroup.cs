using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotGroup : MonoBehaviour
{
    public enum Type
    {
        None = -1,
        Normal,
        Slow,
    }

    BotCharacter[] bots;

    public BotCharacter botPrefab;

    public float runSpeed = 1;

	void Update ()
    {
        Vector3 pos =  transform.position;

        pos.z += runSpeed * Time.deltaTime;

        transform.position = pos;
    }

    public void TakeDamage()
    {
        //受到伤害时，飞溅
        GroupBlowout();
    }

    public void SpawnBot(int botNum)
    {
        bots = new BotCharacter[botNum];
        Vector3 position;
        for (int i = 0; i < botNum; i++)
        {
            position = transform.position;
            BotCharacter bot = Instantiate<BotCharacter>(botPrefab);
            bots[i] = bot;

            // 根据怪物数决定分散范围,并限定在GroupBox内
            
            Vector3 splat_range;
            //根据怪物数量，计算怪物在X,Z轴上的分布范围
            splat_range.x = bot.bounds.size.x * (float)(botNum - 1);
            splat_range.z = bot.bounds.size.z * (float)(botNum - 1);

            //splat_range最大，不能超过怪物组包围盒范围的一半
            var collider = GetComponent<Collider>();
            splat_range.x = Mathf.Min(splat_range.x, collider.bounds.extents.x);
            splat_range.z = Mathf.Min(splat_range.z, collider.bounds.extents.z);
            
            position.x += Random.Range(-splat_range.x, splat_range.x);
            position.z += Random.Range(0f, splat_range.z); //z轴，让怪物集中分布在包围盒的前方
            position.y = 0;

            bots[i].transform.position = position;
            bots[i].transform.parent = transform;

            bots[i].waveAmplitude = (i + 1) * 0.1f;
            //45度单位偏移
            bots[i].waveRadianOffset = (i + 1) * Mathf.PI / 4.0f;
        }
    }

    public void GroupBlowout()
    {

        Vector3 blowout;           // 怪物飞散的方向，含速度
        Vector3 blowoutUp;         // 飞散方向的垂直分量
        Vector3 blowoutRight;      // 飞散方向的水平分量

        float topRadius;           // 圆锥的顶面半径
        float arcLength;           // 圆锥顶面飞散范围弧长，根据Bot数量决定

        float upAngle;              //绕Y轴旋转角度
        float upAngleCenter;        //初始中心
        float upAngleSwing;         //偏移角
        float rightAngle;           //绕x轴旋转角度

        float blowoutSpeed;         //速度
        float blowoutSpeedVary;     //速度变化调控



        //==============================================
        topRadius = 0.3f;
        blowoutSpeedVary = 10.0f;
        rightAngle = 40.0f;
        upAngleCenter = 180.0f;
        upAngleSwing = 10.0f;
        //=========================================

        //随机一个圆锥前后倾斜的角度
        rightAngle += Random.Range(-5.0f, 5.0f);

        //根据组内怪物数量决定圆周顶面的飞散弧长，不能超过120
        arcLength = bots.Length * 30.0f;
        arcLength = Mathf.Min(arcLength, 120.0f);

        //围绕Y轴的旋转角
        upAngle = upAngleCenter;
        upAngle += upAngleSwing;

        //遍历每一个怪物，决定他们的飞散结果
        foreach (BotCharacter bot in bots)
        {

            //飞散方向
            blowoutUp = Vector3.up; //飞散圆锥的中心轴      
            blowoutRight = Vector3.forward * topRadius; //飞散圆锥的顶面中心到顶面边缘的向量
            blowoutRight = Quaternion.AngleAxis(upAngle, Vector3.up) * blowoutRight;//将上一步得到的向量绕Y轴旋转upAngle
            blowout = blowoutUp + blowoutRight; //两向量相加得到代表飞散方向的第三向量
            blowout.Normalize();

            // 飞散方向计算上前后倾斜
            blowout = Quaternion.AngleAxis(rightAngle, Vector3.right) * blowout;

            // 飞散开的速度
            blowoutSpeed = blowoutSpeedVary * Random.Range(0.8f, 1.5f);
            blowout *= blowoutSpeed;

            // 角速度
            //叉乘后得到的第三向量，与飞散方向和Y轴垂直，便是旋转怪物的方向
            Vector3 angular_velocity = Vector3.Cross(Vector3.up, blowout);
            angular_velocity.Normalize();
            angular_velocity *= Random.Range(0.5f, 1.5f) * blowoutSpeed;

            //应用
           // Debug.DrawRay(transform.position, blowout, Color.green, 1000.0f);
            bot.Blowout(blowout, angular_velocity);

            //改变Y轴旋转角， 使每个怪物的飞散方向，都在顶面飞散弧长的不同位置
            upAngle += arcLength / (bots.Length);
        }

        Destroy(this.gameObject);
    }
}
