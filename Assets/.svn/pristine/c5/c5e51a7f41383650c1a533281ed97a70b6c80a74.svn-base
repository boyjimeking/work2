using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace engine {
    public enum charactertype {
        player, hero, monster, otherplayer, boss
    }

    public class CharTemplate :BaseTemp{
        public string model;
        public string controller;//animator controller
        public string summonEffect;//pet summon effect
        public float summonDelay;//play summon trigger after some time of summon effect.
        public float summonDissolveSpeed;//reverse dissolve speed

        public CharacterType type;

        public float rushRange;
        public float moveSpeed;
        public float angularSpeed;
        public float weaponRange=1f;
        public float moveRange = 3f;//if any target is within moveRange,try to move to the target.
        public int summonSkill;

        public int[] skills;//monster available skills

        //the sound effect when being hit,if length>1, a randomly selected sound will be used.
        public string[] hitSounds;
       
        public string deadEffect;//effect when dead
        public string hitEffect;//effect when being hit
        //optional end/////////////


        public CharAnimation animConfig;
        public float randomAnimSpeed = 1;


        

        public override void read(XmlElement e) {
            base.read(e);
            type = (CharacterType)Utility.toInt(e.GetAttribute("type"));
            name = e.GetAttribute("name");
            model = e.GetAttribute("model");
            controller = e.GetAttribute("controller");
            summonEffect = e.GetAttribute("summon");
            summonDelay = Utility.toFloat(e.GetAttribute("summonDelay"));
            summonDissolveSpeed = Utility.toFloat(e.GetAttribute("summonDissolveSpeed"));


            rushRange = Utility.toFloat(e.GetAttribute("rushRange"));
            if (rushRange == 0) rushRange = 10f;

            moveSpeed = Utility.toFloat(e.GetAttribute("moveSpeed"));
            if (moveSpeed == 0) moveSpeed = 1;
            angularSpeed = Utility.toFloat(e.GetAttribute("angularSpeed"));
            if (angularSpeed == 0) angularSpeed = 150;
           

            skills = Utility.toIntArray(e.GetAttribute("skills"), ',');

            weaponRange = Utility.toFloat(e.GetAttribute("weaponRange"));
            if (weaponRange == 0) weaponRange = 1f;
            moveRange = Utility.toFloat(e.GetAttribute("moveRange"));
            if (moveRange == 0) moveRange = 5f;
            if (weaponRange > moveRange) weaponRange = moveRange;
            summonSkill = Utility.toInt(e.GetAttribute("summonSkill"));
        }

        public float randomizeAnimSpeed() {
            return randomAnimSpeed = Random.Range(animConfig.minAnimSpeed, animConfig.maxAnimSpeed);
        }


        public AnimConfig getAnimConfig(int nameHash) {
            return animConfig.get(nameHash);
        }
        public AnimConfig getHurtAnimConfig(string name) {
            return animConfig.getHurt(name);
        }
    }

}
