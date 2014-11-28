using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using engine;

public class HeroController : AvatarController {
    protected Player player;
    public bool startReceiveCombo;

    private int[] comboIndices = new int[] { 1, 2, 3, 4 };//the order of normal attack when player keep pressing attack button.
    private int comboIndex;


    private bool requestMove, requestAtk;
    public bool playSkill;
    private Vector3 lookat = Vector3.zero;
    private Vector3 delta = Vector3.zero;
    private Vector3 nextTriggerPos;

    private int preAtkState;
    private FightCharacter targetToAttack;
    private FightCharacter rushTarget;
    private float rushDistance;
    private bool rushingToTarget;
    private float rushTime;
    private float oldAnimatorSpeed;
    private bool requestedChangeAttackAngle;
    private float moveDist = 1.5f;

    public override void reset(Character f) {
        base.reset(f);
        this.player = f as Player;


        requestMove = requestAtk =rushingToTarget=requestedChangeAttackAngle=playSkill=startReceiveCombo =false;
        targetToAttack = rushTarget = null;
        preAtkState=0;
        showShadow = false;
        player.calcNextPos = true;

        removeListener();
        addListener();

    }

    #region joystick
    protected void addListener() {
        EasyJoystick.On_JoystickMove += onMoveStart;
        EasyJoystick.On_JoystickMoveEnd += onMoveEnd;
    }
    public void removeListener() {
        EasyJoystick.On_JoystickMove -= onMoveStart;
        EasyJoystick.On_JoystickMoveEnd -= onMoveEnd;
    }


    void onMoveEnd(MovingJoystick move) {
        if (UIManager.Instance.Enable) {
            this.requestMove = false;
            player.resetAuto(true, true);
        }
    }
    float moveAngle;
    void onMoveStart(MovingJoystick move) {
        if (UIManager.Instance.Enable) {
            moveAngle = move.Axis2Angle();
            this.requestMove = true;
            player.resetAuto(false, true);
        }
    }
   public void  setJoyEnabled(bool v){
       if (!v)
       {
           requestMove = false;
           moveAngle = 0;
       }
   }
    #endregion

    bool changeAttackAngle = false;
    public void update(ref AnimatorStateInfo currentState) {
        if (rushingToTarget) {
            rushTime -= Time.deltaTime;
            if (rushTime <= 0) {
                rushingToTarget = false;
                rushTarget = null;
                animator.Play(Hash.atk1State, 0, 0);
                preAtkState = 0;
                animator.speed = 1;
                requestAtk = false;
                return;
            }
            else {
                //do nothing when rushing.
                animator.speed = 0;
                return;
            }
        }

        if (showShadow) {
            showShadow = PlayerGhost.instance.update();
        }

        updateInput();

        int state = currentState.nameHash;
        if (player.autoFight)
            autoFight(state);
        bool stateChange = false;
        changeAttackAngle = false;
        if (state == Hash.atk1State || state == Hash.atk2State || state == Hash.atk3State || state == Hash.atk4State) {
            if (preAtkState != 0 && preAtkState != state) {
                stateChange = true;
            }
            preAtkState = state;

            if (stateChange) {
                startReceiveCombo = false;
                requestedChangeAttackAngle = false;
                requestAtk = false;
                setBool(Hash.atknextBool, false);
                prepareSkill(++comboIndex);
                if (requestMove) {
                    requestedChangeAttackAngle = true;
                    joystickRotate();
                } else {
                    //if (!checkRush())
                    //{
                    //    adjustAttackAngle();
                    //}
                    adjustAttackAngle();
                }
            } else {
                if (requestAtk) {
                    if (startReceiveCombo) {
                        setBool(Hash.atknextBool, true);
                    }
                }
                if (!requestedChangeAttackAngle) adjustAttackAngle();

            }
        } else if (state == Hash.idleState) {
            comboIndex = 0;
            startReceiveCombo = false;

            if (requestAtk) {
                if (!playSkill) {
                    prepareSkill(0);
                    if (!checkRush()) {
                        adjustAttackAngle();
                        setTrigger(Hash.atkTrigger);
                    }
                } else {
                    FightCharacter fc = BattleEngine.scene.findNearestTarget(player);
                    if (fc != null) {
                        f.lookat(fc);
                    }
                }
                playSkill = false;
                requestAtk = false;
            } else if (requestMove) {
                setBool(Hash.runBool, true);
            }
        } else if (state == Hash.runState) {
            comboIndex = 0;
            startReceiveCombo = false;
            if (requestAtk) {
                if (!playSkill) {
                    prepareSkill(0);
                    if (!checkRush()) {
                        setTrigger(Hash.atkTrigger);
                    }
                } else {
                    FightCharacter fc = BattleEngine.scene.findNearestTarget(player);
                    if (fc != null) {
                        f.lookat(fc);
                    }
                }
                playSkill = false;
                requestAtk = false;
                setBool(Hash.runBool, false);
            } else if (requestMove) {
                doMove();
            } else if (!requestMove) {
                resetTrigger(Hash.atkTrigger);
                setBool(Hash.runBool, false);
            }
        }else if (state == Hash.atkEndState ) {
            if (requestMove) {
                resetTrigger(Hash.atkTrigger);
                setBool(Hash.runBool, true);
                play(Hash.runState,0,0);
                doMove();
            }
        }

        bool inIdleRun = state == Hash.idleState || state == Hash.runState;
        if (inIdleRun || stateChange || changeAttackAngle) {
            useAnimatorRotation = useAnimatorPosition = false;
        } else {
            useAnimatorRotation = useAnimatorPosition = true;
        }
       
        bool inAttackState=state == Hash.atk1State || state == Hash.atk2State || state == Hash.atk3State || state == Hash.atk4State||state==Hash.skill01State;
        if (!inAttackState) {
            player.enableWeaponCollider(false);
        } 

        if (inAttackState||state==Hash.atk1StartState) {
            player.switchWeapon(true);
        } else {
            player.switchWeapon(false);
        }
		targetToAttack = null;
    }

