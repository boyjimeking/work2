using UnityEngine;
using System.Collections;

namespace engine
{
    public class Stats
    {
        private FightCharacter owner;
        private CharDataTemplate template;

        public int MAXHP;
        public int HP;
        public int ATT;
        public int DEF;
        public float EVA;
        public float CRI;
        public float BeHit;
        public float BeDown;
        public float BeatBack;
        public float BackDistance;

        public void reset(FightCharacter owner)
        {
            this.owner = owner;
            template = BattleEngine.template.getTemp<CharDataTemplate>(owner.charTemplate.id);
            HP = template.HP;
            ATT = template.ATT;
            DEF = template.DEF;
            EVA = template.EVA;
            CRI = template.CRI;
            BeHit = template.BeHit;
            BeDown = template.BeDown;
            BeatBack = template.BeatBack;
            BackDistance = template.BackDistance;

            MAXHP = HP;
        }


    }
}
