using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnController : MonoBehaviour
{
    public GameObject columnPrefab;
    public int columnMax = 5;
    public float spawnRate = 3f;
    public float yMin = -1f;
    public float yMax = 3.5f;
    public float xPos = 10f;

    float timeSinceLastSpawned;
      

    GameObject[] colums;
    int currentColumn = 0;




    

    Vector2 originalPos = new Vector2(-10, -20);

	void Start ()
    {
        colums = new GameObject[columnMax];


        for(int i = 0; i < columnMax; i++)
        {
            colums[i] = Instantiate(columnPrefab, originalPos, Quaternion.identity);
        }

    }

	void Update ()
    {

        timeSinceLastSpawned += Time.deltaTime;

        if (GameMode.instance.gameOver == false && timeSinceLastSpawned >= spawnRate)
        {
            timeSinceLastSpawned = 0f;

            float yPos = Random.Range(yMin, yMax);
            colums[currentColumn].transform.position = new Vector2(xPos, yPos);

            currentColumn++;

            if(currentColumn >= columnMax)
            {
                currentColumn = 0;
            }



        }
  
		
	}
}
