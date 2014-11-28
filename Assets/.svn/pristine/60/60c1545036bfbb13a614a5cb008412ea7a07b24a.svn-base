using UnityEngine;
using System.Collections;
using System.Xml;
namespace engine {
    public class CharAnimation:BaseTemp {
        public AnimConfig[] animations;
        public int[] animNameHash;

        public float minAnimSpeed = 1;
        public float maxAnimSpeed = 1;

        public AnimConfig hurt;//transient

        public override void read(XmlElement e) {
            base.read(e);
            XmlNodeList list= e.ChildNodes;
            int length = list.Count;
            animations = new AnimConfig[length];
            animNameHash = new int[length];
            for (int i = 0; i < length; i++) {
                AnimConfig anim = new AnimConfig();
                anim.read(list.Item(i) as XmlElement);
                animations[i] = anim;
                animNameHash[i] = anim.nameHash;
            }

            minAnimSpeed = Utility.toFloat(e.GetAttribute("minAnimSpeed"));
            maxAnimSpeed = Utility.toFloat(e.GetAttribute("maxAnimSpeed"));
            if (minAnimSpeed == 0) minAnimSpeed = 1;
            if (maxAnimSpeed == 0) maxAnimSpeed = 1;
        }
        public void postRead(Templates manager) {
            CharTemplate template = manager.getTemp<CharTemplate>(id);
            template.animConfig = this;
        }
        public AnimConfig get(int nameHash) {
            for (int i = animNameHash.Length - 1; i > -1; i--) {
                if (animNameHash[i] == nameHash) return animations[i];
            }
            return null;
        }
        public AnimConfig getHurt(string name) {
            if (hurt != null) return hurt;
            foreach (AnimConfig anim in animations) {
                if (anim.name == name) {
                    hurt = anim;
                    break;
                }
            }
            return hurt;
        }

    }

}
