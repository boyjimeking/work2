using UnityEngine;
using System.Collections;
using engine;

public class SpecialMonsterAI : MonsterAI {
    private enum State{
        Flee,Wait,Follow
    }
    public bool playerEnteredRoom;

    private Transform playerTransform;
    private float stayFreeTime=5f;
    private bool tryFollow;
    private State state = State.Wait;

    private float StayFreeTime = 5f;
    private float FleeThinkTime = 0.1f;
    private float originalAgentSpeed;
    private float fleeThinkTime;

    public override void reset(FightCharacter fc) {
        base.reset(fc);
        playerTransform = Player.instance.transform;
        stayFreeTime = 5f;
        playerEnteredRoom=useOppositePosition = false;
        firstFlee = true;
        stuckCount = 0;

    }
    //fix getting stuck in corner case
    private Vector3 preFleePosition;
    private bool firstFlee=true;
    private int stuckCount;
    private bool useOppositePosition;
    private Vector3 oppositePosition;

    public override void update() {
        if (!playerEnteredRoom||!enabled||Player.instance.isDead()) return;
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        if (distance < owner.data.moveRanage) {
            if (fleeThinkTime > 0) {
                fleeThinkTime -= Time.deltaTime;
                if (fleeThinkTime > 0) return;
            }
            
            Vector3 fleePosition =useOppositePosition?oppositePosition: owner.transform.position + (owner.transform.position - playerTransform.position).normalized * 3;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(fleePosition, out hit, 100f, Player.instance.agent.walkableMask)) {
                fleePosition = hit.position;
                owner.agent.speed =owner.data.moveSpeed* 1.1f;
                owner.animator.speed = 1.1f;

                owner.agentMoveTo(fleePosition);
                if (firstFlee) {
                    firstFlee = false;
                    preFleePosition = owner.transform.position;
                } else {
                    if (owner.transform.position == preFleePosition) {
                        stuckCount++;
                        if (stuckCount > 100) {
                            stuckCount = 0;
                            useOppositePosition = true;
                            oppositePosition = fleePosition - owner.transform.forward * 10f;
                        } 
                    }
                    preFleePosition = owner.transform.position;
                }
            }
            stayFreeTime = StayFreeTime;
        } else {
            useOppositePosition = false;
            if (wait()) {
                fleeThinkTime = FleeThinkTime;
                owner.animator.speed = 1;
                owner.agent.speed = owner.data.moveSpeed;
                owner.lookat(Player.instance);
                owner.agentStop();
            } else {
                fleeThinkTime = FleeThinkTime;
                owner.animator.speed = 1;
                owner.agent.speed = owner.data.moveSpeed;
                if (distance > owner.data.moveRanage + 1f) {
                    owner.agentMoveTo(playerTransform.position);
                } else {
                    stayFreeTime = StayFreeTime;
                }
            }
        }
    }
    private bool wait() {
        stayFreeTime -= Time.deltaTime;
        if (stayFreeTime > 0) return true;
        return false;
    }
}
