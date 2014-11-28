using UnityEngine;
using System.Collections;
namespace engine {
    public abstract class AvatarController {
        protected Character f;
        protected GameObject go;
        protected Transform transform;
        protected NavMeshAgent agent;
        protected Animator animator;
        public bool useAnimatorPosition, useAnimatorRotation;

        public  virtual void reset(Character f) {
            this.f = f;
            this.go = f.model;
            this.transform = f.transform;
            this.animator = f.animator;
           
            this.agent = f.agent;
        }
        public  void onAnimatorMove() {
            if (f.isBoss()) {
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.nameHash == Hash.skill02State) {
                    f.transform.position = animator.rootPosition;
                }
                return;
            }
            if (useAnimatorPosition) {
                agent.velocity = animator.deltaPosition / Time.deltaTime;
                // if (useAnimatorRotation) transform.rotation = animator.rootRotation;
            }
           
        }

        public virtual void setTrigger(int v) {
            animator.SetTrigger(v);
        }
        public virtual void resetAttackTrigger() {
           
        }
        public virtual void resetTrigger(int v) {
            animator.ResetTrigger(v);
        }
        public virtual void play(int state, int layer, float time) {
            animator.Play(state, layer,time);
        }
        public void play(int state) {
            animator.Play(state);
        }
        public virtual void setBool(int state, bool v) {
            animator.SetBool(state, v);
        }

    }
}

