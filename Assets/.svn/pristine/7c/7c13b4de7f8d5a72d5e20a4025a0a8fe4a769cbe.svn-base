using UnityEngine;
using System.Collections;
namespace engine {
    public enum PetPosition {
        leftforward, rightforward, lefthehind, rightbehind
    }

    public class PetAI : AI {
        public PetPosition petPosition;
        protected float left, before;

        public FightCharacter petOwner;

        protected float followDelayTime = 0.5f;
        protected float followDelayTimer;
        protected LearnedSkill currentSkill;

        private float helpTimer;
        private bool beingAttacked;
        private bool noFlee;//
        private Vector3 nextTriggerPos;
        private float atkRange = 0;
        public override void reset(FightCharacter fc) {
            base.reset(fc);
            fleeTimer = 0;
            fleeSuccessStayTime = 0;
            tryFlee = inFlee=noFlee=beingAttacked=false;
            fleeAttacker = null;
            nextTriggerPos = Vector3.down;
        }

        public PetAI(PetPosition petPosition) {
            this.petPosition = petPosition;
            float distance = 2f;
            switch (petPosition) {
                case PetPosition.leftforward: left = -distance; before = distance; break;
                case PetPosition.rightforward: left = distance; before = distance; break;
                case PetPosition.lefthehind: left = -distance; before = -distance; break;
                case PetPosition.rightbehind: left = distance; before = -distance; break;
            }
        }
        public Vector3 getPosition(Transform t,int walkablemask) {
            Vector3 position = t.position + left * t.right + before * t.forward;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(position, out hit, 100, walkablemask)) {
                return hit.position;
            }
            return position;
        }

        //when win animation playing, pet need show, 0 show pet in left, 1 show pet in right
        public Vector3 getWinPosition(Transform player, int pos) {
            Vector3 position = Vector3.zero;
            float distance = 2f;
            if (pos == 1) {
                position = player.position + distance * player.right + -distance * player.forward;
            } else {
                position = player.position + -distance * player.right + -distance * player.forward;
            }
            return position;
        }

        public override void update() {
            if (!enabled) {
                return;
            }
            if (owner.isDead() || owner.isHitDown || owner.isHitBack || owner.isPlayHit || owner.isAttacking || owner.isInSummon) return;

            float delta = Time.deltaTime;
            fleeTimer -= delta;
            fleeSuccessStayTime -= delta;
            if (BattleEngine.petEnv.helpPet == owner) {
                helpTimer += delta;
                if (helpTimer > BattleConfig.petTryHelpTime) {
                    if (target == null || target.isDead() || target.ai.target != owner) {
                        helpTimer = 0;
                        BattleEngine.petEnv.helpPet = null;
                    }
                }
            }
           

            //flee/ask help/try help others

            //check if this pet is the only friend alive on the scene
            //if so, no more flee behavior
            bool onlyThisAlive = true;
            foreach (FightCharacter c in BattleEngine.scene.getFriends()) {
                if (c != owner && !c.isDead()) {
                    onlyThisAlive = false;
                    break;
                }
            }
            if (!onlyThisAlive&&!noFlee) {
                noFlee = false;
                if (checkFleeOrHelp()) return;
                if (beingAttacked) {
                    beingAttacked = false;
                } else if (fleeSuccessStayTime > 0) return;//stay in the safe area for some time.
            }
            //test
            if (owner.charTemplate.id == 5001)
            {
                atkRange = 0;
            }
            if (currentSkill == null) {
                currentSkill = nextSkill();
                if (currentSkill != null) {
                    atkRange = currentSkill.template.atkRange;
                }
            }
            if (target != null && !target.isDead()) {
                nextTriggerPos = Vector3.down;
                //float distance = Vector3.Distance(target.transform.position, transform.position);
                if (owner.inAttackRange(target,atkRange)) {
                    owner.agentStop();
                    owner.setObstacleMode(true);
                    if (!owner.inAttackAnimation()) {
                        owner.lookat(target);
                    }
                    if (currentSkill != null) {
                        owner.attack(currentSkill.skillId);
                        currentSkill = null;
                    }
                } else {
                    if (target.isBoss() && (target.Position.y - owner.Position.y) > 0.1f) {
                        attackTimer = attackInterval;
                        target = BattleEngine.scene.findNearestTarget(owner, target, false);
                        if (target == null) {
                            gotoPetOwner();
                        }
                    }
                    else if (fleeSuccessStayTime <= 0) {
                        if (!(owner as PetCharacter).inAttackAnimation()) {
                            attackTimer = attackInterval;
                            owner.setObstacleMode(false);
                            // owner.lookat(target);
                            owner.agentMoveTo(target);
                        }
                    }

                }
            } else {
                attackTimer = attackInterval;
                //owner.setObstacleMode(true);
                target = BattleEngine.scene.findNearestTarget(owner);
                if (target == null) {
                    gotoPetOwner();
                }
            }
        }

