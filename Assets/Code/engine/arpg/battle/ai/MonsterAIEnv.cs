using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace engine {
    public class MonsterAIEnv {
       
        private float neighborDistance = 8f;
        private float separationDistance = 2f;

        private List<FightCharacter> enemies;
        private int length;

        public void reset() {

        }
        public void update() {
            
            //enemies = BattleEngine.scene.getEnemies();
            //length = enemies.Count;
            //for (int i = 0; i < length; i++) {
            //    FightCharacter c = enemies[i];
            //    c.ai.steering = false;
            //    if (c.ai.target!=null&&!c.ai.targetInAttackRange()) {
            //        determineSeparation(c, i);
            //    } 
            //}
        }
        private void determineSeparation(FightCharacter c, int agentIndex) {
            var separation = Vector3.zero;
            int neighborCount = 0;
            for (int i = 0; i < length; ++i) {
                if (agentIndex != i) {
                    if (Vector3.SqrMagnitude(enemies[i].transform.position - c.transform.position) < neighborDistance) {
                        separation += enemies[i].transform.position - c.transform.position;
                        neighborCount++;
                    }
                }
            }

            if (neighborCount == 0) {
                c.ai.steering = false;
            } else {
                c.ai.steering = true;
                c.ai.steeringPosition= c.ai.target.transform.position+ ((separation / neighborCount) * -1).normalized * separationDistance;
            }

        }

    }
}


