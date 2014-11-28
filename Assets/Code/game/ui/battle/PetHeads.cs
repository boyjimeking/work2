using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using engine;

//单个的宠物头像
public class PetHead
{
    public enum PetState { 
        fight, rest, die
    }
    public static PetHead instance = new PetHead();


    public Transform petHead;
    private int index;
    private UITexture icon;
    private UISprite hp, down, die;
    public PetState pState;
    private PetData _data;

    public void init(Transform petHead,int index)
    {
        this.petHead = petHead;
        this.index = index;
        icon = petHead.gameObject.getChild("icon").GetComponent<UITexture>();
        hp = petHead.gameObject.getChild("up").GetComponent<UISprite>();
        down = petHead.gameObject.getChild("down").GetComponent<UISprite>();
        die = petHead.gameObject.getChild("die").GetComponent<UISprite>();
        hp.gameObject.addOnce<ChangeHp>();
        Click.set(petHead.gameObject, onHeadClick);
        //setState(PetState.rest);
    }
    public void setData(PetData data)
    {
        this._data = data;
        if (data == null)
        {
            petHead.gameObject.SetActive(false);
        }
        else
        {
            petHead.gameObject.SetActive(true);
            icon.mainTexture = Engine.res.loadObject("Local/icon/pet"+(data.charDataTemplate.id-4999)) as Texture2D;
            hp.spriteName = "pethp1";
            hp.fillAmount = 1f;
        }
        setState(PetState.rest);
    }
    public void setState(PetState pstate)
    {
        this.pState = pstate;
        if(pstate==PetState.fight){
            petHead.transform.localScale = Vector3.one;
            icon.shader = null;
            die.gameObject.SetActive(false);
        }else if(pstate==PetState.rest){
            petHead.transform.localScale = Vector3.one * 0.8f;
            icon.shader = null;
            die.gameObject.SetActive(false);
        }else if(pstate==PetState.die){
            petHead.transform.localScale = Vector3.one;
            icon.shader = App.res.loadObject("Local/Shader/GrayShader") as Shader;
            die.gameObject.SetActive(true);
        }
    }
    public void setValue(float v)
    {
        float fillend = 0.75f * v + 0.25f;
        Hashtable ht = iTween.Hash("from", hp.fillAmount, "to", fillend, "time", 0.5f, "easeType",
             iTween.EaseType.easeInCubic, "onupdate", "changeHp");
        iTween.ValueTo(hp.gameObject, ht);   
        if (v < 0.5f) hp.spriteName = "pethp2";
        else hp.spriteName = "pethp1";

    }
    private void onHeadClick(GameObject head)
    {
        if (!BattleUI.instance.enable || Player.instance.isDead()) return;
        if (pState != PetState.rest || _data==null) return;
        bool sommonSuc = BattleUI.instance.summonPet(index);
        if(sommonSuc) setState(PetState.fight);
    }
    public void clear() {
        setState(PetState.rest);
    }

    class ChangeHp : MonoBehaviour
    {
        public UISprite blood;

        void changeHp(float value)
        {
            if (blood == null) blood = gameObject.GetComponent<UISprite>();
            blood.fillAmount = value;
        }
    }
}
public class PetHeads {

    public static PetHeads instance = new PetHeads();

    private GameObject headContainer;

    private PetHead[] heads;
    public void init()
    {

        headContainer = App.res.createSingle("Local/prefab/battleui/petHeadContainer");
        heads = new PetHead[2];
        for (int i = 0; i < 2; i++)
        {
            PetHead head = new PetHead();
            head.init(headContainer.getChild("petHead"+(i+1)).transform, i);
            heads[i] = head;
            setPetData(i);
        }
    }
    public void clear()
    {
        for (int i = 0; i < heads.Length; i++)
        {
            setPetData(i);
        }
    }

    public void setPetData(int index) {
        PetData petData = PlayerData.instance.getPetData(index);
        heads[index].setData(petData);
    }

    public void onDead(int index) {
        PetHead h = heads[index];
        if (h != null) {
            h.setState(PetHead.PetState.die);
        }
    }

    public void setPetBlood(int index, float value)
    {
        PetHead h = heads[index];
        if (h != null)
        {
            h.setValue(value);
        }
    }
    public void setActive(bool value)
    {
        headContainer.SetActive(value);
    }
}
