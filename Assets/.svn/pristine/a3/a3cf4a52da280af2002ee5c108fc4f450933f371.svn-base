using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using engine;

public class ArrawManager {
    public class ArrawObject {
        public FightCharacter c;
        public GameObject dirArraw;
        //public GameObject monsterArraw;

        public ArrawObject(FightCharacter c) {
            this.c = c;
            dirArraw = App.res.createSingle("Local/prefab/arraw/monsterArraw");
            //monsterArraw = App.res.createSingle("Local/prefab/arraw/monsterArraw");
            //monsterArraw.SetActive(false);
        }

        public void destroy() {
            GameObject.Destroy(dirArraw);
            //GameObject.Destroy(monsterArraw);
        }
    }

    public static ArrawManager instance = new ArrawManager();
    private float rectW = Screen.width-40, rectH = Screen.height-40;
    private Camera worldCamera, uiCamera;
    private float staticAngle;
   
    private GameObject copyArraw;
    private Vector3 copyPosition;
    
    private bool inited;
    //private GameObject playerObject;
    private Player player;
    private bool copyArrawEnable;


    /**小怪指向箭头*/
    private List<ArrawObject> monsterArraws = new List<ArrawObject>();

    public void update() {
        if (!inited) init();
        //副本箭头
        updateCopyArraw();
        //小怪箭头
        updateMonsterArraw();
    }

    protected void updateCopyArraw() {
        if (copyArrawEnable)
        {
            //Vector3 t = worldCamera.WorldToScreenPoint(copyPosition);
            //t = uiCamera.ScreenToWorldPoint(t);

            //Vector3 s = worldCamera.WorldToScreenPoint(Player.instance.transform.position);
            //s = uiCamera.ScreenToWorldPoint(s);
            Vector3 t = new Vector3(copyPosition.x, copyPosition.z, 0);
            Vector3 position = Player.instance.transform.position;
            Vector3 s = new Vector3(position.x, position.z, 0);
            float angle = calcAngle(t, s);
            copyArraw.transform.eulerAngles = new Vector3(copyArraw.transform.eulerAngles.x, -90-angle, copyArraw.transform.eulerAngles.z);
            copyArraw.transform.position = new Vector3(position.x, position.y-0.2f, position.z);
        }
    }
    protected void updateMonsterArraw() {
        int length = monsterArraws.Count;
        for (int i = length - 1; i > -1; i--)
        {
            ArrawObject ao = monsterArraws[i];
            FightCharacter c = ao.c;
            if (c.destroyed || c.isDead() || Player.instance.isDead())//需要销毁
            {
                monsterArraws.RemoveAt(i);
                ao.destroy();
            }
            else
            {
                if (!UIManager.Instance.Active || BattleEngine.scene.pausing) {//不显示UI
                    ao.dirArraw.SetActive(false);
                    return;
                }
                if (!c.becameVisible)
                {
                    //ao.monsterArraw.transform.OverlayPosition(ao.c.model.transform.position, worldCamera, uiCamera);
                    //Vector3 mv = worldCamera.WorldToScreenPoint(ao.monsterArraw.transform.position);
                    //float angle = calcAngle(ao.monsterArraw.transform.position, playerObject.transform.position);

                    //==========================
                    Vector3 s = new Vector3(Player.instance.transform.position.x, Player.instance.transform.position.z, 0);
                    Vector3 t = new Vector3(ao.c.model.transform.position.x, ao.c.model.transform.position.z, 0);
                    float angle = calcAngle(t, s);
                    //==========================

                    angle += 45;
                    //if (mv.z < 0)
                    //{
                    //    if ((angle >= -90 - staticAngle && angle <= -90) || (angle<=90+staticAngle && angle>=staticAngle ))
                    //    {
                    //        angle -= 180;
                    //    }
                    //}
                    float x = 0, y = 0;
                    float tanAngle = 0;
                    if (angle >= 0)
                    {
                        if (angle <= staticAngle)//right 
                        {
                            tanAngle = angle;
                            x = rectW / 2;
                            y = Mathf.Tan(tanAngle * Mathf.PI / 180) * x;
                        }
                        else if (angle <= 180 - staticAngle)//top
                        {
                            tanAngle = 90-angle;
                            y = rectH / 2;
                            x = Mathf.Tan(tanAngle * Mathf.PI / 180) * y;
                        }
                        else if (angle <= 180 + staticAngle)//left
                        {
                            tanAngle = 180-angle;
                            x = -rectW / 2;
                            y = Mathf.Tan(tanAngle * Mathf.PI / 180) * Mathf.Abs(x);
                        }
                        else//bottom
                        {
                            tanAngle = 270 - angle;
                            y = -rectH / 2;
                            x = Mathf.Tan(tanAngle * Mathf.PI / 180) * y;
                        }
                    }
                    else
                    {
                        if (angle >= -staticAngle)//right
                        {
                            tanAngle = angle;
                            x = rectW / 2;
                            y = Mathf.Tan(tanAngle * Mathf.PI / 180) * x;
                        }
                        else if (angle >= -180 + staticAngle)//bottom
                        {
                            tanAngle = 90 + angle;
                            y = - rectH / 2;
                            x = Mathf.Tan(tanAngle * Mathf.PI / 180) * Mathf.Abs(y);
                        }
                        else if (angle >= -180 - staticAngle)//left
                        {
                            tanAngle = 180 + angle;
                            x = -rectW / 2;
                            y = -Mathf.Tan(tanAngle * Mathf.PI / 180) * Mathf.Abs(x);
                        }
                        else//top
                        {
                            tanAngle = 270 + angle;
                            y = rectH / 2;
                            x = -Mathf.Tan(tanAngle * Mathf.PI / 180) * Mathf.Abs(y);
                        }
                    }
                    ao.dirArraw.transform.position = uiCamera.ScreenToWorldPoint(new Vector3(x + Screen.width / 2, y + Screen.height / 2, 0));
                    ao.dirArraw.transform.eulerAngles = new Vector3(0, 0, angle);
                    ao.dirArraw.SetActive(true);
                }
                else
                {
                    ao.dirArraw.SetActive(false);
                }
            }
        }
    }
    public float calcAngle(Vector3 target, Vector3 source)
    {
        return Mathf.Atan2(target.y-source.y, target.x-source.x) * 180 / Mathf.PI;
    }

