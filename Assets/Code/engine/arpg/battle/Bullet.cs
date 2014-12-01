using UnityEngine;
using System.Collections;
namespace engine {
    public class Bullet : PoolObject {
        public FightCharacter owner;

        public int damage;

        protected GameObject explosiveObject;
        protected float life = 5;
        protected float speed = 8f;
        protected float currentLife;
        protected Rigidbody body;
        public SkillEffectTemplate temp;

        protected Vector3 direction;

        public BoxCollider[] colliders;

        public void reset(SkillEffectTemplate temp, FightCharacter actor) {
            this.temp = temp;
            go = App.res.createSingle("Local/prefab/collider/" + temp.bulletPrefab);
            setGameObject(go);
            speed = temp.bulletSpeed;
            life = temp.bulletDist/temp.bulletSpeed;
            reset(actor,temp.bulletHeight);
        }

        public void reset(FightCharacter actor,float yoffset=0) {
            this.owner = actor;
            go.SetActive(false);
            transform.position = actor.transform.position + new Vector3(0, yoffset, 0);
            transform.forward = actor.transform.forward;
            go.SetActive(true);
            currentLife = 0;      
            colliders = go.GetComponentsInChildren<BoxCollider>();
            if (colliders != null)
            {
                foreach (BoxCollider c in colliders)
                {
                    c.isTrigger = true;
                    c.gameObject.addOnce<Binding>().data = this;
                    //onTriggerEnter not reliable for fast moving objects like bullet
                    //TODO opt
                    Rigidbody body = c.gameObject.addOnce<Rigidbody>();
                    body.useGravity = false;
                    body.collisionDetectionMode = CollisionDetectionMode.Continuous;
                }
            }
            else
            {
                Collider collider = go.GetComponent<Collider>();
                if (collider == null)
                {
                    collider = go.AddComponent<BoxCollider>();
                }
                collider.gameObject.addOnce<Binding>().data = this;
                collider.isTrigger = true;
            }
           
            
        }
        public void update() {
            if (completed) return;
            if (currentLife >= life) {
                destroy();
            } else
            {
                if (body != null) {
                    body.velocity = transform.forward * speed;
                } else {
                    transform.position+= transform.forward * speed * Time.deltaTime;
                }
                currentLife += Time.deltaTime;
            }
        }
       
        public void destroy() {
          this.completed = true;
          GameObject.Destroy(go);
            if (temp.nextEffect > 0) {
                SkillEffectTemplate nextTemp = App.template.getTemp<SkillEffectTemplate>(temp.nextEffect);
                if (nextTemp != null) {
                    GameObject fxEff = App.res.createObj("Local/prefab/effect/" + nextTemp.collider, transform.position);
                    if (fxEff == null) {
                        Debug.LogError("can't load effect:" + nextTemp.collider);
                        return;
                    }
                    Object.Destroy(fxEff, nextTemp.lastTime);
                    App.animEventManager.calResult(nextTemp, owner.model.GetComponent<Binding>());
                }
            }
          // Engine.res.free(this);
        }
    }
}

