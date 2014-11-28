using System.Collections;
using System.Collections.Generic;
using engine;
using UnityEngine;

public class BossBorn : MonoBehaviour {
    public delegate void spawnMonster(int[] monsterIds, DungeonTemplate.SpawnGroup group, Team team);

    private Transform bossCamera;
    private GameObject mainCamera;
    private Animator animator;
    private Transform top;
    private Transform bottom;
    private Transform bossBg;
    private Transform bossName;
    private UILabel bossNameLabel;
    private int anchorNum = 0;
    private GameObject bossBorn;
    private float origScale; 

    public float ratio = 8;
    public float yOffset = 2;
    public float lookHeight = 1.45f;
    public float scaleTime = 0.25f;

    public float xDelta= 420;
    public float yDelta = 120;
    public float alpha = 0.2f;
    public float tweenTime = 0.1f;
    public float alphaTime = 1f;
    public float nameTime = 0.2f;
    public float finalTime = 0.1f;
    public spawnMonster callBack;

    void Awake() {
        ratio = CommonTemp.bossOffset;
        yOffset = CommonTemp.bossHeight;
        lookHeight = CommonTemp.bossLookY;
        scaleTime = CommonTemp.bossScale;
        xDelta = CommonTemp.xDelta;
        yDelta = CommonTemp.yDelta;
        alpha = CommonTemp.initAlpha;
        tweenTime = CommonTemp.tweenT;
        alphaTime = CommonTemp.alphaT;
        nameTime = CommonTemp.nameT;
        finalTime = CommonTemp.finalT;

        bossBorn = Instantiate(Resources.Load("Local/UI/bossUI/bossBorn")) as GameObject;
        bossCamera = bossBorn.T("Camera");
        top = bossBorn.T("UI/Camera/top");
        bottom = bossBorn.T("UI/Camera/bottom");
        bossBg = bossBorn.T("UI/Camera/bossInfo");
        bossName = bossBorn.T("UI/Camera/bossName");
        bossNameLabel = bossName.GetComponent<UILabel>();
        Vector3 lookPos = transform.position;
        Vector3 angle = transform.localEulerAngles;
        Vector3 zMove = Quaternion.Euler(angle) * Vector3.forward;
        Vector3 offset = transform.position + zMove * ratio;
        offset.y = lookPos.y + yOffset;
        bossCamera.position = offset;
        lookPos.y = lookPos.y + lookHeight;
        bossCamera.LookAt(lookPos);
        bossBorn.SetActive(false);
        mainCamera = Camera.main.gameObject;
        //onBegin();
    }

    void Update() {
        if (anchorNum == 1) {
            top.localPosition = top.T2V(yDelta, 1);
            bottom.localPosition = bottom.T2V(-yDelta, 1);
            bossBg.localPosition = bossBg.T2V(xDelta, 0);
            bossName.localPosition = bossName.T2V(xDelta, 0);
            setAlpha(top);
            setAlpha(bottom);
            setAlpha(bossName);
            setAlpha(bossBg);
            top.GetComponent<UIAnchor>().enabled = false;
            bottom.GetComponent<UIAnchor>().enabled = false;
            bossBg.GetComponent<UIAnchor>().enabled = false;
            bossName.GetComponent<UIAnchor>().enabled = false;
        }
        anchorNum++;
    }

    void OnAnimatorMove() {

    }

