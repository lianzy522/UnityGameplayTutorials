using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{

    public BotGroup botGroupPrefab;

    //本怪物最多生成多少组怪物
    public float botGroupTotal = 5;
    //当前生成了多少组怪物
    private int botGroupCount = 0;
    private int score = 0;

    //怪物组内的怪物数量
    int spawnBotNum = 1;
    int spawnBotNumMax = 5;

    private int spawnIncreaseCount = 1;
    private int spawnNormalCount = 5;

    bool canSpawnNewGroup = false;

    BotGroup.Type spawnGroupTypeCurrent = BotGroup.Type.Normal;
    BotGroup.Type spawnGroupTypeNext = BotGroup.Type.Normal;

    private float spawnLine;
    private float spawnLineNextOffect = 50.0f;
    public const float spawnLineNextOffectMin = 20.0f;            // 怪物出现间隔的最小值
    public const float spawnLineNextOffectMax = 50.0f;          // 怪物出现间隔的最大值

    private float spawnNextSpeed = 1f;
    public float spawnNextSpeedMin = 1.0f;           // 移动速度的最小值
    public float spawnNextSpeedMax = 6.0f;         // 移动速度的最高值

    private float spawnPosOffect = 15f;



    PlayerCharacter character;
    PlayerHUD hud;

    public enum GameState
    {
        Start,
        Gaming,
        End,
    }


    public GameState gameState = GameState.Start;


    private void Awake()
    {
        character = FindObjectOfType<PlayerCharacter>();
        hud = FindObjectOfType<PlayerHUD>();

    }

    void Update()
    {
        StateLoop();

    }

    void StateLoop()
    {
        switch (gameState)
        {
            case GameState.Start:
                gameState = GameState.Gaming;
                break;
            case GameState.Gaming:
                //杀了全部的怪物后，游戏结束
                if((score / 10) >= botGroupTotal)
                {
                    gameState = GameState.End;
                }
                //没有杀够怪物时，游戏继续
                if(botGroupCount < botGroupTotal)
                {
                    GameLoop();
                }

                break;
            case GameState.End:
                //游戏结束时 显示游戏结束的面板
                hud.ShowGameEnd();

                break;
        }

    }


    public void GameLoop()
    {

        //准备生成一波怪物组
        if (!canSpawnNewGroup)
        {
            canSpawnNewGroup = true;

            if (canSpawnNewGroup)
            {
                // 通过玩家的当前位置计算玩家触发生成怪物组的触发位置

                if (spawnGroupTypeNext == BotGroup.Type.Normal)
                {
                    spawnLine = character.transform.position.z + spawnLineNextOffect;
                }
                else if (spawnGroupTypeNext == BotGroup.Type.Slow)
                {
                    spawnLine = character.transform.position.z + 50.0f;
                }
            }
        }

        ////////////////////////////////////////////////////////////////
        // 当玩家没有到达触发位置时，不生成任何怪物组
        if (character.transform.position.z <= spawnLine)
        {
            return;
        }

        //实际开始生成怪物组
        spawnGroupTypeCurrent = spawnGroupTypeNext;
        SpawnBotGroup(spawnGroupTypeCurrent);

        // 更新下次出现分组时的怪物数量
        spawnBotNum++;
        spawnBotNum = Mathf.Min(spawnBotNum, spawnBotNumMax);

        botGroupCount++;
        canSpawnNewGroup = false;

        //刷新下波怪物组的类型
        UpdateNextGroupType();
    }

    public bool IsNormalGroup()
    {
        if (spawnGroupTypeCurrent != BotGroup.Type.Normal || spawnGroupTypeNext != BotGroup.Type.Normal)
        {
            return false;
        }

        return true;
    }


    public void UpdateNextGroupType()
    {
        //还有要生成的正常怪物时，先生成正常怪物
        if (spawnNormalCount > 0)
        {
            float rate;

            //用于计算10个以内的怪物加速
            rate = (float)botGroupCount / 10.0f;
            rate = Mathf.Clamp01(rate);

            //砍杀的怪物越多，下一批怪物的速度也越快，
            //通过rate，来在最大和最小速度间插值，使得下一波的速度，与当前击杀怪物数量成正比
            spawnNextSpeed = Mathf.Lerp(spawnNextSpeedMin, spawnNextSpeedMax, rate);

            //砍杀的怪物越多，下一批怪物的来的也会越快，
            //通过rate，来在最大和最小位置偏移中插值，使得下一波怪物到来的偏移位置，与当前击杀怪物数量成反比
            spawnLineNextOffect = Mathf.Lerp(spawnLineNextOffectMax, spawnLineNextOffectMin, rate);

            spawnNormalCount--;

            if (spawnNormalCount <= 0)
            {
                //当普通型怪物都生成完后，随机一种增强型怪物
                spawnGroupTypeNext = (BotGroup.Type)Random.Range(1, 1);
                spawnIncreaseCount = 1;
            }

            return;
        }

        //有要生成的强化类怪物时，生成强化类怪物
        if (spawnIncreaseCount > 0)
        {
            spawnIncreaseCount--;

            if (spawnIncreaseCount <= 0)
            {
                //强化类怪物生成完后，必定生成普通怪物
                spawnGroupTypeNext = BotGroup.Type.Normal;
                //下一轮随机生成多少组普通怪物
                spawnNormalCount = Random.Range(3, 7);
            }

            return;
        }

    }

    /// <summary>
    /// 根据类型，生成一组怪物
    /// </summary>
    /// <param name="type"></param>
    private void SpawnBotGroup(BotGroup.Type type)
    {
        float speed = spawnNextSpeed;
        Vector3 spawnPos;
        spawnPos = character.transform.position;
        spawnPos.z += spawnPosOffect;
        switch (spawnGroupTypeCurrent)
        {

            case BotGroup.Type.Slow:
                {
                    speed = 1.0f;
                }
                break;

            case BotGroup.Type.Normal:
                {
                    speed = spawnNextSpeed;
                }
                break;
        }

        BotGroup botGroup = GameObject.Instantiate<BotGroup>(botGroupPrefab);
        //怪物组生成的Y轴位置处于自身碰撞盒范围的一半
        var extents = botGroup.GetComponent<Collider>().bounds.extents;
        spawnPos.y = extents.y / 2;
        botGroup.transform.position = spawnPos;
        //怪物组生成内部怪物
        botGroup.SpawnBot(spawnBotNum);
        botGroup.runSpeed = speed;
    }

    public void Scored()
    {
        score += 10;
        hud.Scored(score);
    }
}
