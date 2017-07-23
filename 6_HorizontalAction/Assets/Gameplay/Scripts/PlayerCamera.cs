using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform target;

    public float distance = 8.0f;
    public float hight = -1.0f;

    void LateUpdate()
    {
        if (!target)
            return;

        transform.position = target.position;
        //Z轴间隔距离
        transform.position -= Vector3.forward * distance;
        //高度调整
        transform.position = new Vector3(transform.position.x, hight, transform.position.z);
    }
}