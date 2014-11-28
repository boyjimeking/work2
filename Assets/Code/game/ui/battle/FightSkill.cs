using UnityEngine;
using System.Collections;
using engine;

public class FightSkill{
    public static FightSkill instance = new FightSkill();

    public class FightSkillItem : AttackButton{
        public Transform trans;
        public UITexture icon;
        public UISprite mask,bg;
        public bool canClick = true;
        public void load(Transform _trans)
        {
            trans = _trans;
            bg = trans.FindChild("bg").GetComponent<UISprite>();
            icon = trans.FindChild("icon").GetComponent<UITexture>();
            mask = trans.FindChild("mask").GetComponent<UISprite>();
            init2(trans.gameObject,mask);
        }

        public override void onCdFinish()
        {
            base.onCdFinish();
            canClick = true;
            go.transform.localScale = Vector3.one;
        }
    }

    private GameObject go;
    protected FightSkillItem[] skillitems;
    protected GameObject normalSkill;
    protected UISprite atkIcon;
    protected bool inited = false;
    public void init(){
        if (inited) return;
        go = Engine.res.createSingle("Local/prefab/battleui/fightskill");
        Transform transform = go.transform;
        skillitems = new FightSkillItem[3];
        for (int i = 0; i < 3; i++)
        {
            FightSkillItem fsi = new FightSkillItem();
            Transform trans = transform.Find("fightskillitem" + i);
            fsi.load(trans);
            skillitems[i] = fsi;
            addClick(trans.gameObject);
        }
        normalSkill = transform.Find("normalskill").gameObject;
        atkIcon = normalSkill.getChildComponent<UISprite>("atkicon");
        addClick(normalSkill.gameObject);
        inited = true;
    }
    private void addClick(GameObject go)
    {
        Click click = Click.get(go);
        click.onClick = onAttackButtonClick;
        click.onPress = onAttackButtonPress;
    }
    private void removeClick(GameObject go)
    {
        Click click = Click.get(go);
        click.onClick = null;
        click.onPress = null;
    }
    public void clear() {
        foreach (FightSkillItem fsi in skillitems)
        {
            fsi.onCompleted();
        }
    }
    private void onAttackButtonPress(GameObject button, bool isPressed)
    {
        foreach (FightSkillItem fsi in skillitems){                   
            if (button == fsi.go){
                if (!fsi.canClick) return;
            }
        }
        if (!isPressed)//松手
        {
            button.transform.localScale = Vector3.one;
            if (button == normalSkill) atkIcon.spriteName = "ptjn1";
        }
        else
        {
            button.transform.localScale = Vector3.one * 1.1f;
            if (button == normalSkill) atkIcon.spriteName = "ptjn2";
        }     
        
    }
    private void onAttackButtonClick(GameObject button)
    {
        if (!BattleUI.instance.enable || Player.instance.isDead()) return;
        foreach (FightSkillItem fsi in skillitems){
            if (button == fsi.go){
                if (!fsi.canClick) return;
            }
        }
        if (button == normalSkill.gameObject){
            Player.instance.attackx();
        }else{
            foreach (FightSkillItem fsi in skillitems){
                if (button == fsi.go) {
                    if (fsi.fire()){
                        fsi.canClick = false;
                        fsi.go.transform.localScale = Vector3.one * 0.8f;
                    }
                    break;
                }
            }
        }

    }
    public void loadSkill()
    {
       init();
       LearnedSkill[] skills =  Player.instance.data.getAllLearnedSkills();
       int i = 0;
       bool has = false ;
       atkIcon.spriteName = "ptjn1";
       foreach(FightSkillItem fsi in  skillitems)
       {
           has = false;
           for (; i < skills.Length;) {
               if (skills[i].skillId >= 50){
                   fsi.setSkill(skills[i]);
                   fsi.icon.mainTexture = Engine.res.loadObject("Local/icon/" + skills[i].template.icon) as Texture2D;
                   fsi.go.transform.localScale = Vector3.one;
                   fsi.icon.gameObject.SetActive(true);
                   fsi.canClick = true;
                   has = true;
                   i++;
                   break;
               }
               i++;
           }
           if (!has){
               fsi.bg.spriteName = "lock";
               fsi.icon.gameObject.SetActive(false);
               fsi.go.transform.localScale = Vector3.one * 0.8f;
               fsi.canClick = false;
           }
       }
    }

   public void update()
   {
       if (!inited) return;
       foreach (FightSkillItem fsi in skillitems) fsi.update();
   }
   public void setActive(bool value)
   {
       go.SetActive(value);
   }
}
