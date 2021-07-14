using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCubeContraller : MonoBehaviour
{
    /*----------------------------------------------------------*/
    //方块一些参数 之后由 BlockFactory 创建新对象MoveCubeModel 从csv中读取参数获取赋值
    float translationSpeed = 8f;
    float drapSpeed = 3f;
    Vector3 initialPos; //初始位置(诞生点)
    bool forward; //是否往前走
    Vector3 target;
    bool Istranslation;
    bool IsDropDown;

    /*----------------------------------------------------------*/
    private void Awake()
    {
        forward = true;
        Istranslation = true;
        IsDropDown = false;
        initialPos = transform.position;
    }
    private void Update()
    {
        
        if (Input.GetMouseButtonDown(1))
        {
            Istranslation = false;

            target = transform.position + Vector3.down * 0.5f;

            IsDropDown = true;
            //transform.position = Vector3.MoveTowards(transform.position, target, drapSpeed * Time.deltaTime);//向目标位置移动没有缓动
        }
    }
    private void FixedUpdate()
    {
        if (Istranslation)
        {
            TranslationFun();
        }

        if (IsDropDown)
        {
            FallDown();
        }
    }
    void TranslationFun() //平移方法
    {

        if (forward) //如果确定往前走
        {
            transform.Translate(0, 0, Time.deltaTime * translationSpeed); //前进
            if (Vector3.Distance(transform.position,initialPos)>= 20) //前进超过20m
                forward = !forward; //变为后退状态                    
        }
        else //如果往后退
        {
            transform.Translate(0, 0, -Time.deltaTime * translationSpeed); //后退
            if (transform.position.z <= initialPos.z)  //如果退回到初始位置
                forward = !forward; //又往前走
        }
    }

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

    void GamestatusChange()
    {
        //生成边角料 


        //z轴偏差大于自身z轴尺寸,说明没落在顶层上
        if (Mathf.Abs(transform.position.z - GameManager.Instance._curTopBlock.transform.position.z) >= transform.localScale.z)
        {
            //调用动画

            //调用特效

            //游戏结束
            GameManager.Instance.gameOver=true;
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
