using UnityEngine;
using System.Collections;
namespace engine {
    public class HurtGround : PoolObject{

        ParticleSystem pa;
        public void reset(FightCharacter actor)
        {
            if (go == null)
            {
                go = App.res.createSingle("Local/prefab/effect/hurt_ground");
                pa = go.GetComponentInChildren<ParticleSystem>();
                setGameObject(go);
            }
            else if (pa != null) pa.time = 0;
            go.SetActive(false);
            transform.position = actor.transform.position;
            transform.forward = actor.transform.forward;
            go.SetActive(true);
            completed = false;
        }

        public void update()
        {
            if (completed) return;
            if (pa != null && !pa.isPlaying)
            {
                destroy();
            }
        }

        public void destroy()
        {
            this.completed = true;
            GameObject.Destroy(go);
            go = null;
        }
    }
}

