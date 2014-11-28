using UnityEngine;
using System.Collections;
namespace engine
{
    //monster base ai
    public class AI
    {
        public bool enabled;

        public FightCharacter target;
        public Vector3 steeringPosition;
        public bool steering;

        protected FightCharacter owner;
        protected GameObject go;
        protected Transform transform;
        protected CharData data;

        protected float attackTimer;
        protected float attackInterval;

        public virtual void reset(FightCharacter fc)
        {
            this.owner = fc;
            this.data = fc.data;
            this.go = fc.model;
            this.transform = fc.transform;
            fc.ai = this;
            enabled = false;
            attackInterval = 2f;
            target = null;

        }
        public virtual void update()
        {

        }
       

        protected bool tryAttack() {
            if (owner.inSkillAnimation())
                return false;
            LearnedSkill skill = nextSkill();
            if(skill!=null)  return owner.attack(skill.skillId);
            return false;
        }

        public LearnedSkill nextSkill()
        {
            attackTimer += Time.deltaTime;
            if (attackTimer > attackInterval)
            {
                attackTimer = 0;
                return owner.data.getRandomLearnedSkill();

            }

            return null;
        }
        
    }
}

