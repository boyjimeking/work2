using UnityEngine;
using System.Collections;
using engine;

public class BossHead{
    public static BossHead instance = new BossHead();

    public Monster c;


    public GameObject ui_instance;
    private GameObject tweengo;
    private GameObject cdgo;
    private GameObject namego;
    private GameObject bloodgo;
    private UISprite blood;
    private UILabel mLabelBossName;
    private UILabel mLabelCDTime;
    private int mBossBlood; //boos blood
    private bool mIsDirect = true;
    private string mBossName = "";
    public float testBlood = 1;
    private GameObject mParent;
    private float bloodw;
    private float time;

    public void init() {

        GameObject namePrefab = Engine.res.loadPrefab("Local/prefab/battleui/bossHead");
        ui_instance = GameObject.Instantiate(namePrefab) as GameObject;
        tweengo = ui_instance.getChild("group");
        bloodgo = tweengo.getChild("bloodGroup");
        cdgo = tweengo.getChild("cd");
        namego = tweengo.getChild("name");
        blood = bloodgo.getChildComponent<UISprite>("blood");
        mLabelBossName = namego.getChild("LabelBossName").GetComponent<UILabel>();
        mLabelCDTime = cdgo.getChild("LabelCDTime").GetComponent<UILabel>();    
        bloodw = blood.width;
        blood.gameObject.addOnce<ChangeHp>();

        bloodgo.SetActive(false);
        cdgo.SetActive(true);
        namego.SetActive(false);
        tweengo.transform.localPosition = new Vector3(tweengo.transform.localPosition.x, 55, 0);
        time = 0;
        mLabelCDTime.text = "挑战耗时： " + timeToString(0);
    }

    public void play(bool show)
    {
        if (show)
        {
            bloodgo.SetActive(true);
            cdgo.SetActive(true);
            namego.SetActive(true);
            Hashtable ht = iTween.Hash("y", 0, "islocal", true, "easetype", iTween.EaseType.easeInExpo, "time", 0.3f);
            iTween.MoveTo(tweengo, ht);
        }
        else
        {
            bloodgo.SetActive(false);
            cdgo.SetActive(true);
            namego.SetActive(false);
            Hashtable ht = iTween.Hash("y", 55, "islocal", true, "easetype", iTween.EaseType.easeInExpo, "time", 0.3f);
            iTween.MoveTo(tweengo, ht);
        }
    }

    public void reset(Monster c) {
        this.c = c;
        //ui_instance.SetActive(true);
        updateBlood();
        setBossName(c.charTemplate.name);
        play(true);
    }

    public void hide() {
        this.c = null;
        play(false);
    }
    public void clear()
    {
        hide();
        time = 0;
        mLabelCDTime.text = "挑战耗时： " + timeToString(0);
    }
    public void updateBlood() {
        if (c == null) return;
        setBossBlood(c.data.hp, c.data.maxhp);
    }

    public void setBossBlood(int HP, int maxHP)
    {
        float current,value;
        if (blood.type == UISprite.Type.Filled)
        {
            value = (float)HP / maxHP;
            current = blood.fillAmount;
        }
        else
        {
            value = (int)(((float)HP) / maxHP * bloodw);
            current = blood.width;
        }
        Hashtable ht = iTween.Hash("from", current, "to", value, "time", 0.5f, "easeType",
               iTween.EaseType.easeInCubic, "onupdate", "changeHp");
        iTween.ValueTo(blood.gameObject, ht);
    }

    /// <summary>
    /// set boos name
    /// </summary>
    /// <param name="bossName"></param>
    public void setBossName(string bossName)
    {
        mBossName = bossName;
        mLabelBossName.text = mBossName;

    }


    /// <summary>
    ///  boss 挑战CD时间
    /// </summary>
    /// <param name="strCDTime"></param>
    public void setBossCDTime(string strCDTime)
    {
        mLabelCDTime.text = strCDTime;
    }

    public void setActive(bool value)
    {
        ui_instance.SetActive(value);
    }

    public void update()
    {
        if (ui_instance.activeSelf)
        {
            time += Time.deltaTime;
            string timestr = timeToString((int)time);
            mLabelCDTime.text = "挑战耗时： " + timestr;
        }
    }
    private string timeToString(int seconds)
    {
        int h_ = seconds / 3600;
        int m_ = (seconds - h_ * 3600) / 60;
        int s_ = seconds - h_ * 3600 - m_ * 60;
        string h = h_ < 10 ? ("0" + h_) : h_ + "";
        string m = m_ < 10 ? ("0" + m_) : m_ + "";
        string s = s_ < 10 ? ("0" + s_) : s_ + "";
        return h + ":" + m + ":" + s;
    }

    class ChangeHp : MonoBehaviour
    {
        public UISprite blood;

        void changeHp(float value)
        {
            if (blood == null)
            {
                blood = gameObject.GetComponent<UISprite>();
            }
            if (blood.type == UISprite.Type.Filled)
                blood.fillAmount = value;
            else
                blood.width = (int)value;
        }
    }
}
