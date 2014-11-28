using UnityEngine;
using System.Collections;
using engine;

public class DungeonFinish{
    public static DungeonFinish instance = new DungeonFinish();

    private GameObject _instance;
    private UIButton restart;
    private BattleWin bw;
    public void show(bool win,BattleWin _bw) {
        bw = _bw;
        BattleUI.instance.setActive(false);
        _instance = Engine.res.createSingle("Local/prefab/battleui/dungeonfinish");
        Transform transform = _instance.transform;
        UISprite bg = transform.FindChild("bg").GetComponent<UISprite>();
        bg.alpha = 0.3f;
        UITexture tittlebg = transform.FindChild("tittlebg").GetComponent<UITexture>();
        UISprite tittle = transform.FindChild("tittle").GetComponent<UISprite>();
        restart = transform.FindChild("restart").GetComponent<UIButton>();
        UIButton back = transform.FindChild("back").GetComponent<UIButton>();
        back.enabled = false;
        back.state = UIButtonColor.State.Disabled;

        if (win || BattleEngine.dungeonWin){
            tittlebg.shader = null;
            tittle.spriteName = "wmtg";
        }
        else {
            tittlebg.shader = App.res.loadObject("Local/Shader/GrayShader") as Shader;
            tittle.spriteName = "zjzl"; 
        }
        Click.get(restart.gameObject).onClick = onAttackButtonClick;

        UIManager.Instance.Enable = true;
        PlayerDie.instance.setActive(false);
        App.input.setJoyEnabled(false);
    }

    private void onAttackButtonClick(GameObject button)
    {
        if (button == restart.gameObject)
        {
            GameObject.Destroy(_instance);
            BattleUI.instance.setActive(true);
            if(bw!=null) bw.onEnd();
            App.coroutine.StartCoroutine(reloadLevel());
        }
    }
    IEnumerator reloadLevel()
    {
        yield return new WaitForEndOfFrame();
        App.sceneManager.clear();
        Application.LoadLevel("map01");
    }
    
}
