using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//物体加载回调
public delegate void GameObjectLoadedCallBack(GameObject obj);

/// <summary>
/// UI层级
/// </summary>
public enum UILayerType
{
    Top,
    Upper,
    Normal,
    Hud
}
/// <summary>
/// UI加载方式
/// </summary>
public enum UILoadType
{
    SyncLoad,
    AsyncLoad,
}



public abstract class UIBase 
{
    public static string UIMain_Path = "Assets/Resources/UIPrefabs";

    #region /*-----------------------Reference基本属性----------------------------*/
    /// <summary>
    /// 是否加载完成的标志位
    /// </summary>
    protected bool isInited;
    /// <summary>
    /// UI名称
    /// </summary>
    private string uiName;
    /// <summary>
    /// 在关闭的时候是否缓存UI
    /// </summary>
    protected bool isCatchUI = false;
    /// <summary>
    /// UI实例化的GameObject
    /// </summary>
    protected GameObject uiGameObject;
    /// <summary>
    /// UI是否可见 默认为不可见
    /// </summary>
    protected bool active = false;
    /// <summary>
    /// 加载完成回调函数
    /// </summary>
    protected GameObjectLoadedCallBack ui_callBack;
    /// <summary>
    /// UI资源全路径 
    /// </summary>
    protected string uiFullPath = string.Empty;
    /// <summary>
    /// UI层类型
    /// </summary>
    protected UILayerType uiLayerType;
    /// <summary>
    /// UI加载方式
    /// </summary>
    protected UILoadType uiLoadType;


    #endregion
    #region /*---------------------------property封装字段-------------------------*/
    public string UIName
    {
        get
        {
            return uiName;
        }
        set
        {
            uiName = value.EndsWith("_uiPrefab") ? value : value + "_uiPrefab";
            uiFullPath = Path.Combine(UIMain_Path, uiName);
        }
    }

    public bool ISCatchUI
    {
        get => isCatchUI;
        set => isCatchUI = value;
    }
    public GameObject UIGameObject
    {
        get => uiGameObject;
        set => uiGameObject = value;
    }
    public bool Active
    {
        get => active;
        set
        {
            active = value;
            if (uiGameObject!=null)
            {
                uiGameObject.SetActive(value);
                if (uiGameObject.activeSelf)
                {
                    OnActive();
                }
                else
                {
                    OnDeActive();
                }
            }
        }
    }
    public bool IsInited { get => isInited; }
    #endregion
    #region /*---------------------------Method方法-------------------------*/
    protected UIBase(string uiName, UILayerType layerType, UILoadType loadType = UILoadType.SyncLoad)
    {
        UIName = uiName;
        uiLayerType = layerType;
        uiLoadType = loadType;
    }
    /// <summary>
    /// 之后 会根据加载 模块实现实例化   加入池概念
    /// </summary>
    public virtual void Init()
    {
        //if (uiLoadType == UILoadType.SyncLoad)
        //{
        //    GameObject go = MObjectManager.singleton.InstantiateGameObeject(m_uiFullPath);
        //    OnGameObjectLoaded(go);
        //}
        //else
        //{
        //    MObjectManager.singleton.InstantiateGameObejectAsync(m_uiFullPath, delegate (string resPath, MResourceObjectItem mResourceObjectItem, object[] parms)
        //    {
        //        GameObject go = mResourceObjectItem.m_cloneObeject;
        //        OnGameObjectLoaded(go);
        //    }, LoadResPriority.RES_LOAD_LEVEL_HEIGHT);
        //}
        GameObject go = GameManager.Instance.UIIwant; //这里暂时由 gamemanager 获取,之后会写入到加载管理器中的, 通过路径加载这个UI资源
        OnGameObjectLoaded(go);
    }
    public virtual void Uninit()
    {
        isInited = false;
        active = false;
        if (isCatchUI)
        {
            //资源并加入到资源池
            //MObjectManager.singleton.ReleaseObject(uiGameObject);
        }
        else
        {
            //彻底清除Object资源
            //MObjectManager.singleton.ReleaseObject(uiGameObject, 0, true);
        }
    }
    void OnGameObjectLoaded(GameObject uiObj)
    {
        if (uiObj == null)
        {
            Debug.Log($"UI加载失败了老铁~看看路径 ResPath: {uiFullPath}"); 
            return;
        }
        uiGameObject = uiObj;
        isInited = true;
        SetPanetByLayerType(uiLayerType);
        uiGameObject.transform.localPosition = Vector3.zero;
        uiGameObject.transform.localScale = Vector3.one;
    }


    protected void SetPanetByLayerType(UILayerType layerType)
    {
        switch (uiLayerType)
        {
            case UILayerType.Top:
                uiGameObject.transform.SetParent(UIManager.Instance.TransTop);
                break;
            case UILayerType.Upper:
                uiGameObject.transform.SetParent(UIManager.Instance.TransUpper);
                break;
            case UILayerType.Normal:
                uiGameObject.transform.SetParent(UIManager.Instance.TransNormal);
                break;
            case UILayerType.Hud:
                uiGameObject.transform.SetParent(UIManager.Instance.TransHUD);
                break;
        }
    }

    protected abstract void OnActive();

    protected abstract void OnDeActive();

    public virtual void Update(float deltaTime)
    {

    }
    public virtual void LateUpdate(float deltaTime)
    {

    }
    public virtual void OnLogOut()
    {

    }
    #endregion
}