    private void autoFight(int state) {
        bool haveTarget = true;
        if (targetToAttack == null || targetToAttack.isDead()) {
            checkAttackTarget(100f);
            if (targetToAttack == null || targetToAttack.isDead()) {
                if (player.calcNextPos) {
                    if (BattleEngine.scene.hasNextTrigger(true)) {
                        nextTriggerPos = BattleEngine.scene.getTriggerPos();
                        requestMove = true;
                        player.lookat(nextTriggerPos);
                    }
                    player.calcNextPos = false;
                }
                else {
                    Vector3 vec = nextTriggerPos;
                    vec.y = player.Position.y;
                    if (Vector3.Distance(player.Position, vec) < player.cc.radius) {
                        player.agent.Stop();
                        requestMove = false;
                    }
                    else {
                        requestMove = true;
                    }
                }
                haveTarget = false;
            }
        }
        if(haveTarget) {
            checkAttackTarget(100f);
            float distance = Vector3.Distance(player.Position, targetToAttack.Position);
            if (distance <= player.getAttackRange() + 1f) {
                requestAtk = true;
                requestMove = false;
                moveDist = 1.5f;
                player.lookat(targetToAttack.Position);
            }
            else {              
                if (targetToAttack.isBoss() && (targetToAttack.Position.y - player.Position.y) > 0.1f) {
                    targetToAttack = BattleEngine.scene.findNearestTarget(player, targetToAttack, false);
                    if (targetToAttack == null) {
                        player.forceIdle();
                        return;
                    }
                }
                nextTriggerPos = targetToAttack.Position;              
                if (distance > player.cc.radius) {
                    if (moveDist > 0) {
                        requestMove = true;
                        player.lookat(nextTriggerPos);
                    }
                    else {
                        NavMeshHit hit;
                        bool inAttackState = state == Hash.atk1State || state == Hash.atk2State || state == Hash.atk3State || state == Hash.atk4State || state == Hash.skill01State;
                        if (inAttackState || player.agent.Raycast(targetToAttack.Position, out hit) || !checkRush()) {
                            moveDist = 1.5f;
                        }
                    }
                }
                else {
                    Debug.LogError("dddddddddddddddddddddd");
                    player.agent.Stop();
                    requestMove = false;
                }           
            }
        }
    }

