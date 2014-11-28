using UnityEngine;
using System.Collections;
using engine;

public class Combo{

    public static Combo instance = new Combo();

    private GameObject go;
    private GameObject bg;
    private UISprite combo1,mask;
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
        combos = new UILabel[3];
        flyCombos = new UILabel[3];
        for (int i = 0; i < 3; i++)
        {
           combos[i] = transform.FindChild("combonum"+(i+1)).GetComponent<UILabel>();
           GameObject combogo = GameObject.Instantiate(combos[i].gameObject) as GameObject;
           flyCombos[i] = combogo.GetComponent<UILabel>();
           flyCombos[i].gameObject.SetActive(false);
        }
        
        showed = false;
        life = 5f;
        flyLife = 0.15f;
        mask.fillAmount = 0;
        mask.transform.position = combo1.transform.position;

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
            bool isActive = combos[i].gameObject.active;
            flyCombos[i].gameObject.SetActive(isActive);
            if (isActive)
            {
                flyCombos[i].transform.position = combos[i].transform.position;
                iTween.MoveTo(flyCombos[i].gameObject, iTween.Hash("x", flyCombos[i].transform.position.x + 0.2f * Random.Range(-1f,1f), "y", flyCombos[i].transform.position.y + 0.3f, "easeType", iTween.EaseType.easeInCirc, "time", flyLife));
                Hashtable ht = iTween.Hash("rotation", NGUITools.RandomRange(0, 360), "easeType", iTween.EaseType.linear, "time", flyLife);
                iTween.RotateTo(flyCombos[i].gameObject, ht);
            }
        }
       
        combo1.transform.position = mask.transform.position;
        if (hitcount > 0) iTween.MoveFrom(combo1.gameObject, iTween.Hash("x", combo1.transform.position.x - 0.2f, "y", combo1.transform.position.y-0.1f, "easeType", iTween.EaseType.easeInOutBack, "time", 0.1f));
        Hashtable scaleht = iTween.Hash("scale", Vector3.one*0.8f, "easeType", iTween.EaseType.easeInExpo, "time", 0.1f);
        bg.transform.localScale = Vector3.one * 0.7f;
        iTween.ScaleFrom(bg, scaleht);
        
        hitcount++;
        if (hitcount < 10)
        {
            combos[0].gameObject.SetActive(false);
            combos[1].gameObject.SetActive(true);
            combos[2].gameObject.SetActive(false);
            combos[1].fontSize = 21;
            combos[1].text = hitcount.ToString();
        }
        else if (hitcount < 100)
        {
            combos[0].gameObject.SetActive(true);
            combos[1].gameObject.SetActive(true);
            combos[2].gameObject.SetActive(false);
            combos[1].fontSize = 18;
            combos[0].text = (hitcount / 10).ToString();
            combos[1].text = (hitcount % 10).ToString();
        }
        else if (hitcount < 1000)
        {
            combos[0].gameObject.SetActive(true);
            combos[1].gameObject.SetActive(true);
            combos[2].gameObject.SetActive(true);
            combos[1].fontSize = 18;
            int c = hitcount % 100;
            combos[0].text = (hitcount / 100).ToString();
            combos[1].text = (c / 10).ToString();
            combos[2].text = (c % 10).ToString();
        }
        else
        {
            hitcount = 1;
            combos[0].gameObject.SetActive(false);
            combos[1].gameObject.SetActive(true);
            combos[2].gameObject.SetActive(false);
            combos[1].fontSize = 30;
            combos[1].text = hitcount.ToString();
        }
    }
  
    public void update()
    {
        if (!showed) return;
        currentLife += Time.deltaTime;
        if (currentLife >= flyLife)
        {
            foreach (UILabel fly in flyCombos)
            {
                fly.gameObject.SetActive(false);
            }
        }
        if (currentLife >= life)
        {
            go.SetActive(false);
            foreach (UILabel fly in flyCombos)
            {
                fly.gameObject.SetActive(false);
            }
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
        foreach (UILabel fly in flyCombos)
        {
            fly.gameObject.SetActive(false);
        }
    }
}
