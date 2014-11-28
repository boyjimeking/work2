using UnityEngine;
using System.Collections;
using System.Xml;

namespace engine {
    //non combo skills can combine into a combo skill template.
    public class SkillTemplate:BaseTemp {
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
