using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockFactory : MonoBehaviour
{
    public GameObject blockPrefab;//方块预制体 之后会通过 加载脚本加载保存

    public GameObject offcut;//边角料

    GameObject lastBlock;//上一次方块记录, 之后会通过 事件监听数值变化而改变 

    float blockVerticalOffSet = 1f;//出生方块的高度 之后会通过 加载脚本加载保存

    float blockHorizontalOffSet = -10;//出生方块的高度 之后会通过 加载脚本加载保存

    public static int blockIndex=0;//记录方块数量
    private void Start()
    {
        lastBlock = GameManager.Instance._curTopBlock; //初始化
        EventSystem.OnTopBlockChange += LastBlockChange;

        EventSystem.OnCreateNewFoundation += CreateNewFoundation;
    }




    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            BlockInitialized(blockVerticalOffSet, blockHorizontalOffSet);
        }
    }


    // 方块的创建与初始化 设置了 缩放 位置 父物体 和添加移动控制脚本,每次的创建都是由上一次方块来决定的.
    void BlockInitialized(float blockVerticalOffSet, float blockHorizontalOffSet)
    {
        var _curblock = Instantiate<GameObject>(blockPrefab).transform;

        _curblock.localScale = lastBlock.transform.localScale;

        if (blockIndex % 2 == 1)
        {
            _curblock.position = lastBlock.transform.position + Vector3.up * blockVerticalOffSet + Vector3.forward * blockHorizontalOffSet;
        }
        else
        {
            _curblock.position = lastBlock.transform.position + Vector3.up * blockVerticalOffSet + Vector3.right * blockHorizontalOffSet;
        }

        _curblock.SetParent(lastBlock.transform.parent);

        _curblock.gameObject.AddComponent<MoveCubeContraller>();

        blockIndex++;
        //lastBlock=_curblock; //之后 当方块完全落下之后 会在MoveCubeController重新设置
    }

    /// <summary>
    /// 当顶层改变时候的回调函数
    /// </summary>
    void LastBlockChange()
    {
        lastBlock = GameManager.Instance._curTopBlock;
    }

    private void OnDestroy()
    {
        //EventSystem.OnTopBlockChange -= LastBlockChange;
    }




    /// <summary>
    /// 用来处理新顶层生成与缩放
    /// </summary>
    /// <param name="movecube">停止下来作为新顶层的移动方块</param>
    void CreateNewFoundation(GameObject movecube)
    {
        Vector3 tPos = lastBlock.transform.position;//获取顶层的位置
        Vector3 tSize = lastBlock.transform.localScale;//获取顶层缩放大小
        Vector3 mPos = movecube.transform.position;//获取移动方块的位置
        var z_Offset = Mathf.Abs(tPos.z - mPos.z);
        var x_Offset = Mathf.Abs(tPos.x - mPos.x);
        if (blockIndex % 2 == 0)
        {
            if (mPos.z < tPos.z)
            {
                movecube.transform.position = new Vector3(mPos.x, mPos.y, tPos.z - z_Offset / 2);


                var v3 = new Vector3(tPos.x, tPos.y + tSize.y, tPos.z - (tSize.z / 2 + z_Offset / 2));
                CreatOffCut(ref v3, ref tSize, z_Offset);
            }
            else
            {
                movecube.transform.position = new Vector3(mPos.x, mPos.y, tPos.z + z_Offset / 2);

                var v3 = new Vector3(tPos.x, tPos.y + tSize.y, tPos.z + (tSize.z / 2 + z_Offset / 2));
                CreatOffCut(ref v3, ref tSize, z_Offset);
            }
            movecube.transform.localScale = new Vector3(tSize.x, tSize.y, Mathf.Abs(tSize.z - z_Offset));
        }
        else
        {
            if (mPos.x < tPos.x)
            {
                movecube.transform.position = new Vector3(tPos.x - x_Offset / 2, mPos.y, mPos.z);


                var v3 = new Vector3(tPos.x - (tSize.x / 2 + x_Offset / 2), tPos.y + tSize.y, tPos.z);
                CreatOffCut(ref v3, ref tSize, x_Offset);
            }
            else
            {
                movecube.transform.position = new Vector3(tPos.x + x_Offset / 2, mPos.y, mPos.z);

                var v3 = new Vector3(tPos.x + (tSize.x / 2 + x_Offset / 2), tPos.y + tSize.y, tPos.z);
                CreatOffCut(ref v3, ref tSize, x_Offset);
            }
            movecube.transform.localScale = new Vector3(Mathf.Abs(tSize.x - x_Offset), tSize.y, tSize.z);
        }

    }


    void CreatOffCut(ref Vector3 pos, ref Vector3 tSize, float Offset)
    {
        var offcuting = Instantiate<GameObject>(offcut, pos, Quaternion.identity);
        if (blockIndex % 2 == 0)
        {
            offcuting.transform.localScale = new Vector3(tSize.x, tSize.y, Offset);
        }
        else
        {
            offcuting.transform.localScale = new Vector3(Offset, tSize.y, tSize.z);       
        }
        offcuting.AddComponent<OffCutController>();
    }

    private void OnDisable()
    {
        EventSystem.OnTopBlockChange -= LastBlockChange;
        EventSystem.OnCreateNewFoundation -= CreateNewFoundation;
        blockIndex=0;
    }
}
