using UnityEngine;
using System.Collections;
using engine;

public class Monster : MonsterCharacter {

    public float delay;//出现的时间
    public bool actived;
    public bool appeared;

    public override void update()
    {
      
       if(actived){
           base.update();
           if (appeared && stateInfo.nameHash != Hash.appearState)
           {
               appeared = false;
               ai.enabled = true;
               if (cc != null) cc.enabled = true;
               if (!isBoss() && deadable)  ArrawManager.instance.addEnemy(this);
           }
           return;
       }
       if (ScriptManager.instance.scripting) return;
       if(isDead())return;
       delay += -Time.deltaTime;
       if (delay <= 0)
       {
           playAppear();
           delay = 0.5f;
       }   
    }

    public void playAppear()
    {
        appeared = true;
        model.SetActive(true);
        actived = true;
        setObstacleMode(false);
        if (cc != null) cc.enabled = false;
        
    }
	
}
