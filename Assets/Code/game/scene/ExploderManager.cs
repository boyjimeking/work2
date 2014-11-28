using UnityEngine;
using System.Collections;
using Exploder;
using engine;

public class ExploderManager {

    public class ExploderOptions { 
        public float Force = 2;
        public float Radius = 2;
        public bool ExplodeFragments = false;
        public float FrameBudget = 200;
        public int TargetFragments = 200;
        public bool ExplodeSelf = true;
        public DeactivateOptions DeactivateOptions = DeactivateOptions.DeactivateTimeout;
        public float DeactivateTimeout = 10f;
        public ExploderObject.OnExplosion callback=null;
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
        exploder.Explode();
    }

    public void exploderDefault(GameObject go) {
        exploder(go, defaultOptions);
    }



}
