using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
public class EventSystem 
{
    public static UnityAction OnTopBlockChange;//匿名委托封装的事件

    public static UnityAction<GameObject> OnCreateNewFoundation;//当生成新的基座与边角料脚本
}