        protected void gotoPetOwner() {
            if (petOwner == null || petOwner.isDead()) {
                if (BattleEngine.scene.hasNextTrigger()) {
                    nextTriggerPos = BattleEngine.scene.getTriggerPos();
                    owner.lookat(nextTriggerPos);
                    owner.agentMoveTo(nextTriggerPos);
                }else if (Vector3.Distance(transform.position, nextTriggerPos) < 0.5f) {
                    nextTriggerPos = Vector3.down;
                    owner.agentStop();
                }
                else if (nextTriggerPos != Vector3.down) {
                    owner.lookat(nextTriggerPos);
                    owner.agentMoveTo(nextTriggerPos);
                }
                return;
            }

            Vector3 position = getPosition(petOwner.transform,owner.agent.walkableMask);
            float distance = Vector3.Distance(transform.position, position);
            if (distance >owner.cc.radius) {
                followDelayTimer += Time.deltaTime;
                if (followDelayTimer > followDelayTime) {
                    followDelayTimer = 0;
                    // owner.lookat(position);
                    owner.agentMoveTo(position);
                }
            } else {
                owner.agentStop();
            }
        }

        private bool checkFleeOrHelp() {
            if (inFlee) {
                if (inFlee = doFlee()) {
                    return true;
                }
            }
            if (tryFlee) {
                tryFlee = false;
                if (doFlee()) {
                    inFlee = true;
                    return true;
                } else {
                    owner.agent.speed = agentSpeed;
                    owner.animator.speed = animatorSpeed;
                    if (BattleEngine.petEnv.fleePet == owner) {
                        BattleEngine.petEnv.fleePet = null;
                    }
                }
            } else {
                //check if somebody else need help
                if (BattleEngine.petEnv.fleePet != null && BattleEngine.petEnv.fleePet != owner) {
                    if (tryHelp(BattleEngine.petEnv.fleePet)) {
                        return true;
                    } else {
                        if (BattleEngine.petEnv.helpPet == owner) {
                            BattleEngine.petEnv.helpPet = null;
                        }
                    }
                }
            }
            return false;
        }

        private bool tryHelp(FightCharacter need) {
            if (need.isDead() || need.ai.target == null || need.ai.target.isDead()) {
                BattleEngine.petEnv.fleePet = null;
                return false;
            }
            if (BattleEngine.petEnv.helpPet != null && BattleEngine.petEnv.helpPet != owner) {
                //somebody has already gone to help
                return false;
            }
            if (owner.data.hp >= owner.data.maxhp * 0.5) {
                target = need.ai.target;
                BattleEngine.petEnv.helpPet = owner;
                return true;
            }
            return false;
        }

        private bool doFlee() {
            if (fleeAttacker == null || fleeAttacker.isDead()) {
                fleeSuccessStayTime = 0;
                return false;
            }
            if (fleeAttacker.ai != null && fleeAttacker.ai.target != owner) {
                fleeSuccessStayTime = 0;
                //the original attacker has changed attack target
                return false;
            }
            if (Vector3.SqrMagnitude(owner.transform.position - fleeAttacker.transform.position) < BattleConfig.petFleedDistanceSqr) {
                
                Vector3 fleePosition = owner.transform.position + (owner.transform.position - fleeAttacker.transform.position).normalized * 3;
                owner.agent.speed *= BattleConfig.petFleeSpeedRate;
                owner.animator.speed *= BattleConfig.petFleeSpeedRate;

                owner.lookat(fleePosition);
                owner.agentMoveTo(fleePosition);
                return true;
            }
            fleeSuccessStayTime = BattleConfig.fleeSuccessStayTime;

            return false;
        }

        //pet flee / look for help ai
        private float agentSpeed, animatorSpeed;
        private float fleeTimer;
        private bool tryFlee,inFlee;
        private FightCharacter fleeAttacker;//flee away from this attacker.
        private float fleeSuccessStayTime;//stay in safe are for sometime
        
        public void afterAttacked() {
            if (beingAttacked) {
                noFlee = true;
            }
            beingAttacked = true;
            if (fleeTimer > 0 || BattleEngine.petEnv.fleePet != null || owner.attacker == null || owner.attacker.isDead()) {
                fleeSuccessStayTime = 0;
                return;
            }
            if (owner.data.hp < owner.data.maxhp * 0.5) {
                float atkRange = 2f;
                if (currentSkill != null) {
                    atkRange = currentSkill.template.atkRange;
                }
                if (lessFriendNumber(atkRange)) {
                    fleeAttacker = owner.attacker;
                    fleeTimer = BattleConfig.petFleeCD;
                    tryFlee = true;
                    BattleEngine.petEnv.fleePet = owner;
                    agentSpeed = owner.agent.speed;
                    animatorSpeed = owner.animator.speed;
                }
            }
        }
        private bool lessFriendNumber(float radius) {
            float distance = radius * radius;
            int friendNo = 0, enemyNo = 0;
            foreach (FightCharacter c in BattleEngine.scene.getFriends()) {
                if (c != owner && Vector3.SqrMagnitude(c.transform.position - owner.transform.position) < distance) {
                    friendNo++;
                }
            }
            foreach (FightCharacter c in BattleEngine.scene.getEnemies()) {
                if (c != owner && Vector3.SqrMagnitude(c.transform.position - owner.transform.position) < distance) {
                    enemyNo++;
                }
            }
            return friendNo * BattleConfig.petFriendNumberRate <= enemyNo;
        }

    }
}

