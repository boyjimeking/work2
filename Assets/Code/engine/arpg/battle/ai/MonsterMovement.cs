using UnityEngine;
using System.Collections;
namespace engine {
    //deal with navmeshagent jittery.
    //the gameobject hierarchy should be:
    // gameobject
    //    proxy
    //    model
    
    public class MonsterMovement : MonoBehaviour {

        protected Transform target;
        protected Transform model;
        protected Transform proxy;

        protected NavMeshAgent agent;
        protected NavMeshObstacle obstacle;

        protected Vector3 lastPosition;

        public void set(Transform target,Transform model,Transform proxy){
            this.target = target;
            this.model = model;
            this.proxy = proxy;
            agent = proxy.GetComponent<NavMeshAgent>();
            if (agent == null) agent = proxy.gameObject.AddComponent<NavMeshAgent>();
            obstacle = proxy.GetComponent<NavMeshObstacle>();
            if (obstacle == null) obstacle = proxy.gameObject.AddComponent<NavMeshObstacle>();
        }

        void Update() {
            if (target == null) return;
            if ((target.position - proxy.position).sqrMagnitude < Mathf.Pow(agent.stoppingDistance,2)) {
                obstacle.enabled = true;
                agent.enabled = false;
            } else {
                obstacle.enabled = false;
                agent.enabled = true;
            }
            model.position = Vector3.Lerp(model.position, proxy.position, Time.deltaTime * 2);
            Vector3 orientation = model.position - lastPosition;
            if (orientation.sqrMagnitude > 0.1f) {
                orientation.y = 0;
                model.rotation = Quaternion.Lerp(model.rotation, Quaternion.LookRotation(model.position - lastPosition), Time.deltaTime * 8);
            } else {
                model.rotation = Quaternion.Lerp(model.rotation,Quaternion.LookRotation(proxy.forward),Time.deltaTime*8);
            }
            lastPosition = model.position;
        }
    }
}


