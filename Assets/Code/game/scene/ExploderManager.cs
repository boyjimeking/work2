using UnityEngine;
using System.Collections;
using Exploder;
using engine;

public class ExploderManager {

    public class ExploderOptions { 
        //爆炸力度
        public float Force = 2; 
        //爆炸影响范围的半径
        public float Radius = 2;
        //爆炸影响其他碎片
        public bool ExplodeFragments = false;
        //碎片数量预算
        public float FrameBudget = 200;
        //碎片目标数量
        public int TargetFragments = 200;
        //炸弹本身爆炸
        public bool ExplodeSelf = true;
        //碎片失效方式 默认超时失效
        public DeactivateOptions DeactivateOptions = DeactivateOptions.DeactivateTimeout;
        //碎片超时时间,DeactivateOptions=DeactivateOptions.DeactivateTimeout 时有效
        public float DeactivateTimeout = 10f;
        //爆炸过后销毁炸弹原型
        public bool DestroyOriginalObject = true;
        //回调
        public ExploderObject.OnExplosion callback=null;

        public bool explodered = false;

    }

    public static ExploderManager instance = new ExploderManager();
    public ExploderOptions defaultOptions = new ExploderOptions();
    public void exploder(GameObject go, ExploderOptions options)
    {
        ExploderObject exploder = go.addOnce<ExploderObject>();
        exploder.Force = options.Force;
        exploder.Radius = options.Radius;
        exploder.ExplodeFragments = options.ExplodeFragments;
        exploder.FrameBudget = options.FrameBudget;
        exploder.TargetFragments = options.TargetFragments;
        exploder.ExplodeSelf = options.ExplodeSelf;
        exploder.DeactivateOptions = options.DeactivateOptions;
        exploder.DeactivateTimeout = options.DeactivateTimeout;
        exploder.DestroyOriginalObject = options.DestroyOriginalObject;
        exploder.Explode();
    }

    public void exploderDefault(GameObject go) {
        exploder(go, defaultOptions);
    }


}
