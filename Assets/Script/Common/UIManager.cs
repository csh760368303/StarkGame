using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SignletonBase<UIManager>
{
    /// <summary>
    /// 核心的管理所有UI的Dictionary
    /// </summary>
    private Dictionary<string, UIBase> m_uiDict;



    public Transform TransTop;
    public Transform TransUpper;
    public Transform TransNormal;
    public Transform TransHUD;
}
