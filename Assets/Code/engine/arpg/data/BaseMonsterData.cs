using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace engine {
    public class BaseMonsterData : CharData {
        public LearnedSkill[] learnedSkills;

        private static List<LearnedSkill> options = new List<LearnedSkill>();

        public override void learnSkill(int skillId, int level) {
            LearnedSkill current = getLearnedSkill(skillId);
            if (current != null) {
                current.level = level;
            } else {
                SkillTemplate template = BattleEngine.template.getTemp<SkillTemplate>(skillId);
                current = new LearnedSkill();
                current.skillId = skillId;
                current.template = template;
                current.level = level;
                learnedSkills = Utility.add(learnedSkills, current);
            }
            current.cdDuration = current.template.getCDDuration(current.level);
        }
        public override LearnedSkill getRandomLearnedSkill() {
            options.Clear();
            float now=Time.time;
            for (int i = 0, max = learnedSkills.Length; i < max; i++) {
                LearnedSkill skill = learnedSkills[i];
                if (skill.cdTime + skill.template.cd < now) {
                    options.Add(skill);
                }
            }
            if (options.Count == 0) return null;
            return options[Random.Range(0, options.Count)];

            
        }
       
        public override LearnedSkill getLearnedSkill(int skillId) {
            if (learnedSkills == null) return null;
            for (int i = learnedSkills.Length - 1; i > -1; i--) {
                if (learnedSkills[i].skillId == skillId) return learnedSkills[i];
            }
            return null;
        }
        public override LearnedSkill[] getAllLearnedSkills()
        {
            return learnedSkills;
        }
       
    }
}

