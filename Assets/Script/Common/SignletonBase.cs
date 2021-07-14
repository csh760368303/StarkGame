using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//泛型单例模式  主要用于manager的单例实现
public class SignletonBase<T> : MonoBehaviour where T : SignletonBase<T>
{
    //单例 字段
    private static T instance;

    //单例属性 只读 共访问使用
    public static T Instance
    {
        get { return instance; }
    }

    //虚方法 Awake 可用重写, 主要用保证 单例只存在一个并生成唯一单例
    protected virtual void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = (T)this;
        }
    }
    //判断是否单例实例化
    public static bool IsInitialized
    {
        get
        {
            return instance != null;
        }
    }
    //销毁时释放单例
    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