    /**小怪超出摄像机范围*/
    public void addEnemy(FightCharacter c) 
    {
        ArrawObject ao = new ArrawObject(c);
        monsterArraws.Add(ao);
    }

    public void showCopyArraw(Vector3 copyPosition) {
        this.copyArrawEnable = true;
        this.copyPosition = copyPosition;
        copyArraw = App.res.createSingle("Local/prefab/arraw/Point");
    }

    public void hideCopyArraw() {
        this.copyArrawEnable = false;
        GameObject.Destroy(copyArraw);
        this.copyArraw = null;

       
    }

    protected void init() {
       this.inited = true;

       staticAngle = calcAngle(new Vector3(rectW / 2, rectH / 2), Vector3.zero);
       worldCamera = Camera.main;
       uiCamera = UIManager.Instance.uiCamera;
       //playerObject = App.res.createSingle("Local/prefab/arraw/monsterArraw");
       //playerObject.transform.OverlayPosition(Player.instance.transform.position, worldCamera, uiCamera);
       //playerObject.SetActive(false);
    }

    public void clear() {
        inited = false;

        foreach(ArrawObject ao in monsterArraws){
            ao.destroy();
        }
        monsterArraws.Clear();
        //GameObject.Destroy(playerObject);
        staticAngle = calcAngle(new Vector3(rectW / 2, rectH / 2), Vector3.zero);
        worldCamera = null;
        uiCamera = null;
    }

}
