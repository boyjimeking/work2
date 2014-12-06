using UnityEngine;
using System.Collections;
using engine;

public class Combo{

    public static Combo instance = new Combo();

    private GameObject go;
    private GameObject bg;
    private UISprite combo1,mask,combonum,flynum;
    private UILabel[] combos;
    private UILabel[] flyCombos;
    private int hitcount;
    private bool showed;
    private float life,currentLife,flyLife;
    public void init()
    {
        go = Engine.res.createSingle("Local/prefab/battleui/combo");
        Transform transform = go.transform;
        bg = transform.FindChild("bg").gameObject;
        combo1 = transform.FindChild("begin").GetComponent<UISprite>();
        mask = transform.FindChild("mask").GetComponent<UISprite>();
        combonum = transform.FindChild("combonum").GetComponent<UISprite>();
        flynum = (GameObject.Instantiate(combonum.gameObject) as GameObject).GetComponent<UISprite>();
        combos = new UILabel[3];
        flyCombos = new UILabel[3];
        for (int i = 0; i < 3; i++)
        {
           combos[i] = combonum.transform.FindChild("combonum" + (i + 1)).GetComponent<UILabel>();
           flyCombos[i] = flynum.transform.FindChild("combonum" + (i + 1)).GetComponent<UILabel>();
           flyCombos[i].gameObject.SetActive(false);
        }
        flynum.transform.parent = transform;
        showed = false;
        life = 5f;
        flyLife = 0.15f;
        mask.fillAmount = 0;
        mask.transform.localPosition = combo1.transform.localPosition;

        go.SetActive(false);
    }
    public void clear()
    {
        if (flyCombos != null) {
            foreach (UILabel fly in flyCombos) {
              if(fly&&fly.gameObject)  fly.gameObject.SetActive(false);
            }
        }
       
       if(go!=null) go.SetActive(false);
        showed = false;
        go = null;
    }
    public void playHit()
    {
        if (!showed)
        {
            showed = true;
            if (go == null)
            {
                init();
            }
            go.SetActive(true);
            hitcount = 0;
        }
        currentLife = 0;
        mask.fillAmount = 0;
       
        for(int i=0;i<combos.Length;i++){
            flyCombos[i].text = combos[i].text;
            flyCombos[i].fontSize = combos[i].fontSize;
            flyCombos[i].gameObject.SetActive(combos[i].gameObject.activeSelf);
        }
        //数字
        flynum.transform.localPosition = combonum.transform.localPosition;
        flynum.gameObject.SetActive(true);
        iTween.MoveTo(flynum.gameObject, iTween.Hash("x", flynum.transform.localPosition.x + 0.2f * Random.Range(-50f, 50f), "y", flynum.transform.localPosition.y + 10f, "easeType", iTween.EaseType.easeInCirc, "time", flyLife, "local", true));

        Hashtable scaletl = iTween.Hash("scale", Vector3.one * 0.6f, "easeType", iTween.EaseType.easeInExpo, "time", 0.1f);
        //combo
        combo1.transform.localScale = Vector3.one*0.7f;
        iTween.ScaleFrom(combo1.gameObject, scaletl);
        //背景
        bg.transform.localScale = Vector3.one * 0.7f;
        iTween.ScaleFrom(bg, scaletl);
        
        hitcount++;
        if (hitcount > 1000) hitcount = 1;
        setComboText();
    }
    public void setComboText()
    {
        combos[1].gameObject.SetActive(true);
        combos[0].gameObject.SetActive(hitcount >= 10);
        combos[2].gameObject.SetActive(hitcount >= 100);
        if (hitcount < 10)
        {
            combos[1].fontSize = 21;
            combos[1].text = hitcount.ToString();
        }
        else if (hitcount < 100)
        {
            combos[1].fontSize = 18;
            combos[0].text = (hitcount / 10).ToString();
            combos[1].text = (hitcount % 10).ToString();
        }
        else
        {
            combos[1].fontSize = 18;
            int c = hitcount % 100;
            combos[0].text = (hitcount / 100).ToString();
            combos[1].text = (c / 10).ToString();
            combos[2].text = (c % 10).ToString();
        }
    }


    public void update()
    {
        if (!showed) return;
        currentLife += Time.deltaTime;
        if (currentLife >= flyLife)
        {
            flynum.gameObject.SetActive(false);
        }
        if (currentLife >= life)
        {
            go.SetActive(false);
            flynum.gameObject.SetActive(false);
            hitcount = 0;
            showed = false;
        }
        else
        {
            mask.fillAmount = currentLife/life;
        }
    }

    public void setActive(bool value)
    {
        if (!showed || value) return;
        go.SetActive(value);
        flynum.gameObject.SetActive(false);
    }
}
