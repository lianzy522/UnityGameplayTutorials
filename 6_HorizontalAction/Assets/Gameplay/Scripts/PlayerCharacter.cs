using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    public Transform grabSocket;
    public Transform grabObject;
    public int armsAnimatorLayer;

    void Start()
    {
        //将手臂层动画的权重设置为完全覆盖
        if (animator)
            animator.SetLayerWeight(armsAnimatorLayer, 1f);
    }

    protected override void Update()
    {
        animator.SetBool("Grab", grabObject ? true : false);
        base.Update();
    }

    public void GrabCheck()
    {
        
        if (grabObject != null && rotateComplete)
        {
            //如果有抓取物，且转向完成
            grabObject.transform.SetParent(null);
            grabObject.GetComponent<Rigidbody>().isKinematic = false;
            grabObject = null;
        }
        else
        {
            //如果没有抓取物
            var dist = cc.radius;

            RaycastHit hit;
            Debug.DrawLine(transform.position, transform.position + transform.forward * (dist + 1f), Color.green, 10f);
            if (Physics.Raycast(transform.position, transform.forward, out hit, dist + 1f))
            {
                if (hit.collider.CompareTag("GrabBox"))
                {
                    grabObject = hit.transform;
                    grabObject.SetParent(grabSocket);
                    grabObject.localPosition = Vector3.zero;
                    grabObject.localRotation = Quaternion.identity;
                    grabObject.GetComponent<Rigidbody>().isKinematic = true;
                }
            }
        }


    }
}
