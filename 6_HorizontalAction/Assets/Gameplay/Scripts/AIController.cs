using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    Character character;
    float lastCheckStateTime = 0;

    float simulateInputX;
    bool simulateJump;

    void Start()
    {
        character = GetComponent<Character>();
    }

	void Update ()
    {
        //每隔一秒检查一次AI状态，确认接下来的行为
        if (Time.time > lastCheckStateTime + 2)
        {
            lastCheckStateTime = Time.time;

            simulateInputX = Random.Range(-1f, 1f);
            simulateJump = Random.Range(0, 2) == 1 ? true : false;

        }

        //实际执行行为
        MoveControl(simulateInputX);
        JumpControl(simulateJump);
    }

    void MoveControl(float inputX)
    {
        character.Move(inputX);
        if (inputX != 0)
        {
            var dir = Vector3.right * inputX;
            character.Rotate(dir, 10);
        }
    }

    void JumpControl(bool jump)
    { 
        if (jump)
        {
            character.Jump();
            simulateJump = false;
        }
    }
}
