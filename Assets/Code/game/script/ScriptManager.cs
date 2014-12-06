using UnityEngine;
using System.Collections;
using engine;
using WellFired;

public class ScriptState
{
    public Script script;
    public int currStep = 0;
    public int talkID = 0;
    public int currSingle = 0;
    public GameObject currShot;
    public Camera camera;
    public PlayBornSript pb;
    public void reset(Script script)
    {
        this.script = script;
        this.currStep = 0;
        this.currSingle = 0;
    }

    public string getTalkContent()
    {
        if (currStep >= script.steps.Count)
            return "";
        ScriptStep step = script.steps[currStep];
        if (step == null) return "";
        if (currSingle >= step.talks.Count)
            return "";
        ScriptStepSingle single = step.talks[currSingle];
        return single != null ? single.talk : "";
    }
    public string getTalkName() {
        ScriptStep step = script.steps[currStep];
        if (step == null) return "";
        int charId = step.charId;
        if(charId==0){
            return PlayerData.instance.name;
        }else{
            CharTemplate tmp = App.template.getTemp<CharTemplate>(charId);
            if(tmp==null) return "";
            return tmp.name;
        }
    }
    public bool next(){
        bool b = nextSingle();
        if (!b)//没有下一句话
        {
            bool bb = nextStep(false);
            if (!bb)//剧情结束
            {
                return false;
            }
        }
        return true;
    }
     

    public bool nextSingle() {
        currSingle++;
        ScriptStep step = script.steps[currStep];
        if (currSingle >= step.talks.Count) {
            return false;
        }
        return true;
    }

    public bool nextStep(bool first)
    {
        
//         if(!first){
//             currStep++;
//             if (currStep >= script.steps.Count)
//             {
//                 //剧情结束
//                 if (pb != null)
//                 {
//                     pb.OnPalyEnd();
//                 }
//                return false;
//             }
//             currSingle = 0;
//         }

        if (pb != null)
        {
            pb.OnPalyEnd();
        }
        
        
        BattleEngine.scene.pause(true);
       
        playCurrShot2(currStep);
        return true;
    }

    public void playCurrShot2(int value)
    {
        ScriptStep step = script.steps[currStep];
        currShot = App.res.createSingle("Local/prefab/Movie/" + step.prefab);
        pb = currShot.transform.Find("Moviecamera/movieCamera").GetComponent<PlayBornSript>();
        pb.onBegin(CameraManager.CameraFollow.target.transform);
        FightCharacter c = null;
        if (step.charId == 0)
        {
            if (Player.instance.isDead())
            {
                foreach (FightCharacter cc in BattleEngine.scene.getFriends())
                {
                    if (cc.model == CameraManager.CameraFollow.target.gameObject)
                    {
                        c = cc;
                        break;
                    }
                }
            }
            else c = Player.instance;
        }
        else
        {
            c = BattleEngine.scene.getEmemy(step.charId);
        }

        if (c != null)
        {
            c.model.SetActive(true);
            c.pauseAnimator(false);
        }
        
    }

    //private void playCurrShot() {
    //    ScriptStep step = script.steps[currStep];
    //    currShot = App.res.createSingle("Local/sequence/scriptClip/" + step.prefab + "/Sequence");
    //    USTimelineContainer container = currShot.getChild("Container").GetComponent<USTimelineContainer>();
    //    USTimelineObjectPath path = currShot.getChild("Container/path").GetComponent<USTimelineObjectPath>();
    //    USLookAtObjectEvent lookAtEvent = currShot.getChild("Container/Timeline/USLookAtObjectEvent").GetComponent<USLookAtObjectEvent>();
    //    USSequencer seq = currShot.GetComponent<USSequencer>();
    //    camera = currShot.T("Camera").GetComponent<Camera>();
    //    container.AffectedObject = camera.transform;
    //    FightCharacter c = null;
    //    if (step.charId == 0) {//主角
    //        lookAtEvent.objectToLookAt = CameraManager.CameraFollow.target.gameObject;
    //        if (Player.instance.isDead())
    //        {
    //            foreach (FightCharacter cc in BattleEngine.scene.getFriends())
    //            {
    //                if (cc.model == CameraManager.CameraFollow.target.gameObject)
    //                {
    //                    c = cc;
    //                    break;
    //                }
    //            }
    //        }
    //        else c = Player.instance;
    //    } else {
    //        c = BattleEngine.scene.getEmemy(step.charId);
    //        lookAtEvent.objectToLookAt = c.model;
    //    }
        
    //    c.model.SetActive(true);
    //    c.pauseAnimator(false);
    //    seq.Play();
    //}
}

public class ScriptManager {

    public static ScriptManager instance = new ScriptManager();
    public GameObject ui;
    public UILabel talkName;
    public UILabel talkContent;
    public GameObject clickObject;
    public ScriptState state;
    public bool scripting;

   
    //complete callback
    public delegate void OnComplete();
    public OnComplete onComplete;

    public void RoomTrigger(int roomID)
    {
        if (roomID == 1)
        {
            trigger(2);
        }
        else if (roomID == 2)
        {
            if (onComplete != null)
                onComplete();
            onComplete = null;
            return;
        }
    }
    public void trigger(int scriptId) {
        //auto fight, dead status not play scenario
        if (Player.instance == null || Player.instance.autoFight || Player.instance.isDead()) {
            if (onComplete != null)
                onComplete();
            onComplete = null;
            return;
        }
        scripting = true;
        Script script = App.template.getTemp<Script>(scriptId);
        if (script == null) return;
        if (state == null) state = new ScriptState();
        state.reset(script);
        if (ui == null) {
            ui = App.res.createSingle("Local/prefab/script/script");
            talkName = ui.getChildComponent<UILabel>("talkName");
            talkContent = ui.getChildComponent<UILabel>("talkContent");
            clickObject = ui.getChildComponent<UIWidget>("clickObject").gameObject;
            //Click.set(clickObject.gameObject, onClick);
        }

        start();
    }

    private void start()
    {
        UIManager.Instance.Active = false;
        CameraManager.Main.gameObject.SetActive(false);
        foreach (FightCharacter c in BattleEngine.scene.getFriends())
        {
            c.HpBar.Parent.SetActive(false);
        }
        foreach (FightCharacter c in BattleEngine.scene.getEnemies())
        {
            c.HpBar.Parent.SetActive(false);
        }
        //ui.SetActive(true);
        state.nextStep(true);
        //updateData();
    }

    private void end()
    {
        scripting = false;
        UIManager.Instance.Active = true;
        ui.SetActive(false);
//         CameraManager.Main.gameObject.SetActive(true);
//         foreach (FightCharacter c in BattleEngine.scene.getFriends())
//         {
//             c.HpBar.Parent.SetActive(true);
//         }
//         foreach (FightCharacter c in BattleEngine.scene.getEnemies())
//         {
//             c.HpBar.Parent.SetActive(true);
//         }
        BattleEngine.scene.pause(false);

        //BattleEngine.scene.shieldCharacter(false);

        if (onComplete != null)
        {
            OnComplete cb = this.onComplete;
            this.onComplete = null;
            cb();
        }
    }

    private void updateData()
    {
        if (state.getTalkName().Equals(""))
        {
           
        }
        talkName.text = state.getTalkName()+"：";
        talkContent.text = state.getTalkContent();
    }

    public void onClick(GameObject go) {

        //if (state.next())
        //    updateData();
        //else end();
    }

    public void showNextDilage(int talkSingle)
    {
        ui.SetActive(true);
        state.currSingle = talkSingle;
        updateData();
    }

    public void CloseScript()
    {
        end();
    }

}
