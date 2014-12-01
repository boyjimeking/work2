using UnityEngine;
using System.Collections;
namespace engine {
    public class Hash {
        //public static string down = "down"; public static int downHash = Animator.StringToHash(down);
        //public static string stand = "up"; public static int standHash = Animator.StringToHash(stand);
        //public static string idle = "idle"; public static int idleHash = Animator.StringToHash(idle);
        //public static string run = "run"; public static int runHash = Animator.StringToHash(run);
        //public static string run2 = "run2"; public static int run2Hash = Animator.StringToHash(run2);
        //public static string wait = "wait"; public static int waitHash = Animator.StringToHash(wait);
        //public static string hit = "hurt"; public static int hurtHash = Animator.StringToHash(hit);
        //public static string die = "die"; public static int dieHash = Animator.StringToHash(die);

        //public static string die2a = "die2a"; public static int die2aHash = Animator.StringToHash(die2a);
        //public static string atk1 = "attack1"; public static int atk1Hash = Animator.StringToHash(atk1);
        //public static string atk2 = "attack2"; public static int atk2Hash = Animator.StringToHash(atk2);
        //public static string atk3 = "attack3"; public static int atk3Hash = Animator.StringToHash(atk3);
        //public static string atk4 = "attack4"; public static int atk4Hash = Animator.StringToHash(atk4);
        //public static string atk5 = "attack5"; public static int atk5Hash = Animator.StringToHash(atk5);
        //public static string atk6 = "attack6"; public static int atk6Hash = Animator.StringToHash(atk6);
        //public static string atk7 = "attack7"; public static int atk7Hash = Animator.StringToHash(atk7);
        //public static string atk8 = "attack8"; public static int atk8Hash = Animator.StringToHash(atk8);
        //public static string ak9 = "attack9"; public static int atk9Hash = Animator.StringToHash(ak9);
        //public static string atk10 = "attack10"; public static int atk10Hash = Animator.StringToHash(atk10);


        ////animator state
        //public static string invalid = "invalid";public static int invalidHash=Animator.StringToHash(invalid);


        public static int cityIdleState = Animator.StringToHash("Base Layer.idle");
        public static int cityRunState = Animator.StringToHash("Base Layer.run");
        public static int runState = Animator.StringToHash("Base Layer.battle_run");
        public static int idleState = Animator.StringToHash("Base Layer.battle_idle");
        public static int dieState = Animator.StringToHash("Base Layer.die");
        public static int hurtState = Animator.StringToHash("Base Layer.hurt");
        public static int downState = Animator.StringToHash("Base Layer.down");
        public static int dizzyState = Animator.StringToHash("Base Layer.dizzy");
        public static int winState = Animator.StringToHash("Base Layer.win");
        public static int appearState = Animator.StringToHash("Base Layer.appear");  

        public static int atk1StartState = Animator.StringToHash("atk.atk1_start");
        public static int atk1State = Animator.StringToHash("atk.atk1");
        public static int atk2State = Animator.StringToHash("atk.atk2");
        public static int atk3State = Animator.StringToHash("atk.atk3");
        public static int atk4State = Animator.StringToHash("atk.atk4");
        public static int atk1EndState = Animator.StringToHash("atk.atk1_end");
        public static int atk2EndState = Animator.StringToHash("atk.atk2_end");
        public static int atk3EndState = Animator.StringToHash("atk.atk3_end");
        public static int atk4EndState = Animator.StringToHash("atk.atk4_end");

        public static int atkEndState = Animator.StringToHash("atk.atk_end");

        //summon state
        public static int summonState = Animator.StringToHash("Base Layer.Summon");

        //skill state
        public static int skill01State = Animator.StringToHash("Base Layer.skill01");
        public static int skill02State = Animator.StringToHash("Base Layer.Skill02");

        public static int monsterAtk1State = Animator.StringToHash("Base Layer.attack1");

        public static int runBool = Animator.StringToHash("RunBool");
        public static int atkTrigger = Animator.StringToHash("AttackTrigger");
        public static int hitdownTrigger = Animator.StringToHash("DownTrigger");
        public static int playHitTrigger = Animator.StringToHash("HurtTrigger");
        public static int dizzyTrigger = Animator.StringToHash("DizzyTrigger");
        public static int winTrigger = Animator.StringToHash("WinTrigger");
        public static int atknextBool = Animator.StringToHash("AtkNextBool");
        public static int dieTrigger = Animator.StringToHash("DieTrigger");
        public static int summonTrigger = Animator.StringToHash("SummonTrigger");//宠物降临
        public static int summonBool = Animator.StringToHash("summonBool");
        
      
        public static int skill1Trigger = Animator.StringToHash("Skill1Trigger");
        public static int skill2Trigger = Animator.StringToHash("Skill2Trigger");

    }



}
