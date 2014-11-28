using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using engine;

public class InputManager {


    private GameObject input;
    private GameObject joystick;
    private GameObject uiCamera;
   // private GameObject attackBar;
    

    private bool inited;

    private EasyJoystick easyjoy;
    
    //private Dictionary<string,AttackButton> attackButtons=new Dictionary<string,AttackButton>();
    //private AttackButton[] attackButonArray;
    private Dictionary<int, AttackButton> boundSkills = new Dictionary<int, AttackButton>();

    public InputManager(GameObject input) {
        this.input = input;
    }
    
    public void init(bool showJoystick = false) {
        if (!inited) {
            inited = true;
            input.SetActive(true);
            doInit();
            input.SetActive(false);
        }
        
        input.SetActive(showJoystick);
        joystick.SetActive(showJoystick);
    }

    public void setJoyEnabled(bool v)
    {
        if(easyjoy==null) easyjoy = joystick.GetComponent<EasyJoystick>();
        easyjoy.JoystickTouch = Vector2.zero;
        easyjoy.enable = v;
        Player.instance.setJoyEnabled(v);
    }
   
    private void doInit() {
        joystick = input.getChild("moveJoystick");
        uiCamera = input.getChild("uiCamera");
        //attackBar = input.getChild("attackBar");

        //initAttackBar();
    }
    //private void initAttackBar() {
    //    Transform atkBar = attackBar.transform;
    //    int count=atkBar.childCount;
    //    for (int i = 0; i < count; i++) {
    //        Transform child=atkBar.GetChild(i);
    //        if (child.name.StartsWith(Naming.AttackButtonPrefix)) {
    //            AttackButton button = new AttackButton();
    //            button.name = child.name;
    //            button.init(child.gameObject);
    //            attackButtons[child.name]=button;
    //            Click.get(button.go).onClick = onAttackButtonClick;
    //        }
    //    }
    //    attackButonArray = new AttackButton[attackButtons.Count];
    //    int index = 0;
    //    foreach (AttackButton button in attackButtons.Values) {
    //        attackButonArray[index++] = button;
    //    }
    //}

    //private void onAttackButtonClick(GameObject button) {
    //    AttackButton atkButton = getAttackButton(button);
    //    atkButton.fire();
    //}
    //private AttackButton getAttackButton(GameObject button) {
    //    for (int i = 0, max = attackButonArray.Length; i < max; i++) {
    //        if (attackButonArray[i].go == button) return attackButonArray[i];
    //    }
    //    return null;
    //}
    //public void bindAttackButton(string buttonName, LearnedSkill skill) {
    //    for (int i = 0, max = attackButonArray.Length; i < max; i++) {
    //        if (attackButonArray[i].name == buttonName) {
    //            attackButonArray[i].setSkill(skill);
    //            boundSkills[skill.skillId] = attackButonArray[i];
    //            return;
    //        }
    //    }
    //    Debug.Log("can't bind skill "+skill.template.name+" to attack button:" + buttonName);
    //}
    //simulate clicking attack button
    //public void clickAttackButton(int skillId) {
    //    if (boundSkills.ContainsKey(skillId)) {
    //        AttackButton button = boundSkills[skillId];
    //        button.fire();
    //    }
    //}
    public void update() {
        //for (int i = 0, max = attackButonArray.Length; i < max; i++) attackButonArray[i].update();

    }
}
