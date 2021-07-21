using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//这个类用来控制方块的移动
public class MoveCubeContraller : MonoBehaviour
{
    /*----------------------------------------------------------*/
    //方块一些参数 之后由 BlockFactory 创建新对象MoveCubeModel 从csv中读取参数获取赋值
    float translationSpeed = 8f;//水平方向方块的移动速度
    float drapSpeed = 3f;//竖直方向方块掉落的速度
    Vector3 initialPos; //初始位置(诞生点)
    bool forward; //是否往前走 
    Vector3 target;//掉落位置 目的地
    bool Istranslation;//是否保持移动 默认为出生后就保持移动 直到鼠标右键点击后 改变状态
    bool IsDropDown;//控制 是否掉落 默认为false 等待点击右键后,改变状态

    int blockIndex; //从factory获取当前方块的序列号, 之后会通过model来实现数据的获取

    float distacen = 10f;//方块与中心值 的距离
    /*----------------------------------------------------------*/
    private void Awake()
    {
        forward = true;
        Istranslation = true;
        IsDropDown = false;
        initialPos = transform.position;
    }
    private void Start()
    {
        blockIndex = BlockFactory.blockIndex;//获取当前方块序列号
    }
    private void Update()
    {


        if (Input.GetMouseButtonDown(1))
        {
            //停止移动
            Istranslation = false; 

            //位置修正
            PositionAmendMth(0.5f,5);

            //设置下落目标
            target = transform.position + Vector3.down * 0.5f;

            //开启下落状态
            IsDropDown = true;
            //transform.position = Vector3.MoveTowards(transform.position, target, drapSpeed * Time.deltaTime);//向目标位置移动没有缓动
        }
    }
    private void FixedUpdate()
    {
        distacen = Vector3.Distance(transform.position, initialPos);//记录当前位移情况

        //速度修正
        SpeedAmendMth();

        //移动方法控制器
        if (Istranslation)
        {
            TranslationFun();
        }

        //坠落方法控制器
        if (IsDropDown)
        {
            FallDown();
        }
    }

    #region 方法群组
    //平移方法
    void TranslationFun() 
    {
        if (forward) //如果确定往前走
        {
            if (blockIndex % 2 == 0)
            {
                transform.Translate(0, 0, Time.deltaTime * translationSpeed); //z前进
            }
            else
            {
                transform.Translate(Time.deltaTime * translationSpeed, 0, 0); //x前进
            }

            if (distacen >= 20) //前进超过20m
                forward = !forward; //变为后退状态                    
        }
        else //如果往后退
        {
            if (blockIndex % 2 == 0)
            {
                transform.Translate(0, 0, -Time.deltaTime * translationSpeed); //z后退
            }
            else
            {
                transform.Translate(-Time.deltaTime * translationSpeed, 0, 0); //x后退
            }

            if (distacen <= 0)  //如果退回到初始位置
                forward = !forward; //又往前走
        }
    }

    //掉落方法 在方块停止后下落
    void FallDown()
    {
        if (Vector3.Distance(target, transform.position) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, drapSpeed * Time.deltaTime);//向目标位置移动没有缓动
        }
        else
        {
            IsDropDown = false;
            GamestatusChange();
        }
    }
    //速度修正方法 , 最高8 最低6 的二次函数方程
    void SpeedAmendMth()
    {
        translationSpeed = 0.02f * distacen * distacen - 0.4f * distacen + 8f;
    }
    //位置修正方法,根据两个参数 范围 和 强度 进行修正
    void PositionAmendMth(float AmendRanger,float AmendStrength)
    {
        if (Vector3.Distance(transform.position, initialPos) <= 10f+AmendRanger && Vector3.Distance(transform.position, initialPos) > 10f-AmendRanger)
        {
            transform.position = GameManager.Instance._curTopBlock.transform.position + Vector3.up * 1;
        }
    }

    #endregion 
   

    //用来反馈当前游戏进程方法  在每次方块降落完毕之后进行判断
    void GamestatusChange()
    {
        //Debug.Log($"进行变换之前顶层方块的Z坐标: {transform.position.z},和Z轴方向上的缩放: {transform.localScale.z}");
        //对新的顶层进行处理
        EventSystem.OnCreateNewFoundation?.Invoke(gameObject);
        //.Log("---------------------------------------------------------------------------------------------");
        //Debug.Log($"进行变换之后顶层方块的Z坐标: {transform.position.z},和Z轴方向上的缩放: {transform.localScale.z}");

        //生成边角料 

        //z轴偏差大于自身z轴尺寸,说明没落在顶层上
        if (Mathf.Abs(transform.position.z - GameManager.Instance._curTopBlock.transform.position.z) >= transform.localScale.z)
        {
            //调用动画

            //调用特效

            //游戏结束
            this.enabled = false;
            GameManager.Instance.gameOver = true;
            //在其他脚本处理游戏结束逻辑
        }
        else //否则就视为成功落在顶层底座上
        {
            //生成新顶层和边角料
            GameManager.Instance._curTopBlock = gameObject;
            EventSystem.OnTopBlockChange?.Invoke();
            //当完成任务后此脚本关闭监听
            this.enabled = false;
        }
    }

}
