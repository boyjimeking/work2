using UnityEngine;
using System.Collections;
using engine;
using Exploder;
public class ExploderTrigger : MonoBehaviour
{
    //爆炸力度
    public float Force = 2;
    //爆炸影响范围的半径
    public float Radius = 2;
    //爆炸影响其他碎片
    public bool ExplodeFragments = false;
    //碎片数量预算
    public float FrameBudget = 15;
    //碎片目标数量
    public int TargetFragments = 15;
    //炸弹本身爆炸
    public bool ExplodeSelf = true;
    //碎片失效方式 默认超时失效
    public DeactivateOptions DeactivateOptions = DeactivateOptions.DeactivateTimeout;
    //碎片超时时间,DeactivateOptions=DeactivateOptions.DeactivateTimeout 时有效
    public float DeactivateTimeout = 10f;
    //爆炸过后销毁炸弹原型
    public bool DestroyOriginalObject = true;
    // Use this for initialization
    void Start()
    {
        Binding binding = gameObject.addOnce<Binding>();
        ExploderManager.ExploderOptions option = new ExploderManager.ExploderOptions();
        option.Force = Force;
        option.Radius = Radius;
        option.ExplodeFragments = ExplodeFragments;
        option.FrameBudget = FrameBudget;
        option.TargetFragments = TargetFragments;
        option.ExplodeSelf = ExplodeSelf;
        option.DeactivateOptions = DeactivateOptions;
        option.DeactivateTimeout = DeactivateTimeout;
        option.DestroyOriginalObject = DestroyOriginalObject;
        binding.data = option;
    }

}
  
