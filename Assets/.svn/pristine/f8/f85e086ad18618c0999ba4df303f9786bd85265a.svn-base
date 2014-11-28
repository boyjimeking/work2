using UnityEngine;
using System.Collections;
namespace engine {
    public class CharData {
        public int id;//instance id
        public string name = "闫继昌";
        public CharTemplate charTemplate;
        public CharDataTemplate charDataTemplate;
        public Team team;

        public int hp, maxhp;
       

        public WeaponType primaryWeaponType;//current primary weapon type
        public float weaponRange=1;//current weapon attack distance,used for weapon colliding.
        public float moveRanage = 5;//move to an attackable target if target is within this range


        public int level=1;

      
        //base
        public int AGI = 2;
        public int INT = 2;
        public int STR = 2;


        public int defend;
        

        public float moveSpeed = 1;//current move speed
        public float attackSpeed = 1f;//current attack speed
        public float turnSpeed = 10;//current turn speed

        public int statPoint;

        public void update() {
            //random add stat when level up
            if (statPoint > 0) {
                int d = Random.Range(0, 2);
                switch (d) {
                    case 0: STR += 1; break;
                    case 1: AGI += 1; break;
                }
                statPoint -= 1;
            }
        }


        public virtual void learnSkill(int skillId, int level) {
            //subclass override this
        }

        
        public virtual LearnedSkill getLearnedSkill(int skillId) {
            return null;
        }
        public virtual LearnedSkill getRandomLearnedSkill() {
            return null;
        }
        public virtual LearnedSkill getNormalAttackSkill()
        {
            return null;
        }
        public virtual LearnedSkill[] getAllLearnedSkills() {
            return null;
        }
    }

}
