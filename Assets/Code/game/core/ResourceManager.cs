using UnityEngine;
using System.Collections;
using engine;

public class ResourceManager : DefaultResourceManager {
    public ResourceManager(BundleManager bundle):base(bundle){
        
    }
    public override void init() {
        base.init();

        addPool(typeof(engine.Bullet), new BundleObjectPool(newBullet));
    }

    private engine.Bullet newBullet() {
        return new engine.Bullet();
    }

    
}
