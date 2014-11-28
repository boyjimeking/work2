using UnityEngine;
using System.Collections;

namespace engine {
    public interface IResourceManager {
        void clear();
        /// <summary>
        /// load some resources from local resources folder. these objects might not be GameObject,such as AnimatorController.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Object loadObject(string name);
        GameObject loadPrefab(string name);
        void disposePrefab(string name);
        void disposeAllPrefab();

        GameObject createSingle(string name);
        GameObject createObj(string name, Vector3 pos);

        string loadText(string name);

        AudioClip loadSound(string name);

        void addPool(System.Type type, BundleObjectPool pool);
        T get<T>(string bundleName, string assetName) where T : PoolObject;
        void free(PoolObject item);
    }
}
