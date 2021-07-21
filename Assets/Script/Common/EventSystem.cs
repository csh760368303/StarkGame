using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
public class EventSystem 
{
    //匿名委托封装的事件
    public static UnityAction OnTopBlockChange;
    //当生成新的基座与边角料脚本
    public static UnityAction<GameObject> OnCreateNewFoundation;
}
