using UnityEngine;
using System.Collections;
using engine;

public class PetController : AvatarController {
    protected FightCharacter f;
    public override void reset(Character f) {
        base.reset(f);
        this.f = f as FightCharacter;
    }


    public void update() {
        if (rushingToTarget) {
            rushTimer += Time.deltaTime;
            if (rushTimer >= rushTime) {
                rushingToTarget = false;
                rushTarget = null;
                animator.speed = oldAnimatorSpeed;
                animator.SetTrigger(Hash.atkTrigger);
                preAtkState = 0;
            } else {
                //do nothing when rushing.
                return;
            }
        }

        updateInput();

        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        int state = currentState.nameHash;
        bool stateChange = false;
        bool changeAttackAngle = false;
        if (state == Hash.atk1State || state == Hash.atk2State || state == Hash.atk3State) {
            if (preAtkState != 0 && preAtkState != state) {
                stateChange = true;
            }
            preAtkState = state;

            if (stateChange) {
                animator.SetBool(Hash.atknextBool, false);
                requestAtk = false;
                if (requestMove) {
                    lookat.Set(transform.position.x + dx, transform.position.y, transform.position.z + dy);
                    transform.LookAt(lookat);
                } else {
                    //check attack direction,if not face to target,ajust angle
                    if (targetToAttack != null) {
                        if (!targetToAttack.isDead()) {
                            if (!f.inMeleeAttackDirection(targetToAttack)) {
                                changeAttackAngle = true;
                                transform.LookAt(targetToAttack.transform.position);
                            }
                        } else {
                            targetToAttack = null;
                        }
                    }
                }
            } else if (requestAtk) {
                animator.SetBool(Hash.atknextBool, true);
            }
        } else if (state == Hash.idleState) {
            if (requestAtk) {
                requestAtk = false;
                if (!checkRush()) {
                    //check attack direction,if not face to target,ajust angle
                    if (targetToAttack != null) {
                        if (!targetToAttack.isDead()) {
                            if (!f.inMeleeAttackDirection(targetToAttack)) {
                                changeAttackAngle = true;
                                transform.LookAt(targetToAttack.transform.position);
                            }
                        } else {
                            targetToAttack = null;
                        }
                    }
                    animator.SetTrigger(Hash.atkTrigger);
                }
            } else if (requestMove) {
                animator.SetBool(Hash.runBool, true);
            }
        } else if (state == Hash.runState) {
            if (requestAtk) {
                requestAtk = false;
                if (!checkRush()) {
                   // animator.SetBool(Hash.runatkBool, true);
                    animator.SetTrigger(Hash.atkTrigger);
                }
                animator.SetBool(Hash.runBool, false);

            } else if (requestMove) {
                doMove();
            } else if (!requestMove) {
                //animator.SetBool(Hash.runatkBool, false);
                animator.ResetTrigger(Hash.atkTrigger);
                animator.SetBool(Hash.runBool, false);
            }
        }

        bool inIdleRun = state == Hash.idleState || state == Hash.runState;
        if (inIdleRun || stateChange || changeAttackAngle) {
            useAnimatorRotation = useAnimatorPosition = false;
        } else {
            useAnimatorRotation = useAnimatorPosition = true;
        }
        if (!(state == Hash.atk1State || state == Hash.atk2State || state == Hash.atk3State)) {
            f.enableWeaponCollider(false);
        }

    }
    private int preAtkState;
    private FightCharacter targetToAttack;
    private FightCharacter rushTarget;
    private float rushDistance;
    private bool rushingToTarget;
    private float rushTime, rushTimer;
    private float oldAnimatorSpeed;
    private bool checkRush() {
        if (rushTarget == null) {
            checkRushTarget();
        }
        if (rushTarget != null) {
            oldAnimatorSpeed = animator.speed;
            animator.speed *= 1.5f;
            animator.SetBool(Hash.runBool, true);
            Vector3 targetPosition = Vector3.MoveTowards(transform.position, rushTarget.transform.position, rushDistance - 1f);
            transform.LookAt(rushTarget.transform.position);
            iTween.MoveTo(f.model, iTween.Hash("x", targetPosition.x, "z", targetPosition.z, "easeType", iTween.EaseType.easeInExpo, "time", 0.1f));
            rushingToTarget = true;
            rushTime = 0.1f;
            rushTimer = 0;
            return true;
        }
        return false;
    }

    private void checkRushTarget() {
        FightCharacter enemy = BattleEngine.scene.findNearestTarget(f);
        if (enemy == null) return;
        float distance = Vector3.Distance(f.transform.position, enemy.transform.position);
        if (distance < f.getAttackRange() + 1f) {
            targetToAttack = enemy;
            return;
        }
        if (distance < 10) {
            targetToAttack = enemy;
            rushDistance = distance;
            rushTarget = enemy;
        } else {
            targetToAttack = null;
        }
    }

    private void updateInput() {
        //if (Input.GetKeyDown(KeyCode.X)) {
        //    attackx();
        //}
    }
    private void doMove() {
        lookat.Set(transform.position.x + dx, transform.position.y, transform.position.z + dy);
        transform.LookAt(lookat);
        delta.Set(dx * 4 * Time.deltaTime, 0, dy * 4 * Time.deltaTime);
        agent.Move(delta);
    }

    private float dx, dy;
    private bool requestMove, requestAtk;
    private Vector3 lookat = Vector3.zero;
    private Vector3 delta = Vector3.zero;
    public void joymove(float dx, float dy) {
        this.dx = dx;
        this.dy = dy;
        this.requestMove = true;
    }
    public void joystop() {
        requestMove = false;
    }
    
    public void attackx() {
        requestAtk = true;
       

    }
}