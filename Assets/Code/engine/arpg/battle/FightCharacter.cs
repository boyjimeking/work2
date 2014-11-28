﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace engine {


    //a character with fight ability,common base class for player,fightable pet,monster
    public class FightCharacter : Character, IAttackable {

        public int atkNo;
        public int localHitNo, globalHitNo;
        private HpBar hpBar;
        private GameObject hpBarObject;
        public bool suspendAI;          //suspend the character AI
        public bool suspendAnimator;    //suspend the character appear, the AI will also suspend

        public Stats stats;

        public bool becameVisible;

        public FightCharacter attacker;//remember who is attacking me.
        public float dietime;
        public bool disappear;
        public GameObject effectBySelf;

        public void clear()
        {
            GameObject.Destroy(model);
            if (hpBarObject != null)
            {
                GameObject.Destroy(hpBarObject);
            }
           
            model = null;
            hpBar = null;
        }

        #region reset

        public override void reset(GameObject model, CharData data, AI ai) {
            base.reset(model, data, ai);
            resetStats();
            this.team = data.team;
            if (hitColliders != null) hitColliders.Clear();

            if (ai != null) ai.reset(this);
            suspendAnimator = false;
            suspendAI = false;
        }

        protected void resetStats() {
            if (stats == null) stats = new Stats();
            stats.reset(this);
        }

        #endregion


        #region iattackable

        public void addParticleEffect(Vector3 position, GameObject explosive) {
            if (explosive != null) {
                GameObject instance = GameObject.Instantiate(explosive, position, Quaternion.identity) as GameObject;
                GameObject.Destroy(instance, 2f);
            }
        }

        public void playHitSound(Vector3 position) {

        }

        public Team team;
        public Team getTeam() {
            return team;
        }

        #endregion


        #region attack
        private Weapon weapon,atkWeapon,currentWeapon;
        public LearnedSkill currentSkill;
        public SkillEffectTemplate currentSkillEffect;

        public void setWeapons(Weapon idleWeapon, Weapon atkWeapon, Weapon current) {
            this.weapon = idleWeapon;
            this.atkWeapon = atkWeapon;
            this.currentWeapon = current;
        }
        public void enableWeaponCollider(bool v) {
            if (currentWeapon == null) return;
            currentWeapon.enableCollider(v);
        }
        public void switchWeapon(bool atk) {
            if (atk && currentWeapon == atkWeapon || !atk && currentWeapon == weapon) return;
            Weapon old = currentWeapon;
            currentWeapon = atk ? atkWeapon : weapon;
            if (old != currentWeapon) {
                old.model.SetActive(false);
                currentWeapon.model.SetActive(true);
            }
        }

        public float getAttackRange() {
            return data.weaponRange;
        }
        public bool attack(int skillId) {
            if (prepareSkill(skillId))
            {
                if (currentSkillEffect==null || currentSkillEffect.triggerHash != 0) {
                    controller.setTrigger(currentSkillEffect.triggerHash);
                } else {
                    controller.setTrigger(Hash.atkTrigger);
                }
                return true;
            }
            return false;
            
        }
        public bool prepareSkill(int skillId) {
            LearnedSkill learnedSkill = data.getLearnedSkill(skillId);
            if (learnedSkill == null) return false;

            currentSkill = learnedSkill;
            currentSkill.cdTime = Time.time;
            currentSkillEffect = currentSkill.template.effect;
            return true;
        }
        #endregion

        public virtual bool inSkillAnimation() {
            return false;
        }


        #region hit
        private Dictionary<int, ColliderObject> hitColliders;
        private HitEffect hitEffect;
        public bool addHit(ColliderObject co) {
            if (hitColliders == null) hitColliders = new Dictionary<int, ColliderObject>();
            if (hitColliders.ContainsKey(co.id)) return false;
            hitColliders[co.id] = co;
            return true;
        }
        public void removeHit(ColliderObject co) {
            if (hitColliders != null && hitColliders.ContainsKey(co.id)) {
                hitColliders.Remove(co.id);
            }

        }
        public SkinnedMeshRenderer skinRenderer;
        public Material[] orignalMaterial;
        public Texture[] originalMainTExture;
        public SkinnedMeshRenderer getSkinnedMeshRenderer() {
            if (skinRenderer != null) return skinRenderer;
            skinRenderer = model.GetComponentInChildren<SkinnedMeshRenderer>();
            if (skinRenderer == null) return null;
            int length = skinRenderer.materials.Length;
            orignalMaterial = new Material[length];
            originalMainTExture = new Texture[length];
            for (int i = 0; i < length; i++) {
                orignalMaterial[i] = skinRenderer.materials[i];
                originalMainTExture[i] = orignalMaterial[i].mainTexture;
            }

            return skinRenderer;
        }
        #endregion


        #region damage and hit state
        public bool isHitDown, isHitBack, isAttacking, isDying, isPlayHit, isFreezing;
        private float hitbackTime, hitbackTimer;
        private float freezTime, freezTimer;
        public bool isInSummon;//used in pet summon.
        public bool isIdle;
        public bool isDead() {
            return data.hp <= 0;
        }
        public virtual bool inAttackAnimation() {
            return false;
        }
        public void takeDamage(ColliderObject co, string particleEffect = null) {
            FightCharacter attacker = co.owner;
            bool canEVA = !((attacker is Player) && co.effect.id > 4);
            AttackResult result = BattleEngine.controller.calcDamage(attacker, this, canEVA);
            applyDamage(result, attacker);
            if (result.state == AttackState.eva) return;
            if (isDead()) {
                quickDie = true;
                //animator.SetTrigger(Hash.dieTrigger);
            } else {
                if (particleEffect == null)
                {
                    particleEffect = Naming.EffectPath + "hurt";
                }
                else
                {
                    particleEffect = Naming.EffectPath + particleEffect;
                }
                GameObject prefab = Engine.res.loadPrefab(particleEffect);
                addParticleEffect(transform.position + Vector3.up, prefab);
                HitState state = BattleEngine.controller.calcHitState(attacker, this, co.effect);
                applyHitState(state);
            }
            setBuffState(co.effect);
            addHitEffect();
            Engine.sound.playSound("Local/sound/hurt", transform.position);
        }
        public void takeDamage(Bullet bullet, string particleEffect = null) {
            AttackResult result = BattleEngine.controller.calcDamage(bullet.owner, this);
            applyDamage(result, bullet.owner);
            if (result.state == AttackState.eva) return;
            if (isDead())
            {
                applyHitEffect(particleEffect);
                quickDie = true;
                //animator.SetTrigger(Hash.dieTrigger);
            }
            else
            {
                applyHitEffect(particleEffect);
                if (attacker.currentSkillEffect == null)
                    return;
                HitState state = BattleEngine.controller.calcHitState(attacker, this, attacker.currentSkillEffect);
                setBuffState(attacker.currentSkillEffect);
                applyHitState(state);
                //takeDamage(bullet.owner, bullet.temp);
            }
            Engine.sound.playSound("Local/sound/hurt", transform.position);
        }
        public void takeDamage(FightCharacter attacker, string particleEffect = null) {
            AttackResult result = BattleEngine.controller.calcDamage(attacker, this);
            applyDamage(result, attacker);
            if (result.state == AttackState.eva) return;
            if (isDead()) {
                applyHitEffect(particleEffect);
                quickDie = true;
                //animator.SetTrigger(Hash.dieTrigger);
            } else {
                applyHitEffect(particleEffect);
                if (attacker.currentSkillEffect == null)
                    return;
                HitState state = BattleEngine.controller.calcHitState(attacker, this, attacker.currentSkillEffect);
                setBuffState(attacker.currentSkillEffect);
                applyHitState(state);
            }
            Engine.sound.playSound("Local/sound/hurt", transform.position);
            //AudioSource.PlayClipAtPoint(Engine.res.loadSound("Local/sound/hurt"), transform.position);
        }

        public void takeDamage(FightCharacter attacker, SkillEffectTemplate temp) {
            AttackResult result = BattleEngine.controller.calcDamage(attacker, this);
            applyDamage(result, attacker);
            if (isDead()) {
                quickDie = true;
                return;
            }
            setBuffState(temp);
            HitState state = BattleEngine.controller.calcHitState(attacker, this, temp);
            applyHitState(state);
            applyHitEffect(temp.hitEffect);
            Engine.sound.playSound("Local/sound/hurt", transform.position);
        }

        protected void setBuffState(SkillEffectTemplate temp) {
            if (temp.buffs != null && temp.buffTimes != null && temp.buffs.Length > 0 && temp.buffs.Length == temp.buffTimes.Length) {
                for (int i = 0; i < temp.buffs.Length; i++) {
                    switch (temp.buffs[i]) {
                        case BuffType.none:
                            break;
                        case BuffType.dizzy:
                            setDizzy(temp.buffTimes[i]);
                            break;
                        case BuffType.shakeCamera:
                            CameraManager.shakeCamera(CameraManager.Main, temp.buffTimes[i]);
                            break;
                        case BuffType.netTarget:
                            setDizzy(temp.buffTimes[i], "Local/prefab/effect/stop");
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        protected void applyHitEffect(string particleEffect) {
            if (particleEffect == null) {
                particleEffect = Naming.EffectPath + "hurt";
            }
            else
            {
                particleEffect = Naming.EffectPath + particleEffect;
            }
            GameObject prefab = Engine.res.loadPrefab(particleEffect);
            addParticleEffect(transform.position + new Vector3(0, 1f, 0), prefab);
            addHitEffect();
        }
        protected void addHitEffect() {
            if (hitEffect == null) {
                HitEffect effect = new HitEffect();
                effect.reset(this, BattleConfig.hitEffectTime, Color.white);
                hitEffect = effect;
            } else {
                hitEffect.reset(this, BattleConfig.hitEffectTime, Color.white);
            }
        }

        public bool inMeleeAttackDirection(FightCharacter c) {
            float direction = Vector3.Dot((c.transform.position - transform.position).normalized, transform.forward);
            if (direction < 0.5f) return false;
            return true;
        }
        protected bool quickDown, quickHurt, quickDie;
        protected virtual void applyHitState(HitState state) {
            if (state.hitdown) {
                if (!isHitDown) {
                    quickDown = true;
                }
            } else if (state.hitback) {
                //if (!isHitDown) 
                playHitBack(state.attacker, state.hitbackDistance);
                quickHurt = state.playHit && !isPlayHit;

            } else if (state.playHit && !isPlayHit) {
                if (!isPlayHit) {
                    quickHurt = true;
                }
            }

        }
        public bool isBeyondCamera()
        {
            return model.gameObject.renderer && !model.gameObject.renderer.isVisible;
        }
        protected virtual void onDead()
        {
            animator.ResetTrigger(Hash.skill2Trigger);
            controller.play(Hash.dieState, 0, 0);
            isDying = true;
        }
       
        public void playHitBack(FightCharacter attacker, float force) {
            if (isBoss()) {
                AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
                if (info.nameHash == Hash.skill02State) {
                    return;
                }
            }
            Vector3 dir = (transform.position-attacker.transform.position).normalized;
            dir.y = 0;
            Vector3 targetPosition = transform.position +dir * force;

            NavMeshHit hit;
            if (NavMesh.Raycast(transform.position, targetPosition, out hit, 1)) {
                return;
            }

            if (beatBackTween == null) beatBackTween = new BeatBackTween();
            beatBackTween.reset(model, dir * force);

            hitbackTimer = 0;
            isHitBack = true;

            setObstacleMode(false);
            hitbackTime = BattleConfig.hitBackTime1 + BattleConfig.hitBackTime2;
            //iTween.MoveTo(model, iTween.Hash("x", targetPosition.x, "z", targetPosition.z, "easeType", (iTween.EaseType)BattleConfig.hitBackType, "time", hitbackTime));

            isFreezing = true;
            freezTime = hitbackTime + BattleConfig.hitBackFreezeTime;
            freezTimer = 0;

        }

        private IEnumerator hitbackStep(float delay, Vector3 tp, float duration) {
            yield return new WaitForSeconds(delay);
            Hashtable args = new Hashtable();
            args.Add("x", tp.x);
            args.Add("z", tp.z);
            args.Add("easeType", iTween.EaseType.linear);
            args.Add("time", duration);
            iTween.MoveTo(model, args);
        }

        public void addSkillState(FightCharacter attacker, SkillEffectTemplate temp) {
            if (!skillStates.ContainsKey(temp.id)) {
                SkillState state = new SkillState(temp, this, attacker);
                skillStates.Add(temp.id, state);
            }
        }

        public bool existMultiSKill(int skillID) {
            if (skillStates.ContainsKey(skillID)) {
                SkillState state = skillStates[skillID];
                if(state.canHurt())
                    return true;
            }
            return false;
        }

        public void forceDie() {
            //when boss die,all other monsters die
            AttackResult result = new AttackResult(data.hp + 1);
            applyDamage(result, attacker);
            quickDie = true;
        }
        public virtual void applyDamage(AttackResult attackResult, FightCharacter attacker)
        {
            if (this.isDead()) {
                this.attacker=null;
                return;
            }
            data.hp -= attackResult.damage;
            if (this.isDead())
            {
                data.hp = 0;
                onDead();
                if (this.isBoss())
                {
                    BattleEngine.dungeonWin = true;
                    BattleEngine.battleEnded = true;
                }
            }
            HpBar.CurrentHp = data.hp;
            addFloatingText2(transform.position, attackResult);
            if (this == Player.instance)
            {
                if (data.hp < data.maxhp * 0.2f)
                    RedScreen.instance.begin();
                else
                    RedScreen.instance.stop();
            }   
            if (attacker != null && attacker.ai is MonsterAI)
            {
                ((MonsterAI)attacker.ai).resetNoHit();
            }
            this.attacker = attacker;
            if(!isDead()) afterAttacked();
            Debug.Log(attackResult.state + "----" + attackResult.damage);
        }
        //escape ai, currently only pet
        protected virtual void afterAttacked() {

        }
        protected void addFloatingText2(Vector3 position, AttackResult attackResult) { 
             GameObject prefab = getFloatingTextPrefab();
             if (prefab != null)
             {
                 var floattext = (GameObject)GameObject.Instantiate(prefab, position, transform.rotation);
                 GameObject.Destroy(floattext, 1);
                 FloatingText floatingText = floattext.GetComponent<FloatingText>();
                 GameObject label = null;
                 string damageText = "";
                 if (attackResult.state == AttackState.none) {
                     label = Engine.res.createSingle("Local/prefab/floatingtext/EffectNormal");
                     damageText = attackResult.damage + "";
                 }
                 else if (attackResult.state == AttackState.critical) {
                     label = Engine.res.createSingle("Local/prefab/floatingtext/EffectCritical");
                     damageText = "" + attackResult.damage;
                 }
                 else if (attackResult.state == AttackState.eva) {
                     label = Engine.res.createSingle("Local/prefab/floatingtext/EffectEva");
                 }
                 floatingText.reset(label, damageText);
             }
             
        }
        protected void addFloatingText(Vector3 position, string text, FloatingTextFormat format) {
            GameObject prefab = getFloatingTextPrefab();
            if (prefab != null)
            {
                var floattext = (GameObject)GameObject.Instantiate(prefab, position, transform.rotation);
                FloatingText floatingText = floattext.GetComponent<FloatingText>();
                if (floatingText)
                {
                    floatingText.Text = text;
                    floatingText.FontSize = format.fontSize;
                    floatingText.TextColor = format.color;
                }
                GameObject.Destroy(floattext, 1);
            }
        }
        protected virtual GameObject getFloatingTextPrefab() {
            return Engine.res.loadPrefab("Local/prefab/floatingtext/FloatTextEnemy");
        }
        #endregion

        private void setDizzy(float dizzyTime, string dizzyPath = null) {
            if (isPlayer()) {
                if (!UIManager.Instance.Enable) //if in dizzy state return
                    return;
                (controller as HeroController).disableMove();
                Player.instance.resetAuto(false, true);
                UIManager.Instance.Enable = false;
                App.input.setJoyEnabled(false);
            }
            Transform trans = getTagPoint("help_hp");
            string afterPath = null;
            if (dizzyPath == null)
                dizzyPath = "Local/prefab/effect/xuanyun";
            else
                afterPath = "Local/prefab/effect/stop_end";
            GameObject dizzyObj = App.res.createSingle(dizzyPath);
            Vector3 vec = new Vector3(0f, 1.4f, 0f);
            if (trans != null)
                vec = trans.localPosition;
            vec.y = vec.y - 0.3f;
            if (dizzyPath != null)
                vec = Vector3.zero;
            dizzyObj.transform.parent = trans.parent;
            dizzyObj.transform.localPosition = vec;
            suspendAI = true;
            //agent.enabled = false;
            animator.SetBool(Hash.runBool, false);
            animator.Play(Hash.idleState);      
            App.coroutine.StartCoroutine(clearDizzy(dizzyTime, dizzyObj, afterPath));
        }

        private IEnumerator clearDizzy(float dizzyTime, GameObject dizzyObj, string afterPath) {
            yield return new WaitForSeconds(dizzyTime);
            if (!UIManager.Instance.Enable) {
                UIManager.Instance.Enable = true;
                App.input.setJoyEnabled(true);
            }
            if(isPlayer())
                Player.instance.resetAuto(true, true);
            suspendAI = false;
            //agent.enabled = true;
            Object.Destroy(dizzyObj);
            if (afterPath != null) {
                GameObject obj = App.res.createSingle(afterPath);
                obj.transform.parent = transform;
                obj.transform.localPosition = Vector3.zero;
                Object.Destroy(obj, 1f);
            }
        }
       
        #region update
       protected  AnimatorStateInfo stateInfo;
       private BeatBackTween beatBackTween;
        public override void update() {
            if (destroyed) return;
            if (suspendAnimator) {
                return;
            }
            if (animator != null) {
                stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                updateAnimatorState();
            }
           
            if (isHitBack) {
                if (beatBackTween != null && !beatBackTween.end)
                {
                    beatBackTween.update();
                }
                else isHitBack = false;
                //hitbackTimer += Time.deltaTime;

                //if (hitbackTimer >= hitbackTime) {
                //    hitbackTimer = 0;
                //    isHitBack = false;
                //}
            }
            if (isFreezing) {
                freezTimer += Time.deltaTime;
                if (freezTimer >= freezTime) {
                    freezTimer = 0;
                    isFreezing = false;
                }
            }

            if (hitEffect != null && !isDead()) {
                hitEffect.update();
                if (hitEffect.completed) {
                    if (skinRenderer != null) skinRenderer.materials = orignalMaterial;
                }
            }

            if (isDead()) {
                updateDead();
            } else {
                //make sure ai update is executed after animator update.
                if (ai != null && !suspendAI) {
                    ai.update();
                }
                
            }


            isIdle = stateInfo.nameHash == Hash.idleState;

        }
       
        protected virtual void updateAnimatorState() {
            controller.useAnimatorPosition = false;
            controller.useAnimatorRotation = false;
            binding.canTrigger = true;
            int state = stateInfo.nameHash;
            if (isBoss() && state == Hash.skill02State) {
                controller.useAnimatorPosition = true;
                binding.canTrigger = false;
            }
            if (quickDown) {
                quickDown = false;
                if (state != Hash.downState) {
                    lookat(attacker);
                    setObstacleMode(false);
                    controller.useAnimatorPosition = true;
                    //controller.resetAttackTrigger();
                    //controller.setTrigger(Hash.hitdownTrigger);
                    controller.play(Hash.downState, 0, 0);
                }
                isHitDown = true;
            } else  {
                isHitDown = state == Hash.downState;
            }
            if (quickHurt) {
                quickHurt = false;
                if (state != Hash.hurtState) {
                    lookat(attacker);
                    controller.play(Hash.hurtState, 0, 0);
                }
                isPlayHit = true;
            } else {
                isPlayHit = state == Hash.hurtState;
            }

            if (quickDie) {
                quickDie = false;
                //controller.play(Hash.dieState, 0, 0);
                //isDying = true;
            }
        }

        public virtual void updateDead() {
            if (state != CharacterState.dead)
            {
                state = CharacterState.dead;
                if (hitEffect != null)
                {
                    hitEffect.setComplete();
                }
                if (skinRenderer != null)
                {
                    skinRenderer.materials = orignalMaterial;
                }
                if (!isPlayer())
                {
                    dietime = 0;
                    disappear = false;
                    GameObject.Destroy(model, 4f);
                }
            }
            else if (this.team.teamNo == Naming.TeamMonster)
            {
                if (!disappear)
                {
                    dietime += Time.deltaTime;
                    if (dietime > 1f)
                    {
                        playDisappear();
                    }
                }
                
            }
        }

        void playDisappear()
        {
            disappear = true;
            GameObject effect = Engine.res.createSingle("Local/prefab/effect/monster_disappear");
            effect.transform.position = model.transform.position;
            GameObject.Destroy(effect, 4f);
        }
        #endregion


        public HpBar HpBar {
            get {
                if (hpBar == null) {
                    Object temp = Resources.Load("Local/UI/HP_UI/head_board");
                    GameObject obj = Object.Instantiate(temp, Vector3.zero, Quaternion.identity) as GameObject;
                    obj.transform.localPosition = new Vector3(0, 1.4f, 0);
                    hpBar = obj.transform.FindChild("ui_hp_bar").GetComponent<HpBar>();
                    hpBar.Owner = this;
                    obj.transform.parent = UIManager.Instance.uiCamera.transform;
                    hpBarObject = obj;
                }
                return hpBar;
            }
        }

        public virtual FightCharacter AttackTarget{
            get { return ai.target; }
        }

        private float animatorSpeedBeforePause;
        public void pause(bool v) {
            pauseAnimator(v);
            if (v) {
                suspendAI = true;
                agent.enabled = false;
            } else {
                suspendAI = false;
                setObstacleMode(false);
            }
        }
        public void pauseAnimator(bool v)
        {
            if (suspendAnimator == v) return;
            if (v)
            {
                suspendAnimator = true;
                animatorSpeedBeforePause = animator.speed;
                animator.speed = 0;

            } else {
                suspendAnimator = false;
                animator.speed = animatorSpeedBeforePause;
            }
        }
    }
}