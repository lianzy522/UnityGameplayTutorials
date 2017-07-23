using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundRepeate : MonoBehaviour
{
    BoxCollider2D groundCollider;
    float groundHorizontalLength;

	void Start ()
    {
        groundCollider = GetComponent<BoxCollider2D>();
        groundHorizontalLength = groundCollider.size.x;
    }
	
	void Update ()
    {
        //如果当前背景位置在后移过程中，位置小于了背景宽度，那么丢到前面去复用
		if(transform.position.x < -groundHorizontalLength)
        {
            Vector2 offset = new Vector2(groundHorizontalLength * 2f - 0.1f, 0);
            transform.position = (Vector2)transform.position + offset;
        }
	}
}
