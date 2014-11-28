using UnityEngine;
using System.Collections;

namespace engine{
    public class CharDataTemplate : BaseTemp {
        public CharacterType type;
        public int HP;
        public int ATT;
        public int DEF;
        public float EVA;
        public float CRI;
        public float BeHit;
        public float BeDown;
        public float BeatBack;
        public float BackDistance;

        public override void read(System.Xml.XmlElement e)
        {
            base.read(e);
            name = e.GetAttribute("name");

            type = (CharacterType)Utility.toInt(e.GetAttribute("type"));
            HP = Utility.toInt(e.GetAttribute("HP"));
            ATT = Utility.toInt(e.GetAttribute("ATT"));
            DEF = Utility.toInt(e.GetAttribute("DEF"));
            EVA = Utility.toFloat(e.GetAttribute("EVA"));
            CRI = Utility.toFloat(e.GetAttribute("CRI"));
            BeHit = Utility.toFloat(e.GetAttribute("BeHit"));
            BeDown = Utility.toFloat(e.GetAttribute("BeDown"));
            BeatBack = Utility.toFloat(e.GetAttribute("BeatBack"));
            BackDistance = Utility.toFloat(e.GetAttribute("BackDistance"));
        }

    }
}
