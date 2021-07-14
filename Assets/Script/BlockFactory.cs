using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockFactory : MonoBehaviour
{
    public GameObject blockPrefab;//方块预制体 之后会通过 加载脚本加载保存

    GameObject lastBlock;//上一次方块记录, 之后会通过 事件监听数值变化而改变 

    float blockVerticalOffSet = 1f;//出生方块的高度 之后会通过 加载脚本加载保存

    float blockHorizontalOffSet = -10;//出生方块的高度 之后会通过 加载脚本加载保存

    private void Start()
    {
        lastBlock = GameManager.Instance._curTopBlock; //初始化

        EventSystem.OnTopBlockChange += LastBlockChange;
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

        _curblock.position = lastBlock.transform.position + Vector3.up * blockVerticalOffSet + Vector3.forward * blockHorizontalOffSet;

        _curblock.SetParent(lastBlock.transform.parent);

        _curblock.gameObject.AddComponent<MoveCubeContraller>();

        //lastBlock=_curblock; //之后 当方块完全落下之后 会在MoveCubeController重新设置
    }

    void LastBlockChange()
    {
        lastBlock = GameManager.Instance._curTopBlock;
    }

    private void OnDestroy()
    {
        //EventSystem.OnTopBlockChange -= LastBlockChange;
    }

    private  void OnDisable() 
    {
        EventSystem.OnTopBlockChange -= LastBlockChange;
    }
    
}
