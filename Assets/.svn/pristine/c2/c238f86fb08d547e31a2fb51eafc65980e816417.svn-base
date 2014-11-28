using UnityEngine;
using System.Collections;

namespace engine {
    public class BattleEngine {

        public static BattleScene scene;//current active battle scene.

        //public static IBattleFactory factory;
        
        public static Templates template;
        public static IAttackController controller;
        public static DefaultEffectManager effect;

        public static IColliderManager collider;

        public static int atkNo;//identify each attack,auto incremental after each attack


        public static PetAIEnv petEnv;
        public static MonsterAIEnv monsterEnv;

        public static bool dungeonWin;
        public static bool battleEnded;//the dungeon battle is ended, friend side or boss side died.


        //vo pools/////////////////////////
        private static Pool<ColliderObject> colliderObjects=new Pool<ColliderObject>();
        public static ColliderObject getColliderObject(){
            return colliderObjects.get();
        }
        public static void freeColliderObject(ColliderObject item) {
            colliderObjects.free(item);
        }


        public static void reset() {
            dungeonWin = false;
            battleEnded = false;
            if (petEnv != null) petEnv.reset();
            if (monsterEnv != null) monsterEnv.reset();
        }
    }

}