    private void adjustAttackAngle() {

        //check attack direction,if not face to target,ajust angle
        if (targetToAttack == null || targetToAttack.isDead()) {
            checkAttackTarget();
        }
        if (targetToAttack != null) {
            if (!targetToAttack.isDead()) {
                if (!player.inMeleeAttackDirection(targetToAttack)) {
                    changeAttackAngle = true;
                    f.lookat(targetToAttack);
                }
            } else {
                targetToAttack = null;
            }
        }
    }
    private void prepareSkill(int index) {
        if (index < 0 || index >= comboIndices.Length) index = 0;
        comboIndex = index;
        player.prepareSkill(comboIndices[index]);
       
    }

    private bool showShadow;
    private void resetRushShadow() {
        PlayerGhost.instance.resetRushShadow();
        showShadow = true;
    }

   
    private bool checkRush() {
        //return false;
        //if (player.autoFight)
        //    return false;
        if (rushTarget == null) {
            checkRushTarget();
        }
        if (rushTarget != null && rushTarget.transform != null) {
            oldAnimatorSpeed = animator.speed;
            animator.Play(Hash.atk1StartState,0,0);
            Vector3 targetPosition = Vector3.MoveTowards(transform.position, rushTarget.transform.position, rushDistance - 1f);
            transform.LookAt(rushTarget.transform.position);
            resetRushShadow();
            float distance = Vector3.Distance(targetPosition, transform.position);
            rushTime = distance / BattleConfig.sprintSpeed;

            iTween.MoveTo(player.model, iTween.Hash("x", targetPosition.x, "z", targetPosition.z, "easeType", iTween.EaseType.easeInExpo, "time", rushTime));
            rushingToTarget = true;

            return true;
        }
        return false;
    }

    private void checkRushTarget() {
        FightCharacter enemy = BattleEngine.scene.findNearestTarget(player);
        if (enemy == null) return; 
        targetToAttack = enemy;
        float distance = Vector3.Distance(player.transform.position, enemy.transform.position);
        if (distance > player.getAttackRange() + BattleConfig.minSprintRange && distance < player.getAttackRange() + BattleConfig.sprintRange) {
            rushDistance = distance;
            rushTarget = enemy;
        } 
    }
    private void checkAttackTarget(float extraRange=1f) {
        targetToAttack = BattleEngine.scene.findNearestTarget(player);
        //FightCharacter enemy = BattleEngine.scene.findNearestTarget(player);
        //if (enemy == null) return;
        //float distance = Vector3.Distance(player.transform.position, enemy.transform.position);
        //if (distance < player.getAttackRange() + extraRange) {
        //    targetToAttack = enemy;
        //}
    }

    private void updateInput() {
        if (!BattleUI.instance.enable) return;
        if (Input.GetKeyDown(KeyCode.X)) {
            attackx();
        } else if (Input.GetKeyDown(KeyCode.A)) {
            //BattleUI.instance.summonPet(0);
        }
    }
   
    private void doMove() {
        if (player.autoFight) {
            player.agentMoveTo(nextTriggerPos);
            agent.speed = player.charTemplate.moveSpeed;
            moveDist -= agent.speed*Time.deltaTime;
        }
        else {
            float offset = player.charTemplate.moveSpeed * Time.deltaTime;
            joystickRotate();
            delta.Set(f.transform.forward.x * offset, 0, f.transform.forward.z * offset);
            agent.Move(delta);
        }      
    }
    private void joystickRotate() {
        //TODO opt
        Vector3 v1 = Camera.main.transform.rotation.eulerAngles;
        f.transform.eulerAngles = new Vector3(0, moveAngle + v1.y, 0);
    }
  

    public void attackx(bool atk = true) {
        requestAtk = atk;
    }

    public void disableMove() {
        requestMove = false;
        setBool(Hash.runBool, false);
    }

    public FightCharacter TargetToAttack {
        get { return targetToAttack; }
    }
   
}
