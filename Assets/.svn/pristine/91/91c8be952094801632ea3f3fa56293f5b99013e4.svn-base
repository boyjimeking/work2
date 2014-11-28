using UnityEngine;

namespace engine {
    public class SkillState {

        public int skillID;   
        public SkillEffectTemplate temp;
        public FightCharacter attacker;
        public FightCharacter owner;
        
        private float everyTime;
        private float lastTime;
        private int totalNum;
        private int hurtNum;

        public SkillState(SkillEffectTemplate t, FightCharacter o, FightCharacter a) {
            temp = t;
            attacker = a;
            owner = o;
            skillID = temp.id;
            lastTime = Time.realtimeSinceStartup;
            hurtNum = 1;
            totalNum = temp.hurtNum;
            lastTime = temp.lastTime;
            everyTime = lastTime/totalNum;
        }

        public bool canHurt() {
            if (Time.realtimeSinceStartup - lastTime >= everyTime) {
                lastTime = Time.realtimeSinceStartup;
                if (++hurtNum >= totalNum) {
                    removeSkillState();
                }
                return true;
            }
            return false;
        }

        private void removeSkillState() {
            if (owner != null && owner.skillStates.ContainsKey(skillID)) {
                owner.skillStates.Remove(skillID);
            }
        }
    }
}

