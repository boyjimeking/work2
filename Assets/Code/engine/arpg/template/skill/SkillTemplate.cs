using UnityEngine;
using System.Collections;
using System.Xml;

namespace engine {
    //non combo skills can combine into a combo skill template.
    public class SkillTemplate:BaseTemp {
        //special skill flags
        public static int Strength_0 = 0;
        public static int Strength_1 = 1;//施放过程中不会被击退，会被受击、击倒等状态打断
        public static int Strength_2 = 2;//施放过程中不会被击退、受击，会被击倒打断
        public static int Strength_3 = 3;//施放过程中不会被击退、受击、击倒
        public static int Strength_4 = 4;//无法进入眩晕状态

        public static int CommonMonstAttackId = 10000;

        public SkillEffectTemplate effect;

        public float cd=1f;
        public string icon;
        public float atkRange, moveRange;

        public override void read(XmlElement e) {
            base.read(e);
            name = e.GetAttribute("name");
           
            cd = Utility.toFloat(e.GetAttribute("cd"));
            if (cd == 0) cd = 1;
            icon = e.GetAttribute("icon");
            atkRange = Utility.toFloat(e.GetAttribute("atkRange"));
            moveRange = Utility.toFloat(e.GetAttribute("moveRange"));
        }

        public void postRead(Templates manager) {
            effect = manager.getTemp<SkillEffectTemplate>(id);
        }

        public float getCDDuration(int level) {
            return cd;
        }
       
    }

}
