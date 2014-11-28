namespace engine {
    public class LearnedSkill {
        public SkillTemplate template;

        public int skillId;
        public int level;

        public float cdTime=1;
        public float cdDuration;

        public void reset(SkillTemplate template, int skillLevel) {
            this.template = template;
            this.level = skillLevel;
            cdTime = template.cd;
        }

    }
}

