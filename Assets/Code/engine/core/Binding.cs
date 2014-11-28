using UnityEngine;
using System.Collections;
namespace engine {
    public class Binding : MonoBehaviour {
        public object data;
        public bool canTrigger = true;

        void OnTriggerEnter(Collider other) {
            BattleEngine.collider.onTriggerEnter(this, other, false);
        }

        void OnTriggerStay(Collider other) {
            if (data is FightCharacter) {
                BattleEngine.collider.onTriggerEnter(this, other, true);
            }
        }

        //call back by mecanim animation events
        //the binding gameobject should have Animator component.
        void NewEvent(AnimationEvent e) {
            Engine.animEventManager.handle(this, e);
        }

        void OnAnimatorMove() {
            if (data is Character) {
               AvatarController ac= (data as Character).controller;
               if(ac!=null)ac.onAnimatorMove();
            }
        }

        void OnDestory() {
            if (data != null) {
                (data as Character).onDestroy();
            }
        }
       

    }
}

