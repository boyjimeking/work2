using UnityEngine;
using System.Collections;

public class RedScreen{

    public static RedScreen instance = new RedScreen();

    public Texture2D redtexture;
    public GameObject redObj;
    public bool played;
    public bool depalpha;
    public Color color;
    public void init()
    {
        if (redtexture == null)
        {
            createRedTexture();
        }
        redObj = new GameObject("red");
        redObj.transform.position = new Vector3(.5f, .5f, 1000);
        redObj.AddComponent<GUITexture>();
        color = new Color(Color.red.r,Color.red.g,Color.red.b,0.3f);
        redObj.guiTexture.texture = redtexture;
        redObj.SetActive(false);
    }
    public void createRedTexture()
    {
        Color color = Color.red;
        redtexture = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
        Color[] colors = new Color[Screen.width * Screen.height];
        for (int i = 0; i < Screen.width; i++)
        {
            for (int j = 0; j < Screen.height; j++)
            {
                int z = 0;
                if (i < Screen.width / 2) z = i;
                else z = (Screen.width - i);
                if (j<Screen.height/2 && j<z) z = j;
                else if((Screen.height-j)<z) z = Screen.height-j;
                color.a = 0.8f - z * 0.008f;
                colors[i + Screen.width*j] = color;
            }
        }
        redtexture.SetPixels(colors);
        redtexture.Apply();
    }
    public void begin()
    {
        played = true;
        if (redObj == null) init();
        redObj.SetActive(true);
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
        redObj.SetActive(true);
        if (color.a >= 0.3f) depalpha = true;
        if (color.a <= -0.1f) depalpha = false;
        if (depalpha)
        {
            color.a -= 0.003f;
            redObj.guiTexture.color = color;
        }
        else
        {
            color.a += 0.003f;
            redObj.guiTexture.color = color;
        }
    }

}
