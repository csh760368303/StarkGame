using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffCutController : MonoBehaviour
{
    MeshRenderer mr;
    Color c;
    // Start is called before the first frame update
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
