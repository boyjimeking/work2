using UnityEngine;
using System.Collections;
using System.Xml;

namespace engine {
    public enum ColliderType {
       none, effect,collider,bullet,circle,sector,colliderTarget
    }

    public enum BuffType {
        none, dizzy, shakeCamera, netTarget, fetter
    }

    //public enum EffectType {
    //    self, target, followTarget
    //}

    public class SkillEffectTemplate : BaseTemp {
        public SkillTemplate skillTemplate;//for easy access
        public string animName;
        public string hitEffect;//击中目标效果        
        public int triggerHash;//animator controller param name hash value.
        public string[] launchSounds;//skill launch sound effect.
        public string bulletPrefab;//bullet prefab name
        public float bulletDist;
        public float bulletSpeed;
        public float bulletHeight;
        public string collider;//collider prefab
        public float colliderLife;
        public ColliderType colliderType;
        public int[] subEffectIds;//combo,maybe null
        //probability when target is hit
        public float[] knockback;
        public float knockdown;
        public float playHit;
        public float radius;
        public float angle;
        public float lastTime; //hurt continius time
        public int hurtNum;     //hurtting number
        public BuffType[] buffs;
        public float[] buffTimes;
        public int bulletmode;
        public int nextEffect;
        public float cameraShakeCon;//每个动作震屏倍数
        public int strength;

        public SkillEffectTemplate[] effects;//sub effects,when subEffectIds is null,use itself as sub effect.
        public int[] animNameHash;

        private static float[] staticKnockback = new float[2];
        private static float[] staticKnockdown = new float[2];
        public override void read(XmlElement e) {
            base.read(e);
            hitEffect = e.GetAttribute("hitEffect");
            if (string.IsNullOrEmpty(hitEffect)) hitEffect = "hurt";

            string triggerName = e.GetAttribute("animatorTrigger");
            if (!string.IsNullOrEmpty(triggerName))
            {
                triggerHash = Animator.StringToHash(triggerName);
            }


            string[] arr = Utility.toArray(e.GetAttribute("bullet"));
            if (arr != null && arr.Length > 3) {
                bulletPrefab = arr[0];
                bulletDist = Utility.toFloat(arr[1]);
                bulletSpeed = Utility.toFloat(arr[2]);
                bulletHeight = Utility.toFloat(arr[3]);
            }
            string sounds = e.GetAttribute("launchSounds");
            if (!string.IsNullOrEmpty(sounds)) {
                launchSounds = sounds.Split(',');
            }

            collider = e.GetAttribute("collider");
            colliderLife = Utility.toFloat(e.GetAttribute("colliderLife"));
            int ctype = Utility.toInt(e.GetAttribute("colliderType"));
            colliderType = (ColliderType) (ctype + 1);
            knockback = Utility.toFloatArray(e.GetAttribute("knockback"), ',');
            knockdown = Utility.toInt(e.GetAttribute("knockdown"));
            playHit = Utility.toFloat(e.GetAttribute("playHit"));
            radius = Utility.toFloat(e.GetAttribute("radius"));
            angle = Utility.toFloat(e.GetAttribute("angle"));
            lastTime = Utility.toFloat(e.GetAttribute("lastTime"));
            hurtNum = Utility.toInt(e.GetAttribute("hurtNum"));
            int[] allBuffs = Utility.toIntArray(e.GetAttribute("buff"));
            if (allBuffs != null && allBuffs.Length > 0) {
                buffs = new BuffType[allBuffs.Length];
                for (int i = 0; i < allBuffs.Length; i++) {
                    buffs[i] = (BuffType) allBuffs[i];
                }
            }
            buffTimes = Utility.toFloatArray(e.GetAttribute("buffTime"));
            if (knockback == null) knockback = staticKnockback;
             
            string subeffects = e.GetAttribute("subeffects");
            subEffectIds = Utility.toIntArray(subeffects, ',');
            nextEffect = Utility.toInt(e.GetAttribute("nextEffect"));
            bulletmode = Utility.toInt(e.GetAttribute("bulletmode"));
            cameraShakeCon = Utility.toFloat(e.GetAttribute("cameraShakeCon"));
            strength = Utility.toInt(e.GetAttribute("strength"));
        }
        public void postRead(Templates manager) {
            if (subEffectIds == null) {
                effects = new [] { this };
                animNameHash = new [] { Animator.StringToHash(animName)};
            } else {
                int length=subEffectIds.Length;
                effects = new SkillEffectTemplate[length];
                animNameHash=new int[length];
                for (int i = 0; i < subEffectIds.Length; i++) {
                    SkillEffectTemplate template= manager.getTemp<SkillEffectTemplate>(subEffectIds[i]);
                    effects[i] =template;
                    animNameHash[i]=Animator.StringToHash(template.animName);
                }
            }
        }

    }

}
