using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using engine;
public class Player : PlayerCharacter {
    public static Player instance = new Player();
    public bool canMove = true;
    public bool autoFight = false;
    public bool calcNextPos = false;
    private bool origAutoFight = false;

    private HeroController playerController;
    public override void reset(GameObject model, CharData data,AI ai) {
        base.reset(model, data, ai);
        playerController = controller as HeroController;
        getSkinnedMeshRenderer();
        calcNextPos = true;
        //skinRenderer.material.shader = Shader.Find("BehindWall2");

    }
    protected override AvatarController createAvatarController() {
        return new HeroController();
    }
   
   // protected override void resetAgent() {
    //    base.resetAgent();
        //agent.radius = 0.1f;
        //agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
   // }
    public override void setObstacleMode(bool v) {
        agent.enabled = !v;
        agent.updatePosition = !v;
        agent.updateRotation = !v;
    }
    protected override string getAnimatorControllerName() {
        return "Local/controller/man";
    }

     protected override GameObject getFloatingTextPrefab() {
         return Engine.res.loadPrefab("Local/prefab/floatingtext/FloatTextPlayer");
     }
    

     public void attackx() {
         playerController.attackx();
         
     }
     public void attackx2()
     {
         playerController.attackx();
         playerController.playSkill = true;
     }
     public void startReceiveCombo() {
         playerController.startReceiveCombo = true;
     }

     protected override void updateAnimatorState() {
         base.updateAnimatorState();
         playerController.update(ref stateInfo);
     }

     public void updateAttackingSkillInfo(int skillId) {
         LearnedSkill learnedSkill = data.getLearnedSkill(skillId);
         if (learnedSkill == null) return ;
         currentSkill = learnedSkill;
         currentSkill.cdTime = Time.time;
         currentSkillEffect = currentSkill.template.effect;
     }

     public override void onBorn() {
         GameObject obj = Object.Instantiate(Resources.Load("Local/sequence/bornSEQ")) as GameObject;
         PlayerBorn pb = obj.transform.Find("Camera").GetComponent<PlayerBorn>();
         pb.onBegin(model.transform);
     }

     protected override void onDead() {
         base.onDead();
         if (BattleEngine.scene.getFriends().Count > 0) {
             foreach (FightCharacter c in BattleEngine.scene.getFriends()) {
                 if (!c.isPlayer()) {
                     Time.timeScale = 2.5f;
                     CameraManager.CameraFollow.target = c.transform;
                     break;
                 }
             }
             PlayerDie.instance.setActive(true);
         }
         BattleUI.instance.setActive(false);
         BossHead.instance.setActive(true);
         App.input.setJoyEnabled(false);
     }

    public void beginMove(Vector3 velocity) {
        transform.position = Position + velocity*Time.deltaTime;
    }

    public void setMoveState(bool value) {
        controller.setBool(Hash.runBool, value);
    }
    public void setJoyEnabled(bool v) {
        if (playerController != null) playerController.setJoyEnabled(v);
    }

    public void forceIdle() {
        controller.setBool(Hash.runBool, false);
        controller.resetTrigger(Hash.atkTrigger);
        App.input.setJoyEnabled(false);
    }

    public void resetAuto(bool state, bool reset = false) {
        if (reset) {
            if (state)
                autoFight = origAutoFight;
            else {
                if(autoFight)
                    agent.Stop();
                autoFight = false;  
            }
        }
        else {
            autoFight = state;
            origAutoFight = state;
            if (!state) {
                agent.Stop();
                agent.ResetPath();
                controller.reset(this);
            }
        }

    }

    public override FightCharacter AttackTarget {
        get {   return playerController.TargetToAttack; }
    }
}
