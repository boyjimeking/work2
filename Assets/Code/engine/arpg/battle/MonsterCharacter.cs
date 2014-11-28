using UnityEngine;
using System.Collections;

namespace engine {
    public class MonsterCharacter : FightCharacter {
        protected NavMeshObstacle obstacle;

        protected DissolveEffect dissolveEffect;

        public GameObject goldModel;
        public bool canPickup = false;


        public override void setObstacleMode(bool value) {
            if (agent.enabled == !value) return;
            base.setObstacleMode(value);
            obstacle.enabled = value;
        }

        public override bool inSkillAnimation() {
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            if (info.nameHash == Hash.skill02State || info.nameHash == Hash.skill01State)
                return true;              
            return false;
        }
      
        public override void applyDamage(AttackResult attackResult, FightCharacter attacker)
        {
            base.applyDamage(attackResult, attacker);
            if (isBoss()) {
                BossHead.instance.updateBlood();
            }
        }

        public override void reset(GameObject go, CharData data, AI ai) {
            base.reset(go, data, ai);
            obstacle = go.addOnce<NavMeshObstacle>();
            //obstacle.radius = agent.radius;
            setObstacleMode(true);
           

        }
        public override void updateDead() {
            base.updateDead();
            if (dissolveEffect == null) {
                dissolveEffect = new DissolveEffect();
                dissolveEffect.reset(this, 4f);
            }
            if (!destroyed) dissolveEffect.update();
        }
        protected override void onDead()
        {
            base.onDead();
            if (Player.instance.autoFight)
                (Player.instance.controller as HeroController).attackx(false);
            if (isBoss())
            {
                BossHead.instance.hide();
                if (BattleEngine.scene.getEnemies().Count > 0)
                {
                    foreach (FightCharacter c in BattleEngine.scene.getEnemies())
                    {
                        if (!c.isBoss())
                        {
                            c.forceDie();
                        }
                    }
                }
                Time.timeScale = CommonTemp.bossDieScale;
                CameraManager.CameraFollow.target = this.transform;
                App.coroutine.StartCoroutine(recoverTime());
                BattleUI.instance.setEnabled(false);
                App.input.setJoyEnabled(false);
                App.coroutine.StartCoroutine(playWin());
            }
            else {
                DropManager.instance.dropGold(transform, Random.Range(1,5));
            }
        }
        protected IEnumerator playWin()
        {
            yield return new WaitForSeconds(2f);
            if (BattleEngine.scene.getFriends().Count > 1) {
                foreach (FightCharacter c in BattleEngine.scene.getFriends()) {
                    if (!c.isPlayer()) {
                        c.ai.enabled = false; //disable player, pet AI action
                        c.setAgentEnable(false);
                    }
                }
            }
            GameObject obj = new GameObject();
            MonoBehaviour mono = obj.AddComponent<MonoBehaviour>();
            mono.StartCoroutine(win(obj));
       
        }

        private IEnumerator recoverTime() {
            yield return new WaitForSeconds(CommonTemp.bossScaleTime);
            Time.timeScale = 1f;
        }

        private IEnumerator win(GameObject obj) {
            yield return new WaitForSeconds(2f);
            BattleWin win = Player.instance.model.AddComponent<BattleWin>();
            win.onBegin();
            Object.Destroy(obj);
        }

        protected override void updateAnimatorState() {
            base.updateAnimatorState();
            int state = stateInfo.nameHash;

            isAttacking = !isHitDown && !isHitBack && (state == Hash.monsterAtk1State || state == Hash.skill01State || state == Hash.skill02State);
            
            bool useAnimatorTransform =  !(isHitBack||isPlayHit||isFreezing||isAttacking|| isDying||state == Hash.runState || state == Hash.idleState);
            controller.useAnimatorPosition =isHitDown|| useAnimatorTransform;
            controller.useAnimatorRotation = useAnimatorTransform;
           

            if (isAttacking) {
                setObstacleMode(true);
            } else {
               if (isDead()) {

                    agent.enabled = false;
                    obstacle.enabled = false;
                    //if (isBoss()) {
                    //    if (BattleEngine.scene.getEnemies().Count > 0) {
                    //        foreach (FightCharacter c in BattleEngine.scene.getEnemies()) {
                    //            if (!c.isBoss()) {
                    //                c.forceDie();
                    //            }
                    //        }
                    //    }
                    //    Time.timeScale = 0.3f;
                    //    BattleUI.instance.setEnabled(false);
                    //    App.input.setJoyEnabled(false);

                    //}
               }
               else if (isHitDown){
                   setObstacleMode(false);
               }
            }
        }

        private int beingAttackCount;
        protected override void afterAttacked() {
            if (beingAttackCount++ > BattleConfig.monsterChangeTargetThreshhold) {
                if (this.attacker != ai.target) {
                    ai.target = this.attacker;
                    beingAttackCount = 0;
                }
            }
        }

        public override bool inAttackAnimation() {
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            int state = info.nameHash;
            return state == Hash.monsterAtk1State || state == Hash.skill01State;
        }
       
    }
    
}

