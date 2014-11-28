using UnityEngine;
using System.Collections;
using engine;

public class BattleUI  {
    public static BattleUI instance = new BattleUI();
    public GameObject Prefab_HeadUI;

    PetPosition[] positions = new PetPosition[] { PetPosition.leftforward, PetPosition.rightforward };

    
    private bool inited;
    public bool enable;
    public bool actived;

    public void reset()
    {
        if (!inited)
        {
            inited = true;
            doInit();
        }

        
    }
    private void doInit()
    {
        PetHeads.instance.init();
 		HeadController.instance.init();
        FightSkill.instance.init();
        BossHead.instance.init();
		Combo.instance.init();
        PauseAndAuto.instance.init();

        enable = true;
        actived = true;
        //for (int i = 0; i < 2; i++)
        //{
        //    PetHead head = new PetHead();
        //    head.init(GameObject.Instantiate(headPrefab) as GameObject);
        //    petHeads[i] = head;
        //}
    }
   
    /// <summary>
    /// 召唤伙伴
    /// </summary>
    /// <param name="index"></param>
    public bool summonPet(int index) {
        PetData data = PlayerData.instance.getPetData(index);
        if (data==null || data.summoned) return false;
        data.summoned=true;
        PetBorn born = new PetBorn();
        born.beginBorn(index, data);
        //Pet c = CharFactory.createPet(data, positions[index], Player.instance.agent.walkableMask);
        //c.agent.walkableMask = Player.instance.agent.walkableMask;
        //c.uiIndex = index;
        //BattleEngine.scene.addFriend(c);
        //c.playSummon();
        return true;       
    }

    public void update()
    {
        if (!inited) return;
        FightSkill.instance.update();
        Combo.instance.update();
        RedScreen.instance.update();
        BossHead.instance.update();
    }

    public void setActive(bool value)
    {
        actived = value;
        PetHeads.instance.setActive(value);
        HeadController.instance.setActive(value);
        FightSkill.instance.setActive(value);
        BossHead.instance.setActive(value);
        Combo.instance.setActive(value);
        PauseAndAuto.instance.setActive(value);
    }
    public void clear()
    {
        PetHeads.instance.clear();
        HeadController.instance.clear();
        FightSkill.instance.clear();
        BossHead.instance.clear();
        Combo.instance.clear();
        enable = true;
    }
    public void setEnabled(bool enable)
    {
        this.enable = enable;
    }
}
