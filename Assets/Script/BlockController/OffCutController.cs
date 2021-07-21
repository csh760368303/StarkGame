using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//边角料控制器
public class OffCutController : MonoBehaviour
{
    MeshRenderer mr;//边角料 的渲染器
    Color c;//颜色 用来获取Alp值

    void Start()
    {
        mr = GetComponent<MeshRenderer>();
        c=GetComponent<MeshRenderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
        
        c.a -= Time.deltaTime * 1f;
        mr.material.color=c;
        if (c.a<0)
        {
            Destroy(gameObject);
        }
    }
}
