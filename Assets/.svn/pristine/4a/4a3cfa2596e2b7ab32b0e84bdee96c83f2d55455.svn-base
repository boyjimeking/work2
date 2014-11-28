using UnityEngine;
using System.Collections;
using System.Xml;

namespace engine {
    //basic info about one attack animation,mainly hurt time

    public class AnimConfig : BaseTemp{
       
        public string name;
        public AnimationBlendMode blendMode = AnimationBlendMode.Blend;
        public float speedMulti;//anim speed adjust
        public float moveSpeed;//player move forward when playing animation

        public int nameHash;

        public AudioClip attackSound;
        public AnimationClip clip;

        public override void read(XmlElement e) {
            base.read(e);
            name = e.GetAttribute("name");
            nameHash = Animator.StringToHash(name);

            speedMulti = Utility.toFloat(e.GetAttribute("speed"));
            if (speedMulti == 0) speedMulti = 1;

            moveSpeed = Utility.toFloat(e.GetAttribute("move"));

            string blend = e.GetAttribute("blend");
            if (!string.IsNullOrEmpty(blend)) {
                if (blend == "blend") blendMode = AnimationBlendMode.Blend;
                else if (blend == "add") blendMode = AnimationBlendMode.Additive;
            }
        }
    }

}
