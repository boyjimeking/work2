using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace engine {
    public class PetAIEnv {
        //public FightCharacter leastHpChar;//the pet who has least absolute hp value
        //public FightCharacter leastHpPercentChar;//the pet who has least hp/maxhp value
        //private float leastHPPercent=1;
        //public FightCharacter mostHpChar;//the pet who has most absolute hp


        public FightCharacter fleePet;//the current pet in flee session.
        public FightCharacter helpPet;//the pet who is trying helping the flee pet.

        public void reset() {
            fleePet = helpPet = null;
        }

        public void afterAttacked(FightCharacter pet) {
            PetAI ai = pet.ai as PetAI;
            ai.afterAttacked();
        }

        public void update() {

            //List<FightCharacter> friends = BattleEngine.scene.getFriends();
            //int length = friends.Count;
            //int hp,maxHp;
            //for (int i = 0; i < length; i++) {
            //    FightCharacter c = friends[i];
            //    hp=c.stats.HP;
            //    maxHp=c.stats.MAXHP;

            //    if (leastHpChar == null) leastHpChar = c;
            //    else if (leastHpChar.stats.HP > hp) leastHpChar = c;

            //    float percent = hp * 1.0f / maxHp;
            //    if (percent < leastHPPercent) leastHPPercent = percent;

            //    if (mostHpChar == null) mostHpChar = c;
            //    else if (mostHpChar.stats.HP < hp) mostHpChar = c;
            //}
        }
        //public FightCharacter findFriendThatNeedHelp(FightCharacter idle) {
        //    FightCharacter target;
        //    if (leastHpPercentChar != null&&leastHpPercentChar!=idle) {
        //        if (leastHpPercentChar.ai != null) {
        //            target = leastHpPercentChar.ai.target;
        //            if (target != null && !target.isDead()) {
        //                if (target.stats.HP > leastHpPercentChar.stats.HP) {
        //                    return leastHpPercentChar;
        //                }
        //            }
        //        }
               
        //    }
        //    if (leastHpChar != null && leastHpChar != idle) {
        //        if (leastHpChar.ai != null) {
        //            target = leastHpChar.ai.target;
        //            if (target != null && !target.isDead()) {
        //                if (target.stats.HP > leastHpChar.stats.HP) {
        //                    return leastHpChar;
        //                }
        //            }
        //        }
        //    }

        //    //if any friend is fighting,go to help 
        //    List<FightCharacter> friends = BattleEngine.scene.getFriends();
        //    foreach (FightCharacter c in friends) {
        //        if (c != idle&&c.ai!=null) {
        //            if (c.ai.target != null&&!c.ai.target.isDead()) return c;
        //        }
        //    }
        //    return null;
        //}
    }
}

