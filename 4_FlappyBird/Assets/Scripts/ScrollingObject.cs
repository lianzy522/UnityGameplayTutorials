using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    Rigidbody2D rigid2d;
    
	void Start ()
    {
        rigid2d = GetComponent<Rigidbody2D>();
        rigid2d.velocity = new Vector2(GameMode.instance.scrollSpeed, 0);
	}

	void Update ()
    {
		if(GameMode.instance.gameOver == true)
        {
            rigid2d.velocity = Vector2.zero;
        }
	}
}
