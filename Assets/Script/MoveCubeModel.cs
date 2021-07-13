using System.Collections;
using System.Collections.Generic;
//可移动方块类 可能作为基类 被继承
public class MoveCubeModel
{
    //方块移动速度
    protected float moveSpeed;
    //方块预制体路径
    protected string prefabPath;
    //方块的索引值
    protected int cubeIndex;

    //构造函数 每个方块初始化操作
    public MoveCubeModel()
    {
        
    }
    
    //每个基础的方块都会移动
    void CubeMove(){}

    
    

    //每个方块销毁操作 主要释放空间一类
    ~MoveCubeModel()
    {}
}
