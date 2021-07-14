using System.Collections;
using System.Collections.Generic;
//可移动方块类 可能作为基类  之后由csv表读取
public class MoveCubeModel
{
    //方块移动速度
    protected float moveSpeed;
    //方块预制体路径
    protected string prefabPath;
    //方块的索引值
    protected int cubeIndex;

    
    //每个基础的方块都会移动
    void CubeMove(){}

    
    

    //每个方块销毁操作 主要释放空间一类
    ~MoveCubeModel()
    {}
}
