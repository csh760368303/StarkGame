using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//游戏管理类 负责游戏进程逻辑
public class GameManager : SignletonBase<GameManager>
{
/*----------------------------------------------------------*/
    public bool gameOver=false;


/*----------------------------------------------------------*/

    //记录并保存当前 游戏 最上方方块
    public GameObject _curTopBlock;



    protected override void Awake()
    {
        base.Awake();
        //不可在runtime中销毁
        DontDestroyOnLoad(this);

    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