    void showUI(AnimationEvent ev) {
        int cond = ev.intParameter;
        if (cond == 0) {  //0 begin show Boss UI
            origScale = Time.timeScale;
            Time.timeScale = scaleTime;         
            Vector3 vec1 = top.parent.TransformPoint(top.T2V(-yDelta, 1));
            Hashtable ht1 = iTween.Hash("position", vec1, "easeType", iTween.EaseType.linear, "time", tweenTime);
            iTween.MoveTo(top.gameObject, ht1);
            iTween.FadeTo(top.gameObject, 1f, tweenTime);
            Vector3 vec2 = bottom.parent.TransformPoint(bottom.T2V(yDelta, 1));
            Hashtable ht2 = iTween.Hash("position", vec2, "easeType", iTween.EaseType.linear, "time", tweenTime);
            iTween.MoveTo(bottom.gameObject, ht2);
            Vector3 vec3 = bossBg.parent.TransformPoint(bossBg.T2V(-xDelta, 0));
            Hashtable ht3 = iTween.Hash("position", vec3, "easeType", iTween.EaseType.linear, "time", tweenTime);
            iTween.MoveTo(bossBg.gameObject, ht3);
            Vector3 vec4 = bossName.parent.TransformPoint(bossName.T2V(-xDelta, 0));
            Hashtable ht4 = iTween.Hash("position", vec4, "easeType", iTween.EaseType.linear, "time", tweenTime);
            iTween.MoveTo(bossName.gameObject, ht4);
            Vector3 vec5 = bossName.parent.TransformPoint(bossName.T2V(-xDelta - 30, 0));
            Hashtable ht5 = iTween.Hash("delay", tweenTime, "position", vec5, "easeType", iTween.EaseType.linear, "time",
                nameTime);
            iTween.MoveTo(bossName.gameObject, ht5);
            setAlpha(top, true);
            setAlpha(bottom, true);
            setAlpha(bossBg, true);
            setAlpha(bossName, true);
        }
        else { //1 hidden UI
            top.localPosition = top.T2V(yDelta, 1);
            bottom.localPosition = bottom.T2V(-yDelta, 1);
            bossBg.localPosition = bossBg.T2V(xDelta, 0);
            iTween.Stop(bossName.gameObject);
            Vector3 vec5 = bossName.parent.TransformPoint(new Vector3(-xDelta, bossName.localPosition.y, bossName.localPosition.z));
            Hashtable ht5 = iTween.Hash("position", vec5, "easeType", iTween.EaseType.linear, "time", finalTime);
            iTween.MoveTo(bossName.gameObject, ht5);
            StartCoroutine(endUI());
        }
    }

    void setAlpha(Transform trans, bool tween = false) {
        if (tween) {
            foreach (Transform child in trans) {
                TweenAlpha.Begin(child.gameObject, alphaTime, 1);
            }
        }
        else {
            foreach (Transform child in trans) {
                UIWidget wd = child.GetComponent<UIWidget>();
                wd.alpha = alpha;
            }
        }       
    }

    IEnumerator endUI() {
        yield return new WaitForSeconds(finalTime);
        onEnd();
    }

    private int[] monsterIds;
    private DungeonTemplate.SpawnGroup group;
    private Team team;
    private List<Transform> oldNoActives = new List<Transform>();

    public void onBegin(int[] monsterIds, DungeonTemplate.SpawnGroup group, Team team) {
        UIManager.Instance.Enable = false;
        bossBorn.SetActive(true);
        mainCamera.SetActive(false);
        UIManager.Instance.uiCamera.gameObject.SetActive(false);
        oldNoActives.Clear();
        foreach (Transform t in UIManager.Instance.RootTrans) {
            if (!t.gameObject.activeSelf) oldNoActives.Add(t);
            t.gameObject.SetActive(false);
        }
        animator = GetComponent<Animator>();
        animator.Play(Hash.summonState);
        this.monsterIds = monsterIds;
        this.group = group;
        this.team = team;
        CharTemplate template = App.template.getTemp<CharTemplate>(group.bossID);
        bossNameLabel.fontSize = 40;
        bossNameLabel.text = template.name;
        App.suspend = true;
    }

    private void onEnd() {
        UIManager.Instance.Enable = true;
        Destroy(bossBorn);
        Destroy(GetComponent<BossBorn>());
        mainCamera.SetActive(true);
        foreach (Transform t in UIManager.Instance.RootTrans)
        {
            if (!oldNoActives.Contains(t)) t.gameObject.SetActive(true);
        }
        oldNoActives.Clear();
        UIManager.Instance.uiCamera.gameObject.SetActive(true);
        Time.timeScale = origScale;
        App.suspend = false;
        if (callBack != null)
        {
            callBack(monsterIds, group, team);
        }
    }
}
