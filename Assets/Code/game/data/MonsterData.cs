using UnityEngine;
using System.Collections;
using engine;
public class MonsterData : BaseMonsterData {
    private static int instanceId;
    public static MonsterData create(Templates templates, int templateId) {
        CharTemplate template = templates.getTemp<CharTemplate>(templateId);
        CharDataTemplate charDataTemplate = templates.getTemp<CharDataTemplate>(templateId);
        MonsterData data = new MonsterData();
        data.id = ++instanceId;
        data.charTemplate = template;
        data.charDataTemplate = charDataTemplate;

        data.hp = data.maxhp =  data.charDataTemplate.HP;

        data.moveSpeed = template.moveSpeed;
        data.moveRanage = template.moveRange;
        data.weaponRange = template.weaponRange;

        //init monster skills if any
        int[] skills = template.skills;
        if (skills != null) {
            LearnedSkill[] lskills = new LearnedSkill[skills.Length];
            for (int i = 0, max = skills.Length; i < max; i++) {
                LearnedSkill skill = new LearnedSkill();
                skill.skillId = skills[i];
                skill.level = 1;
                skill.template = templates.getTemp<SkillTemplate>(skills[i]);
                lskills[i] = skill;
            }
            data.learnedSkills = lskills;
        }

        return data;
    }
    //public static MonsterData from(MonsterInfo info) {
    //    ITemplateManager templates = BattleEngine.template;
    //    CharTemplate template=templates.chart(info.templateId);
    //    MonsterData data = new MonsterData();
    //    data.id = info.instanceId;
    //    data.charTemplate = template;

    //    Health health = new Health();
    //    data.health = health;

    //   // data.hp = info.hp;//TODO restore this

    //    data.moveSpeed = template.moveSpeed;
    //    data.moveRanage = template.moveRange;
    //    data.weaponRange = template.weaponRange;

    //    //init monster skills if any
    //    int[] skills = template.skills;
    //    if (skills != null) {
    //        LearnedSkill[] lskills = new LearnedSkill[skills.Length];
    //        for (int i = 0, max = skills.Length; i < max; i++) {
    //            LearnedSkill skill = new LearnedSkill();
    //            skill.skillId = skills[i];
    //            skill.level = 1;
    //            skill.template = templates.skill(skills[i]);
    //            lskills[i] = skill;
    //        }
    //        data.learnedSkills = lskills;
    //    }

    //    return data;
    //}
}
