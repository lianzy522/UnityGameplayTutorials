using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MoveToPoints : MonoBehaviour
{
    public float speed;
    public float delay;
    public MovementType movementType;

    Rigidbody rigid;
    List<Transform> waypoints = new List<Transform>();
    int waypointCurrent;
    bool playForward = true;
    bool arrived = false;
    float arrivedTime;
    

    public enum MovementType
    {
        Once,
        Loop,
        PingPong,
    }



    void Start ()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.isKinematic = true;
        rigid.useGravity = false;
        rigid.interpolation = RigidbodyInterpolation.Interpolate;

        foreach (Transform child in transform)
        {
            if (child.tag == "Waypoint")
                waypoints.Add(child);
        }

        foreach (Transform waypoint in waypoints)
            waypoint.parent = null;
    }

    void FixedUpdate()
    {
        if (!arrived)
        {
            Vector3 direction = (waypoints[waypointCurrent].position - transform.position).normalized;
            rigid.MovePosition(transform.position + (direction * speed * Time.fixedDeltaTime));
        }
    }

    void Update ()
    {
		if(waypoints.Count > 0)
        {
            if(!arrived)
            {
                if (Vector3.Distance(transform.position, waypoints[waypointCurrent].position) < 0.3f)
                {
                    arrivedTime = Time.time;
                    arrived = true;
                }
            }
            else
            {
                if(Time.time > arrivedTime + delay)
                {
                    NextWaypoint();
                    arrived = false;
                }
            }
        }
	}

    void NextWaypoint()
    {
        if(movementType == MovementType.Once)
        {
            waypointCurrent++;
            if (waypointCurrent == waypoints.Count)
            {
                enabled = false;
            }
        }
        else if(movementType == MovementType.Loop)
        {
            waypointCurrent = (waypointCurrent == waypoints.Count - 1) ? 0 : waypointCurrent + 1;
        }
        else if(movementType == MovementType.PingPong)
        {
            if(waypointCurrent == waypoints.Count - 1)
            {
                playForward = false;
            }
            else if (waypointCurrent == 0)
            {
                playForward = true;

            }
            waypointCurrent = playForward ? waypointCurrent + 1 : waypointCurrent - 1; 
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        foreach (Transform child in transform)
        {
            if (child.tag == "Waypoint")
                Gizmos.DrawSphere(child.position, .7f);
        }
    }
}
