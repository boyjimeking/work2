using UnityEngine;
using System.Collections;
using engine;

public class AttackButton :CDButton {
    protected LearnedSkill skill;//bound skill

    //bind a LearnedSkill to this button.
    public void setSkill(LearnedSkill skill) {
        this.skill = skill;
        this.cdDuration = skill.cdDuration;
        this.speed = 1f / cdDuration ;
    }
    public bool fire() {
        if (!completed) return false;
        if (skill != null)
        {
            if (Player.instance.attack(skill.skillId))
            {
                Player.instance.attackx2();
                startCD();
            }
        }
        else
        {
            Player.instance.attackx();
        }
        return true;
    }
   
}
