using UnityEngine;
using System.Collections;
using engine;

public class RedScreen{

    public static RedScreen instance = new RedScreen();

    public GameObject redObj;
    public UISprite redsprite;
    public bool played;
    public bool depalpha;
    public float delay;
    public void init()
    {
        redObj = Engine.res.createSingle("Local/prefab/battleui/redscreen");
        redsprite = redObj.GetComponentInChildren<UISprite>();
        redsprite.depth = -10;
        redsprite.width = Screen.width;
        redsprite.height = Screen.height;
        redObj.SetActive(false);
    }
    public void createRedTexture()
    {
        
    }
    public void clear()
    {
        stop();
    }
    public void begin()
    {
        played = true;
        if (redObj == null) init();
        redObj.SetActive(true);
        delay = 0;
    }

    public void stop()
    {
        played = false;
        if (redObj != null) redObj.SetActive(false);
    }
    public void update()
    {
        if (redObj == null) return;
        if (!played || !BattleUI.instance.enable || !UIManager.Instance.Enable || !BattleUI.instance.actived || Player.instance.isDead())
        {
            redObj.SetActive(false);
            return;
        }
        delay += Time.deltaTime;
        if (delay > 8)
        {
            redObj.SetActive(false);
            return;
        }
        redObj.SetActive(true);
        if (redsprite.alpha >= 0.8f) depalpha = true;
        if (redsprite.alpha <= 0.2f) depalpha = false;
        if (depalpha)
        {
            redsprite.alpha -= 0.003f;
        }
        else
        {
            redsprite.alpha += 0.003f;
        }
    }

}
