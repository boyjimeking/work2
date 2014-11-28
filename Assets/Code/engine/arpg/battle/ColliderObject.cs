using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace engine {
    public class ColliderObject : PoolObject {
        private static int colliderId;

        public int id;
        public FightCharacter owner;
        public LearnedSkill skill;
        public SkillEffectTemplate effect;
        public string resName;

        private HashSet<FightCharacter> hitObjects;
        public void addHit(FightCharacter c) {
            if (hitObjects == null) hitObjects = new HashSet<FightCharacter>();
            hitObjects.Add(c);
        }
        public void reset(FightCharacter owner, LearnedSkill skill, SkillEffectTemplate effect,string resName) {
            this.owner = owner;
            this.skill = skill;
            this.effect = effect;
            this.id = colliderId++;
            this.resName = resName;

        }

        public void onDestroy() {
            if (hitObjects != null) {
                foreach (FightCharacter c in hitObjects) {
                    c.removeHit(this);
                }
                hitObjects.Clear();
            }
            BattleEngine.freeColliderObject(this);
        }
    }
}

