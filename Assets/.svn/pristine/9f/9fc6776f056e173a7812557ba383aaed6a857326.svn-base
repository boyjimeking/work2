﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using engine;

public class BattleWin : MonoBehaviour {
    private Transform winCamera;
    private GameObject mainCamera;
    private GameObject uiCamera;
    private Animator animator;
    private GameObject battleWin;
    private FightCharacter hero1;
    private FightCharacter hero2;
    private Vector3 zMove;
    private bool windialogshowed;

    void Awake() {  
        battleWin = Instantiate(Resources.Load("Local/UI/bossUI/bossBorn")) as GameObject;
        winCamera = battleWin.T("Camera");
        uiCamera = battleWin.T("UI/Camera").gameObject;
        uiCamera.SetActive(false);
        Vector3 lookPos = transform.position;
        zMove = Quaternion.Euler(transform.localEulerAngles) * Vector3.forward;
        Vector3 offset = transform.position + zMove * CommonTemp.winOffset;  
        offset.y = lookPos.y + CommonTemp.winHeight;
        winCamera.position = offset;  //final camera pos
        winCamera.camera.fieldOfView = CommonTemp.winView;
        lookPos.y = lookPos.y + CommonTemp.winLookY;
        winCamera.LookAt(lookPos);
        battleWin.SetActive(false);
        mainCamera = Camera.main.gameObject;
        foreach (FightCharacter c in BattleEngine.scene.getFriends()) {
            if (c.isHero()) {
                if (hero1 == null)
                    hero1 = c;
                else if (hero2 == null)
                    hero2 = c;
            }
        }
        Combo.instance.setActive(false);
    }

    void OnAnimatorMove() {

    }

    public void onBegin() {
        App.suspend = true;
        UIManager.Instance.Enable = false;
        Time.timeScale = 1f;
        mainCamera.SetActive(false);
        uiCamera.SetActive(false);
        battleWin.SetActive(true);
        animator = GetComponent<Animator>();
        animator.SetBool(Hash.runBool, false);
        animator.Play(Hash.winState);
        Transform p = Player.instance.transform;
        
        if (hero1 != null) {
            hero1.setAgentEnable(false);
            hero1.ai.enabled = false;
            hero1.transform.position = transform.position - zMove * CommonTemp.petPosOffset.z + transform.right * CommonTemp.petPosOffset.x;
            Animator animator1 = hero1.transform.GetComponent<Animator>();
            animator1.SetBool(Hash.runBool, false);
            animator1.SetTrigger(Hash.atkTrigger);
            hero1.transform.localEulerAngles = transform.localEulerAngles;
        }
        if (hero2 != null) {
            hero2.setAgentEnable(false);
            hero2.ai.enabled = false;
            //hero2.transform.position = transform.position - BattleConfig.petPos.x * transform.right - transform.forward * BattleConfig.petPos.z;
            hero2.transform.position = transform.position - zMove * CommonTemp.petPosOffset.z - transform.right * CommonTemp.petPosOffset.x;
            Animator animator2 = hero2.transform.GetComponent<Animator>();
            animator2.SetBool(Hash.runBool, false);
            animator2.SetTrigger(Hash.atkTrigger);
            hero2.transform.localEulerAngles = transform.localEulerAngles;
        }
        windialogshowed = false;
        App.coroutine.StartCoroutine(showWinDialog());
        addListener();
    }
    public IEnumerator showWinDialog()
    {
        yield return new WaitForSeconds(CommonTemp.showwindelay);
        if (!windialogshowed)
        {
            windialogshowed = true;
            DungeonFinish.instance.show(true, this);
            removeListener();
        }
        
    }
    private void addListener()
    {
        EasyTouch.On_TouchUp += onClick;
    }
    private void removeListener()
    {
        EasyTouch.On_TouchUp -= onClick;
    }
    private void onClick(Gesture gesture)
    {
        if (windialogshowed) return;
        windialogshowed = true;
        DungeonFinish.instance.show(true, this);
        removeListener();
    }
    public void onEnd() {
        UIManager.Instance.Enable = true;
        App.suspend = false;
        Destroy(battleWin);
        Destroy(GetComponent<BattleWin>());
        mainCamera.SetActive(true);
    }
}
