using UnityEngine;
using System.Collections;
using engine;

public class OtherPlayer : FightCharacter {

    protected override void updateAnimatorState() {

        if (agent.enabled &&agent.remainingDistance<0.1f) {
            stop();
        }
        base.updateAnimatorState();
    }

    private static Vector3 moveV = new Vector3();
    public void move(float x, float z) {
        controller.setBool(Hash.runBool, true);
        moveV.Set(x, transform.position.y, z);
        transform.LookAt(moveV);
        agent.SetDestination(moveV);

    }
    public void stop() {
        agent.Stop();
        controller.setBool(Hash.runBool, false);
    }
   

    protected override string getAnimatorControllerName() {
        return "Local/controller/PlayerController";
    }

}
