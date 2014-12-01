using UnityEngine;
using System.Collections;
using engine;
public class Pet : PetCharacter {
    public int uiIndex;
    protected override void resetAgent() {
        base.resetAgent();
        agent.radius = 0.1f;
        //agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
    }
    public override void applyDamage(AttackResult attackResult, FightCharacter attacker)
    {
        base.applyDamage(attackResult, attacker);
        PetHeads.instance.setPetBlood(uiIndex, ((float)data.hp)/data.maxhp);
    }
    protected override void onDead()
    {
        base.onDead();
        PetHeads.instance.onDead(uiIndex);
        if (Player.instance.isDead()) {
            foreach (FightCharacter c in BattleEngine.scene.getFriends()) {
                if (!c.isPlayer()) {
                    if (!c.isDead()) {
                        Time.timeScale = 2.5f;
                        CameraManager.CameraFollow.target = c.transform;
                        break;
                    }
                }
            }
        }
    }
    protected override AvatarController createAvatarController() {
        return new PetController();
    }
    protected override string getAnimatorControllerName() {
         return "Local/controller/"+this.charTemplate.controller;
    }

    public void playSummon() {
      
       
        if (!string.IsNullOrEmpty(charTemplate.summonEffect)) {
            isInSummon = true;
            summonTriggered = false;
       
            getSkinnedMeshRenderer();
           
            //currently dissolve material name is using model name.
            Material m = Engine.res.loadObject("Local/material/" + charTemplate.model) as Material;
            this.skinRenderer.material = m;
            //controller.setTrigger(Hash.summonTrigger);
           //startReverseDissolve();
           
        } else {
            enableAI();
        }
    }
    private Timed timed;
    private bool summonTriggered;
    public void startReverseDissolve() {
        
        timed = this.skinRenderer.gameObject.addOnce<Timed>();
        timed.m_fDestruktionSpeed = charTemplate.summonDissolveSpeed;
        timed.m_fTime = 1;
       
    }

    //public bool Visible {
    //    set {
    //        getSkinnedMeshRenderer();
    //        if (value) {
    //            skinRenderer.enabled = true;
    //        }
    //        else {
    //            skinRenderer.enabled = false;
    //        }
    //    }
    //}

    public override void update() {
        //pet summon phase
        if (isInSummon) {
            if (timed!=null&&timed.m_fTime <= 0) {
                skinRenderer.materials = orignalMaterial;
                skinRenderer.gameObject.D<Timed>();
                timed = null;
                enableAI();
                isInSummon = false;   
                //CameraManager.removeFocus(model);
            }
            //AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            //int state = stateInfo.nameHash;
            //if (state == Hash.summonState) {
            //    summonTriggered = true;
            //}
            //if (summonTriggered&&state == Hash.idleState) {
            //    isInSummon = false;               
            //}
            return;
        }
       
        base.update();
    }
   
    private void enableAI() {
        setAgentEnable(true);
        ai.enabled = true;
        (ai as PetAI).petOwner = Player.instance;
        this.HpBar.Owner = this;
    }

    //flee ai
    protected override void afterAttacked() {
        BattleEngine.petEnv.afterAttacked(this);
    }
}
