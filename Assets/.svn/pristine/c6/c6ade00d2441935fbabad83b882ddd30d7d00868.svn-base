using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace engine {
    public class DefaultResourceManager : IResourceManager {
        protected Hashtable all = new Hashtable();

        protected Dictionary<string, Object> loadedPrefabs = new Dictionary<string, Object>();
        public virtual void init() {
            
        }
        public void clear()
        {
            //foreach (Object o in loadedPrefabs.Values)
            //{
           //     Object.DestroyImmediate(o);
           // }

           // loadedPrefabs.Clear();
        }

        public Object loadObject(string name) {
            if (loadedPrefabs.ContainsKey(name)) {
                return loadedPrefabs[name];
            }
            Object prefab = Resources.Load<Object>(name);
            loadedPrefabs[name] = prefab;
            return prefab;
        }
        public GameObject loadPrefab(string name) {
            if (loadedPrefabs.ContainsKey(name)) {
                return loadedPrefabs[name] as GameObject;
            }
            try {
                GameObject prefab = Resources.Load<GameObject>(name);
                loadedPrefabs[name] = prefab;
                return prefab;
            }catch(System.Exception ex){
                Debug.LogError(ex);
            }
            return null;
           
        }
        public void disposePrefab(string name) {
            if (loadedPrefabs.ContainsKey(name)) {
                Object.Destroy(loadedPrefabs[name]);
                
            }
        }
        public void disposeAllPrefab() {
            foreach(GameObject o in loadedPrefabs.Values){
                Object.Destroy(o);
            }
            loadedPrefabs.Clear();
        }

        public GameObject createSingle(string name)
        {
            GameObject prefab = loadPrefab(name);
            if (prefab == null) return null;
            return Object.Instantiate(prefab) as GameObject;
        }

        public GameObject createObj(string name, Vector3 pos) {
            GameObject prefab = loadPrefab(name);
            if (prefab == null) return null;
            return Object.Instantiate(prefab, pos,Quaternion.identity) as GameObject;
        }
        
        public string loadText(string name) {
           return Resources.Load<TextAsset>(name).text;
        }
        public AudioClip loadSound(string name) {
            return Resources.Load<AudioClip>(name);
        }
        public DefaultResourceManager(BundleManager bundleManager) {
            BundleObjectPool.bundleManager = bundleManager;
        }
        public void addPool(System.Type type, BundleObjectPool pool) {
            all.Add(type, pool);
        }

        public T get<T>(string bundleName, string assetName) where T : PoolObject {
            BundleObjectPool pool = (BundleObjectPool)all[typeof(T)];
            T result = (T)pool.get(bundleName, assetName);
            result.go.SetActive(true);
            result.inPool = false;
            return result;
        }
        //remove item from scene and return to pool
        public void free(PoolObject item) {
            BundleObjectPool pool = (BundleObjectPool)all[item.GetType()];
            if (item == null || item.inPool) return;
            item.inPool = true;
            item.go.SetActive(false);
           // BundleObjectPool pool = (BundleObjectPool)all[item.GetType()];
            pool.free(item);
        }
    }
}

