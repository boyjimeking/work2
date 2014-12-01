using UnityEngine;
using System.Collections;
using engine;

public class PauseAndAuto
{

    public static PauseAndAuto instance = new PauseAndAuto();

    public GameObject go;
    public UISprite puase, auto;
    public bool isPuase, isAuto;
    
    public void init()
    {
        go = Engine.res.createSingle("Local/prefab/battleui/pauseandauto");
        Transform transform = go.transform;
        puase = transform.FindChild("pause").GetComponent<UISprite>();
        auto = transform.FindChild("auto").GetComponent<UISprite>();
        addClick(puase.gameObject);
        addClick(auto.gameObject);
    }
    private void addClick(GameObject go)
    {
        Click click = Click.get(go);
        click.onClick = onAttackButtonClick;
        click.onPress = onAttackButtonPress;
    }
    private void onAttackButtonPress(GameObject button, bool isPressed)
    {
        if (isPressed)button.transform.localScale = Vector3.one * 1.1f;
        else button.transform.localScale = Vector3.one;
    }
    private void onAttackButtonClick(GameObject button)
    {
        if (!BattleUI.instance.enable || Player.instance.isDead()) return;
        if (button == auto.gameObject)
        {
            
            Player.instance.resetAuto(!Player.instance.autoFight);
            isAuto = Player.instance.autoFight;
            if (isAuto) TweenColor.Begin(auto.gameObject, 0.1f, Color.yellow);
            else TweenColor.Begin(auto.gameObject, 0.1f, Color.white);
            
        }
        else
        {
            isPuase = !isPuase;
            if (isPuase) TweenColor.Begin(puase.gameObject, 0.1f, Color.yellow);
            else TweenColor.Begin(puase.gameObject, 0.1f, Color.white); 
        }

    }
    public void clear()
    {
        isPuase = false;
        isAuto = false;
        TweenColor.Begin(auto.gameObject, 0.1f, Color.white); 
        TweenColor.Begin(puase.gameObject, 0.1f, Color.white); 
    }

    public void setActive(bool value)
    {
        go.SetActive(value);
    }
}
