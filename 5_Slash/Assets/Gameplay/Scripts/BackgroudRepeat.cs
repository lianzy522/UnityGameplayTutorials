using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroudRepeat : MonoBehaviour
{
    //单个背景的宽度
    const float width = 50f;
    //背景数量
    const int widthNum = 2;

    private GameObject mainCamera = null;

    //初始位置
    private Vector3 initPos;

    void Start ()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        initPos = transform.position;
    }
	
	void Update ()
    {

        float totalWidth = width * widthNum;

        //z轴上， 相机距离此背景初始位置的距离
        float dist = mainCamera.transform.position.z - initPos.z;
        // 用背景初始位置与当前相机位置的距离除以整体背景的宽度，再四舍五入后，就是当前相机位置处于几倍的总宽度。
        int n = Mathf.RoundToInt(dist / totalWidth);

        Vector3 position = this.initPos;

        position.z += n * totalWidth;

        this.transform.position = position;

    }
}
