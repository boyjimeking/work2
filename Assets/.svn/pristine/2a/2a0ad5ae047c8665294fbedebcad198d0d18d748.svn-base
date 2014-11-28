using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace engine {
    public class MonsterSpawner : MonoBehaviour {
        protected Transform mTransform;
        protected float spawnRadius=10f;
        void Start() {
            mTransform = transform;
        }
        //choose a valid random position for the gameobject.
        public void setPosition(GameObject go, float radius) {
           
            Vector3 temp = mTransform.position;
            Vector2 random2d = Random.insideUnitCircle * radius;
            temp.Set(mTransform.position.x + random2d.x, mTransform.position.y, mTransform.position.z + random2d.y);
            NavMeshHit hit;
            if (NavMesh.SamplePosition(temp, out hit, 100, 1)) {
                temp = hit.position;
            }
            go.transform.position = temp;
        }
        public GameObject spawn(GameObject prefab, float radius) {
            Vector3 temp = mTransform.position;
            Vector2 random2d = Random.insideUnitCircle * radius;
            temp.Set(mTransform.position.x + random2d.x, mTransform.position.y, mTransform.position.z + random2d.y);
            NavMeshHit hit;
            if (NavMesh.SamplePosition(temp, out hit, 100, 1)) {
                temp = hit.position;
            }
            GameObject monster = GameObject.Instantiate(prefab, temp, mTransform.rotation) as GameObject;
            monster.AddComponent<MonsterMovement>();
            return monster;
        }
        public List<GameObject> spawn(GameObject prefab, float radius, int count) {
            List<GameObject> result = new List<GameObject>();
            Vector3 temp=mTransform.position;
            BundleManager bundle = App.bundle;
            for (int i = 0; i < count; i++) {
                Vector2 random2d = Random.insideUnitCircle * radius;
                temp.Set(mTransform.position.x+random2d.x,mTransform.position.y,mTransform.position.z+random2d.y);
                NavMeshHit hit;
                if (NavMesh.SamplePosition(temp, out hit, 100, 1)) {
                    temp = hit.position;
                }
                GameObject monster = GameObject.Instantiate(prefab, temp, mTransform.rotation) as GameObject;
                monster.AddComponent<MonsterMovement>();
                result.Add(monster);
            }
            return result;
        }

        void OnDrawGizmosSelected() {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, spawnRadius);
        }
       
    }

}
